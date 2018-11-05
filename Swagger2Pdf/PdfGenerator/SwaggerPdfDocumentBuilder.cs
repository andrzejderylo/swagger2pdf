using System.IO;
using System.Linq;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using Swagger2Pdf.PdfGenerator.Model;

namespace Swagger2Pdf.PdfGenerator
{
    public class SwaggerPdfDocumentBuilder
    {
        public void BuildPdf(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            Document document = new Document();
            document.DefineStyles();

            DrawWelcomePage(document, swaggerDocumentModel);
            DrawAuthorizationInfoPage(document, swaggerDocumentModel);
            DrawEndpointDocumentation(document, swaggerDocumentModel);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer();
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(swaggerDocumentModel.PdfDocumentPath);
        }

        private void DrawWelcomePage(Document pdfDocument, SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            var welcomeSection = pdfDocument.AddSection();
            var imageFile = new FileInfo(swaggerDocumentModel.WelcomePageImage);
            if (imageFile.Exists)
            {   
                var image = welcomeSection.AddImage(imageFile.FullName);
                image.Left = ShapePosition.Center;
            }

            welcomeSection.AddParagraph(swaggerDocumentModel.Title).AsTitle().Centered();
            welcomeSection.AddParagraph(swaggerDocumentModel.Author).AsHeader().Centered();
            welcomeSection.AddParagraph(swaggerDocumentModel.Version).Centered();
            welcomeSection.AddParagraph(swaggerDocumentModel.DocumentDate.ToShortDateString()).Centered();
        }

        private void DrawAuthorizationInfoPage(Document pdfDocument, SwaggerPdfDocumentModel swaggerDocumentModel)
        {

        }

        private void DrawEndpointDocumentation(Document pdfDocument, SwaggerPdfDocumentModel swaggerDocumentModel)
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
            }
        }

        private void DrawPathParameters(EndpointInfo docEntry, Section pathSection)
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

                foreach (var queryParameter in docEntry.PathParameters)
                {
                    row = table.AddRow();
                    row[0].AddParagraph(queryParameter.Name ?? "");
                    row[1].AddParagraph(queryParameter.Type ?? "");
                    var schema = SwaggerPdfJsonConvert.SerializeObject(queryParameter.Schema);
                    if (schema != "null")
                    {
                        row[2].AddParagraph(schema).AsFixedCharLength();
                    }

                    row[3].AddParagraph(queryParameter.Description ?? "");
                }
            }
        }

        private void DrawResponses(EndpointInfo docEntry, Section pathSection)
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

        private void DrawEndpointHeader(EndpointInfo docEntry, Section pathSection)
        {
            var headerParagraph = pathSection.AddParagraph($"{docEntry.HttpMethod.ToUpper()} {docEntry.EndpointPath}").AsHeader();
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

        private void DrawBodyParameters(EndpointInfo docEntry, Section pathSection)
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

        private void DrawFormDataParameters(EndpointInfo docEntry, Section pathSection)
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
                    row[0].AddParagraph(parameter.Name ?? "");
                    row[1].AddParagraph(parameter.Type ?? "");
                    row[2].AddParagraph(parameter.Description ?? "");
                }
            }
        }

        private void DrawUrlParameters(EndpointInfo docEntry, Section pathSection)
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
                    row[0].AddParagraph(queryParameter.Name ?? "");
                    row[1].AddParagraph(queryParameter.Type);
                    var schema = SwaggerPdfJsonConvert.SerializeObject(queryParameter.Schema);
                    if (schema != "null")
                    {
                        row[2].AddParagraph(schema).AsFixedCharLength();
                    }

                    row[3].AddParagraph(queryParameter.Description ?? "");
                }
            }
        }
    }
}
