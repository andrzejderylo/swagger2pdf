using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using Swagger2Pdf.PdfGenerator.Model;

namespace Swagger2Pdf.PdfGenerator
{
    public class SwaggerPdfDocumentBuilder
    {
        private readonly Document _document;
        private static readonly ILog Logger = LogManager.GetLogger(Assembly.GetEntryAssembly().GetName().Name);

        public SwaggerPdfDocumentBuilder()
        {
            _document = new Document();
        }

        public void BuildPdf(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            Logger.Info("Building pdf document");
            _document.DefineStyles();

            Logger.Info("Drawing welcome page");
            DrawWelcomePage(_document, swaggerDocumentModel);

            Logger.Info("Drawing authorization info page");
            DrawAuthorizationInfoPage(_document, swaggerDocumentModel);

            Logger.Info("Drawing drawing endpoint documentation");
            DrawEndpointDocumentation(_document, swaggerDocumentModel);

            var renderer = new PdfDocumentRenderer { Document = _document };

            Logger.Info("Rendering PDF document");
            renderer.RenderDocument();

            var fi = new FileInfo(swaggerDocumentModel.PdfDocumentPath);
            Logger.Info($"Saving PDF document to: {fi.FullName}");
            renderer.PdfDocument.Save(swaggerDocumentModel.PdfDocumentPath);
        }

        private static void DrawWelcomePage(Document pdfDocument, SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            var welcomeSection = pdfDocument.AddSection();
            if (!string.IsNullOrEmpty(swaggerDocumentModel.WelcomePageImage))
            {
                var imageFile = new FileInfo(swaggerDocumentModel.WelcomePageImage);
                if (imageFile.Exists)
                {
                    var image = welcomeSection.AddImage(imageFile.FullName);
                    image.Left = ShapePosition.Center;
                }
            }

            welcomeSection.AddParagraph(swaggerDocumentModel.Title).AsTitle().Centered();
            welcomeSection.AddParagraph(swaggerDocumentModel.Author).AsHeader().Centered();
            welcomeSection.AddParagraph(swaggerDocumentModel.Version).Centered();
            welcomeSection.AddParagraph(swaggerDocumentModel.DocumentDate.ToShortDateString()).Centered();
            welcomeSection.Footers.Primary.AddParagraph().AsSubHeader().PullRight().AddPageField();
        }

        private static void DrawAuthorizationInfoPage(Document pdfDocument, SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            var authorizationSection = pdfDocument.AddSection();
            authorizationSection.AddParagraph("Authorization information").AsHeader();
            authorizationSection.AddParagraph();
            foreach (var authorizationInfo in swaggerDocumentModel.AuthorizationInfos)
            {
                authorizationSection.AddParagraph($"Authorization option: {authorizationInfo.Key}").AsSubHeader().Bold();
                authorizationSection.AddParagraph();
                authorizationInfo.Value.WriteAuthorizationInfo(authorizationSection.AddParagraph());
                authorizationSection.AddParagraph();
            }
        }

        private static void DrawEndpointDocumentation(Document pdfDocument, SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            foreach (var docEntry in swaggerDocumentModel.DocumentationEntries)
            {
                var pathSection = pdfDocument.AddSection();

                DrawEndpointHeader(docEntry, pathSection);
                DrawPathParameters(docEntry, pathSection);
                DrawUrlParameters(docEntry, pathSection);
                DrawFormDataParameters(docEntry, pathSection);
                DrawBodyParameters(docEntry, pathSection);
                DrawResponses(docEntry, pathSection);
                pathSection.Footers.Primary.AddParagraph().AsSubHeader().PullRight().AddPageField();
            }
        }

        private static void DrawPathParameters(EndpointInfo docEntry, Section pathSection)
        {
            if (docEntry.PathParameters != null && docEntry.PathParameters.Any())
            {
                pathSection.AddParagraph();
                pathSection.AddParagraph("Path parameters").AsSubHeader();
                var table = pathSection.AddBorderedTable();
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));

                var row = table.AddRow();
                row[0].AddParagraph("Name");
                row[1].AddParagraph("Type");
                row[2].AddParagraph("Schema");
                row[3].AddParagraph("Description");

