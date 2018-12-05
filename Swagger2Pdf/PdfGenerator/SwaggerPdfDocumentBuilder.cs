using System.IO;
using System.Linq;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using log4net;
using Swagger2Pdf.PdfGenerator.Helpers;
using Swagger2Pdf.PdfGenerator.Model;

namespace Swagger2Pdf.PdfGenerator
{
    public class SwaggerPdfDocumentBuilder
    {
        private readonly Document _document;

        public static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        public SwaggerPdfDocumentBuilder(SwaggerPdfDocumentModel model)
        {
            var pdfDocument = new PdfDocument(new PdfWriter(model.PdfDocumentPath));
            _document = new Document(pdfDocument);
        }

        public void BuildPdf(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            Logger.Info("Building pdf document");

            Logger.Info("Drawing welcome page");
            DrawWelcomePage(_document, swaggerDocumentModel);
            Logger.Info("Drawing welcome page done.");

            Logger.Info("Drawing authorization info page");
            DrawAuthorizationInfoPage(_document, swaggerDocumentModel);
            Logger.Info("Drawing authorization info page done.");

            Logger.Info("Drawing drawing endpoint documentation");
            DrawEndpointDocumentation(_document, swaggerDocumentModel);
            Logger.Info("Drawing drawing endpoint documentation done.");

            Logger.Info("Rendering PDF document");
            var fi = new FileInfo(swaggerDocumentModel.PdfDocumentPath);
            Logger.Info($"Saving PDF document to: {fi.FullName}");
            Logger.Info("Done");
            _document.Close();
        }

        private static void DrawWelcomePage(Document document, SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            if (!string.IsNullOrEmpty(swaggerDocumentModel.WelcomePageImage))
            {
                var imageFile = new FileInfo(swaggerDocumentModel.WelcomePageImage);
                if (imageFile.Exists)
                {
                    document.AddCenteredImage(imageFile.FullName, i => i.SetMarginTop(150));
                }
            }
            else
            {
                document.Add(new Div().SetWidth(100).SetHeight(300));
            }

            document.AddParagraph(p => p.Centered().AddText(swaggerDocumentModel.Title).AsTitle());
            document.AddParagraph(p => p.Centered().AddText(swaggerDocumentModel.Author).AsHeader());
            document.AddParagraph(p => p.Centered().AddText(swaggerDocumentModel.Version));
            document.AddParagraph(p => p.Centered().AddText(swaggerDocumentModel.DocumentDate.ToShortDateString()));
        }

        private static void DrawAuthorizationInfoPage(Document document, SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            document.AddAreaBreak();
            document.AddParagraph("Authorization information", p => p.AsHeader());
            document.AddParagraph();
            foreach (var authorizationInfo in swaggerDocumentModel.AuthorizationInfo)
            {
                document.AddParagraph($"Authorization option: {authorizationInfo.Key}", p => p.AsSubHeader().Bold());
                document.AddParagraph();
                document.AddParagraph(p => authorizationInfo.Value.WriteAuthorizationInfo(p));
                document.AddParagraph();
            }
        }

        private static void DrawEndpointDocumentation(Document document, SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            foreach (var docEntry in swaggerDocumentModel.DocumentationEntries)
            {
                document.AddAreaBreak();

                DrawEndpointHeader(docEntry, document);
                DrawPathParameters(docEntry, document);
                DrawUrlParameters(docEntry, document);
                DrawFormDataParameters(docEntry, document);
                DrawBodyParameters(docEntry, document);
                DrawResponses(docEntry, document);
            }
        }

        private static void DrawPathParameters(EndpointInfo docEntry, Document document)
        {
            if (docEntry.PathParameters == null || !docEntry.PathParameters.Any()) return;

            document.AddParagraph();
            document.AddParagraph("Path parameters", p => p.AsSubHeader());
            var table = ParagraphHelper.CreateBorderedTable(new float[] {25, 25, 25, 25});
                
            table.AddHeaderCell("Name");
            table.AddHeaderCell("Type");
            table.AddHeaderCell("Schema");
            table.AddHeaderCell("Description");

            foreach (var pathParameter in docEntry.PathParameters)
            {
                table = table.StartNewRow();
                table.AddCell(new Cell().VerticallyCentered().AddParagraph(pathParameter.Name ?? ""));
                table.AddCell(new Cell().VerticallyCentered().AddParagraph(pathParameter.Type ?? ""));
                if (pathParameter.Schema != null)
                {
                    var schema = SwaggerPdfJsonConvert.SerializeObject(pathParameter.Schema);
                    var schemaParagraph = new Paragraph(schema).AsFixedCharLength();
                    table.AddCell(new Cell().VerticallyCentered().Add(schemaParagraph));
                }
                else
                {
                    table.AddCell(string.Empty);
                }

                var description = new Paragraph();
                pathParameter.WriteDetailedDescription(description);
                table.AddCell(new Cell().VerticallyCentered().Add(description));
            }

            document.Add(table);
        }

