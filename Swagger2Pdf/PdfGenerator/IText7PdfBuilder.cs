using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iText.Html2pdf;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using log4net;
using Swagger2Pdf.PdfGenerator.Helpers;
using Swagger2Pdf.PdfGenerator.Model;

namespace Swagger2Pdf.PdfGenerator
{
    public class iText7PdfBuilder : PdfBuilderBase
    {
        private readonly Document _document;

        public static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        public iText7PdfBuilder(SwaggerPdfDocumentModel model)
        {
            var pdfDocument = new PdfDocument(new PdfWriter(model.PdfDocumentPath));
            _document = new Document(pdfDocument);
        }

        protected override void DrawWelcomePage(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            if (!string.IsNullOrEmpty(swaggerDocumentModel.WelcomePageImage))
            {
                var imageFile = new FileInfo(swaggerDocumentModel.WelcomePageImage);
                if (imageFile.Exists)
                {
                    _document.AddCenteredImage(imageFile.FullName, i => i.SetMarginTop(150));
                }
            }
            else
            {
                _document.Add(new Div().SetWidth(100).SetHeight(300));
            }

            _document.AddParagraph(p => p.Centered().AddText(swaggerDocumentModel.Title).AsTitle());
            _document.AddParagraph(p => p.Centered().AddText(swaggerDocumentModel.Author).AsHeader());
            _document.AddParagraph(p => p.Centered().AddText(swaggerDocumentModel.Version));
            _document.AddParagraph(p => p.Centered().AddText(swaggerDocumentModel.DocumentDate.ToShortDateString()));
        }

        protected override void WriteCustomPage(StringWriter writer)
        {
            var sb = new StringBuilder();
            sb.Append("<html><head><style>");
            sb.Append(Properties.Resources.github_markdown);
            sb.Append("</style></head><body class=\"markdown-body\">");
            sb.Append(writer.GetStringBuilder());
            sb.Append("</body>");
            var htmlString = sb.ToString();
            var elements = HtmlConverter.ConvertToElements(htmlString);
            foreach (var element in elements)
            {
                _document.Add((IBlockElement)element);
            }
        }

        protected override void SaveDocument(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            _document.Close();
        }

        protected override void DrawAuthorizationInfoPage(SwaggerPdfDocumentModel swaggerDocumentModel)
        {   
            _document.AddParagraph("Authorization information", p => p.AsHeader());
            _document.AddParagraph();
            foreach (var authorizationInfo in swaggerDocumentModel.AuthorizationInfo)
            {
                _document.AddParagraph($"Authorization option: {authorizationInfo.Key}", p => p.AsSubHeader().Bold());
                _document.AddParagraph();
                _document.AddParagraph(p => authorizationInfo.Value.WriteAuthorizationInfo(p));
                _document.AddParagraph();
            }
        }

        protected override void BeginNewPage()
        {
            _document.AddAreaBreak();
        }

        protected override void DrawPathParameters(List<Parameter> docEntryPathParameters)
        {
            _document.AddParagraph();
            _document.AddParagraph("Path parameters", p => p.AsSubHeader());
            var table = ParagraphHelper.CreateBorderedTable(new float[] { 25, 25, 25, 25 });

            table.AddHeaderCell("Name");
            table.AddHeaderCell("Type");
            table.AddHeaderCell("Schema");
            table.AddHeaderCell("Description");

            foreach (var pathParameter in docEntryPathParameters)
            {
                table = table.StartNewRow();
                table.AddCell(new Cell().VerticallyCentered().AddParagraph(pathParameter.Name ?? ""));
                table.AddCell(new Cell().VerticallyCentered().AddParagraph(pathParameter.Type ?? ""));
                if (pathParameter.Schema == null)
                {
                    table.AddCell(string.Empty);
                }
                else
                {
                    var schema = SwaggerPdfJsonConvert.SerializeObject(pathParameter.Schema);
                    var schemaParagraph = new Paragraph(schema).AsFixedCharLength();
                    table.AddCell(new Cell().VerticallyCentered().Add(schemaParagraph));
                }

                var description = new Paragraph();
                pathParameter.WriteDetailedDescription(description);
                table.AddCell(new Cell().VerticallyCentered().Add(description));
            }

            _document.Add(table);
        }