                foreach (var pathParameter in docEntry.PathParameters)
                {
                    row = table.AddRow();
                    row[0].VerticallyCenteredContent().AddParagraph(pathParameter.Name ?? "");
                    row[1].VerticallyCenteredContent().AddParagraph(pathParameter.Type ?? "");
                    var schema = SwaggerPdfJsonConvert.SerializeObject(pathParameter.Schema);
                    if (schema != "null")
                    {
                        row[2].VerticallyCenteredContent().AddParagraph(schema).AsFixedCharLength();
                    }

                    var description = row[3].VerticallyCenteredContent().AddParagraph(pathParameter.Description ?? "");
                    pathParameter.Schema?.WriteDetailedDescription(description);
                }
            }
        }

        private static void DrawResponses(EndpointInfo docEntry, Section pathSection)
        {
            if (docEntry.Responses != null)
            {
                pathSection.AddParagraph();
                pathSection.AddParagraph("Responses").AsSubHeader();

                foreach (var response in docEntry.Responses)
                {
                    pathSection.AddPageBreakableParagraph($"{response.Code}: {response.Description}").AddBorders();
                    var responseBody = SwaggerPdfJsonConvert.SerializeObject(response.Schema);
                    if (responseBody != "null")
                    {
                        pathSection.AddPageBreakableParagraph(responseBody).AddBorders().AsFixedCharLength();
                    }

                    pathSection.AddParagraph();
                }
            }
        }

        private static void DrawEndpointHeader(EndpointInfo docEntry, Section pathSection)
        {
            var headerParagraph = pathSection.AddParagraph($"{docEntry.HttpMethod} {docEntry.EndpointPath}").AsHeader();
            var summaryParagraph = pathSection.AddParagraph(docEntry.Summary);
            if (docEntry.Deprecated)
            {
                var deprecatedParagraph = pathSection.AddParagraph("Deprecated");
                deprecatedParagraph.Format.Font.Bold = true;
                deprecatedParagraph.Format.Font.Color = Colors.Red;
                headerParagraph.Format.Font.Color = Colors.Red;
                summaryParagraph.Format.Font.Color = Colors.Red;
            }
        }

        private static void DrawBodyParameters(EndpointInfo docEntry, Section pathSection)
        {
            if (docEntry.BodyParameters != null && docEntry.BodyParameters.Any())
            {
                pathSection.AddParagraph();
                pathSection.AddParagraph("Request body").AsSubHeader();

                foreach (var bodyParameter in docEntry.BodyParameters)
                {
                    pathSection.AddPageBreakableParagraph(SwaggerPdfJsonConvert.SerializeObject(bodyParameter.Schema)).AsFixedCharLength();
                }
            }
        }

        private static void DrawFormDataParameters(EndpointInfo docEntry, Section pathSection)
        {
            if (docEntry.FormDataParameters != null && docEntry.FormDataParameters.Any())
            {
                pathSection.AddParagraph();
                pathSection.AddParagraph("Form data parameters").AsSubHeader();
                var table = pathSection.AddBorderedTable();
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));

                var row = table.AddRow();
                row[0].AddParagraph("Parameter name");
                row[1].AddParagraph("Type");
                row[2].AddParagraph("Description");

                foreach (var parameter in docEntry.FormDataParameters)
                {
                    row = table.AddRow();
                    row[0].VerticallyCenteredContent().AddParagraph(parameter.Name ?? "");
                    row[1].VerticallyCenteredContent().AddParagraph(parameter.Type ?? "");
                    var description = row[2].VerticallyCenteredContent().AddParagraph(parameter.Description ?? "");
                    parameter.Schema?.WriteDetailedDescription(description);
                }
            }
        }

        private static void DrawUrlParameters(EndpointInfo docEntry, Section pathSection)
        {
            if (docEntry.UrlParameters != null && docEntry.UrlParameters.Any())
            {
                pathSection.AddParagraph();
                pathSection.AddParagraph("Query string parameters").AsSubHeader();
                var table = pathSection.AddBorderedTable();
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));

                var row = table.AddRow();
                row[0].AddParagraph("Name");
                row[1].AddParagraph("Type");
                row[2].AddParagraph("Schema");
                row[3].AddParagraph("Description");

                foreach (var queryParameter in docEntry.UrlParameters)
                {
                    row = table.AddRow();
                    row[0].VerticallyCenteredContent().AddParagraph(queryParameter.Name ?? "");
                    row[1].VerticallyCenteredContent().AddParagraph(queryParameter.Type);
                    var schema = SwaggerPdfJsonConvert.SerializeObject(queryParameter.Schema);
                    if (schema != "null")
                    {
                        row[2].VerticallyCenteredContent().AddParagraph(schema).AsFixedCharLength();
                    }

                    var description = row[3].VerticallyCenteredContent().AddParagraph(queryParameter.Description ?? "");
                    queryParameter.Schema?.WriteDetailedDescription(description);
                }
            }
        }
    }
}
