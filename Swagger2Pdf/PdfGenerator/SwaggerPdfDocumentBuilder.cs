using System.IO;
using System.Linq;
using System.Reflection;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using log4net;
using Swagger2Pdf.PdfGenerator.Model;

namespace Swagger2Pdf.PdfGenerator
{
    public class SwaggerPdfDocumentBuilder
    {
        private readonly Document _document;

        private static readonly ILog Logger = LogManager.GetLogger(Assembly.GetEntryAssembly().GetName().Name);

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
                    var image = document.AddImage(imageFile.FullName);
                    image.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                }
            }

            document.AddParagraph(swaggerDocumentModel.Title, p => p.AsTitle().Centered());
            document.AddParagraph(swaggerDocumentModel.Author, p => p.AsHeader().Centered());
            document.AddParagraph(swaggerDocumentModel.Version, p => p.Centered());
            document.AddParagraph(swaggerDocumentModel.DocumentDate.ToShortDateString(), p => p.Centered());
            //document.Footers.Primary.AddParagraph().AsSubHeader().PullRight().AddPageField();
        }

        private static void DrawAuthorizationInfoPage(Document document, SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            document.AddAreaBreak();
            document.AddParagraph("Authorization information", p => p.AsHeader());
            document.AddParagraph();
            foreach (var authorizationInfo in swaggerDocumentModel.AuthorizationInfos)
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
                //document.Footers.Primary.AddParagraph().AsSubHeader().PullRight().AddPageField();
            }
        }

        private static void DrawPathParameters(EndpointInfo docEntry, Document document)
        {
            if (docEntry.PathParameters != null && docEntry.PathParameters.Any())
            {
                document.AddParagraph();
                document.AddParagraph("Path parameters", p => p.AsSubHeader());
                var table = PdfHelpers.CreateBorderedTable(new float[] {25, 25, 25, 25});
                
                table.AddHeaderCell("Name");
                table.AddHeaderCell("Type");
                table.AddHeaderCell("Schema");
                table.AddHeaderCell("Description");

                foreach (var pathParameter in docEntry.PathParameters)
                {
                    table = table.StartNewRow();
                    table.AddCell(new Cell().VerticallyCenteredContent().AddParagraph(pathParameter.Name ?? ""));
                    table.AddCell(new Cell().VerticallyCenteredContent().AddParagraph(pathParameter.Type ?? ""));
                    var schema = SwaggerPdfJsonConvert.SerializeObject(pathParameter.Schema);
                    if (schema != "null")
                    {
                        var schemaParagraph = new Paragraph(schema).AsFixedCharLength();
                        table.AddCell(new Cell().VerticallyCenteredContent().Add(schemaParagraph));
                    }

                    var description = new Paragraph(pathParameter.Description ?? "");
                    pathParameter.Schema?.WriteDetailedDescription(description);
                    table.AddCell(new Cell().VerticallyCenteredContent().Add(description));
                }

                document.Add(table);
            }
        }

        private static void DrawResponses(EndpointInfo docEntry, Document document)
        {
            if (docEntry.Responses != null)
            {
                document.AddParagraph();
                document.AddParagraph("Responses", p => p.AsSubHeader());

                foreach (var response in docEntry.Responses)
                {
                    document.AddPageBreakableParagraph($"{response.Code}: {response.Description}", p => p.AddBorders());
                    var responseBody = SwaggerPdfJsonConvert.SerializeObject(response.Schema);
                    if (responseBody != "null")
                    {
                        document.AddPageBreakableParagraph(responseBody, p => p.AddBorders().AsFixedCharLength());
                    }

                    document.AddParagraph();
                }
            }
        }

        private static void DrawEndpointHeader(EndpointInfo docEntry, Document document)
        {
            var deprecatedStyle = new Style();
            deprecatedStyle.SetFontColor(ColorConstants.RED);
            deprecatedStyle.SetBold();
            

            document.AddParagraph($"{docEntry.HttpMethod} {docEntry.EndpointPath}", p =>
            {
                p.AsHeader();
                if (docEntry.Deprecated)
                {
                    p.AddStyle(deprecatedStyle);
                }
            });
            document.AddParagraph(docEntry.Summary, p =>
            {
                if (docEntry.Deprecated)
                {
                    p.AddStyle(deprecatedStyle);
                }
            });
            if (docEntry.Deprecated)
            {
                document.AddParagraph("Deprecated", p => p.AddStyle(deprecatedStyle));
            }
        }

        private static void DrawBodyParameters(EndpointInfo docEntry, Document document)
        {
            if (docEntry.BodyParameters != null && docEntry.BodyParameters.Any())
            {
                document.AddParagraph();
                document.AddParagraph("Request body", p => p.AsSubHeader());

                foreach (var bodyParameter in docEntry.BodyParameters)
                {   
                    document.AddPageBreakableParagraph(SwaggerPdfJsonConvert.SerializeObject(bodyParameter.Schema), p => p.AsFixedCharLength().AddBorders());
                }
            }
        }

        private static void DrawFormDataParameters(EndpointInfo docEntry, Document document)
        {
            if (docEntry.FormDataParameters != null && docEntry.FormDataParameters.Any())
            {
                document.AddParagraph();
                document.AddParagraph("Form data parameters", p => p.AsSubHeader());
                var table = PdfHelpers.CreateBorderedTable(new float[] {33, 33, 33});
                
                table.AddHeaderCell("Parameter name");
                table.AddHeaderCell("Type");
                table.AddHeaderCell("Description");

                foreach (var parameter in docEntry.FormDataParameters)
                {
                    table = table.StartNewRow();
                    table.AddCell(parameter.Name ?? "");
                    table.AddCell(parameter.Type ?? "");

                    var description = new Paragraph(parameter.Description ?? "");
                    parameter.Schema?.WriteDetailedDescription(description);
                    table.AddCell(description);
                }

                document.Add(table);
            }
        }

        private static void DrawUrlParameters(EndpointInfo docEntry, Document document)
        {
            if (docEntry.UrlParameters != null && docEntry.UrlParameters.Any())
            {
                document.AddParagraph();
                document.AddParagraph("Query string parameters", p => p.AsSubHeader());
                var table = PdfHelpers.CreateBorderedTable(new float[] {25, 25, 25, 25});
                table.AddHeaderCell("Name");
                table.AddHeaderCell("Type");
                table.AddHeaderCell("Schema");
                table.AddHeaderCell("Description");

                foreach (var queryParameter in docEntry.UrlParameters)
                {
                    table = table.StartNewRow();
                    table.AddCell(queryParameter.Name ?? "");
                    table.AddCell(queryParameter.Type ?? "");
                    var schema = SwaggerPdfJsonConvert.SerializeObject(queryParameter.Schema);
                    if (schema != "null")
                    {
                        var paragraph = new Paragraph(schema);
                        paragraph.AddStyle(PdfHelpers.FixedCharLengthStyle());
                        table.AddCell(paragraph);
                    }

                    var description = new Paragraph(queryParameter.Description ?? "");
                    queryParameter.Schema?.WriteDetailedDescription(description);
                    table.AddCell(description);
                }

                document.Add(table);
            }
        }
    }
}