        private static void DrawResponses(EndpointInfo docEntry, Document document)
        {
            if (docEntry.Responses == null) return;

            document.AddParagraph();
            document.AddParagraph("Responses", p => p.AsSubHeader());

            foreach (var response in docEntry.Responses)
            {
                document.AddPageBreakableParagraph($"{response.Code}: {response.Description}", p => p.AddBorders());
                if (response.Schema != null)
                {
                    var responseBody = SwaggerPdfJsonConvert.SerializeObject(response.Schema);
                    document.AddPageBreakableParagraph(responseBody, p => p.AddBorders().AsFixedCharLength());
                }

                document.AddParagraph();
            }
        }

        private static void DrawEndpointHeader(EndpointInfo docEntry, Document document)
        {
            if (docEntry.Deprecated)
            {
                document.AddParagraph($"{docEntry.HttpMethod} {docEntry.EndpointPath}", p => p.AsHeader().Bold().SetLineThrough());
                document.AddParagraph(docEntry.Summary, p => p.Bold().SetLineThrough());
                document.AddParagraph("Deprecated", p => p.Bold().SetFontColor(ColorConstants.RED));
            }
            else
            {
                document.AddParagraph($"{docEntry.HttpMethod} {docEntry.EndpointPath}", p => p.AsHeader());
                document.AddParagraph(docEntry.Summary);
            }
        }

        private static void DrawBodyParameters(EndpointInfo docEntry, Document document)
        {
            if (docEntry.BodyParameters == null || !docEntry.BodyParameters.Any()) return;

            document.AddParagraph();
            document.AddParagraph("Request body", p => p.AsSubHeader());

            foreach (var bodyParameter in docEntry.BodyParameters)
            {   
                document.AddPageBreakableParagraph(SwaggerPdfJsonConvert.SerializeObject(bodyParameter.Schema), p => p.AsFixedCharLength().AddBorders());
            }
        }

        private static void DrawFormDataParameters(EndpointInfo docEntry, Document document)
        {
            if (docEntry.FormDataParameters == null || !docEntry.FormDataParameters.Any()) return;

            document.AddParagraph();
            document.AddParagraph("Form data parameters", p => p.AsSubHeader());
            var table = ParagraphHelper.CreateBorderedTable(new float[] {33, 33, 33});
                
            table.AddHeaderCell("Parameter name");
            table.AddHeaderCell("Type");
            table.AddHeaderCell("Description");

            foreach (var parameter in docEntry.FormDataParameters)
            {
                table = table.StartNewRow();
                table.AddCell(parameter.Name ?? "");
                table.AddCell(parameter.Type ?? "");

                var description = new Paragraph();
                parameter.WriteDetailedDescription(description);
                table.AddCell(description);
            }

            document.Add(table);
        }

        private static void DrawUrlParameters(EndpointInfo docEntry, Document document)
        {
            if (docEntry.QueryParameter == null || !docEntry.QueryParameter.Any()) return;

            document.AddParagraph();
            document.AddParagraph("Query string parameters", p => p.AsSubHeader());
            var table = ParagraphHelper.CreateBorderedTable(new float[] {25, 25, 25, 25});
            table.AddHeaderCell("Name");
            table.AddHeaderCell("Type");
            table.AddHeaderCell("Schema");
            table.AddHeaderCell("Description");

            foreach (var queryParameter in docEntry.QueryParameter)
            {
                table = table.StartNewRow();
                table.AddCell(queryParameter.Name ?? "");
                table.AddCell(queryParameter.Type ?? "");
                if (queryParameter.Schema != null)
                {
                    var schema = SwaggerPdfJsonConvert.SerializeObject(queryParameter.Schema);
                    var paragraph = new Paragraph(schema);
                    paragraph.AsFixedCharLength();
                    table.AddCell(paragraph);
                }
                else
                {
                    table.AddCell(string.Empty);
                }

                var description = new Paragraph();
                queryParameter.WriteDetailedDescription(description);
                table.AddCell(description);
            }

            document.Add(table);
        }
    }
}