        protected override void DrawResponses(List<Response> docEntryResponses)
        {
            if (docEntryResponses == null) return;

            _document.AddParagraph();
            _document.AddParagraph("Responses", p => p.AsSubHeader());

            foreach (var response in docEntryResponses)
            {
                _document.AddPageBreakableParagraph($"{response.Code}: {response.Description}", p => p.AddBorders());
                if (response.Schema != null)
                {
                    var responseBody = SwaggerPdfJsonConvert.SerializeObject(response.Schema);
                    _document.AddPageBreakableParagraph(responseBody, p => p.AddBorders().AsFixedCharLength());
                }

                _document.AddParagraph();
            }
        }

        protected override void DrawEndpointHeader(EndpointInfo docEntry)
        {
            if (docEntry.Deprecated)
            {
                _document.AddParagraph($"{docEntry.HttpMethod} {docEntry.EndpointPath}", p => p.AsHeader().Bold().SetLineThrough());
                _document.AddParagraph(docEntry.Summary, p => p.Bold().SetLineThrough());
                _document.AddParagraph("Deprecated", p => p.Bold().SetFontColor(ColorConstants.RED));
            }
            else
            {
                _document.AddParagraph($"{docEntry.HttpMethod} {docEntry.EndpointPath}", p => p.AsHeader());
                _document.AddParagraph(docEntry.Summary);
            }
        }

        protected override void DrawBodyParameters(List<Parameter> docEntryBodyParameters)
        {
            if (docEntryBodyParameters == null || !docEntryBodyParameters.Any()) return;

            _document.AddParagraph();
            _document.AddParagraph("Request body", p => p.AsSubHeader());

            foreach (var bodyParameter in docEntryBodyParameters)
            {
                _document.AddPageBreakableParagraph(SwaggerPdfJsonConvert.SerializeObject(bodyParameter.Schema), p => p.AsFixedCharLength().AddBorders());
            }
        }

        protected override void DrawFormDataParameters(List<Parameter> docEntryFormDataParameters)
        {
            if (docEntryFormDataParameters == null || !docEntryFormDataParameters.Any()) return;

            _document.AddParagraph();
            _document.AddParagraph("Form data parameters", p => p.AsSubHeader());
            var table = ParagraphHelper.CreateBorderedTable(new float[] { 33, 33, 33 });

            table.AddHeaderCell("Parameter name");
            table.AddHeaderCell("Type");
            table.AddHeaderCell("Description");

            foreach (var parameter in docEntryFormDataParameters)
            {
                table = table.StartNewRow();
                table.AddCell(parameter.Name ?? "");
                table.AddCell(parameter.Type ?? "");

                var description = new Paragraph();
                parameter.WriteDetailedDescription(description);
                table.AddCell(description);
            }

            _document.Add(table);
        }

        protected override void DrawQueryParameters(List<Parameter> docEntryQueryParameter)
        {
            if (docEntryQueryParameter == null || !docEntryQueryParameter.Any()) return;

            _document.AddParagraph();
            _document.AddParagraph("Query string parameters", p => p.AsSubHeader());
            var table = ParagraphHelper.CreateBorderedTable(new float[] { 25, 25, 25, 25 });
            table.AddHeaderCell("Name");
            table.AddHeaderCell("Type");
            table.AddHeaderCell("Schema");
            table.AddHeaderCell("Description");

            foreach (var queryParameter in docEntryQueryParameter)
            {
                table = table.StartNewRow();
                table.AddCell(queryParameter.Name ?? "");
                table.AddCell(queryParameter.Type ?? "");
                if (queryParameter.Schema == null)
                {
                    table.AddCell(string.Empty);
                }
                else
                {
                    var schema = SwaggerPdfJsonConvert.SerializeObject(queryParameter.Schema);
                    var paragraph = new Paragraph(schema);
                    paragraph.AsFixedCharLength();
                    table.AddCell(paragraph);
                }

                var description = new Paragraph();
                queryParameter.WriteDetailedDescription(description);
                table.AddCell(description);
            }

            _document.Add(table);
        }
    }
}
