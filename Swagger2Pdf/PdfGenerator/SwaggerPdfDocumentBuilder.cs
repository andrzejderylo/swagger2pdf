using System.Linq;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Swagger2Pdf.PdfGenerator.Model;

namespace Swagger2Pdf.PdfGenerator
{
    public class SwaggerPdfDocumentBuilder
    {
        public void BuildPdf(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            Document document = new Document();
            DrawInfo(document, swaggerDocumentModel);
            DrawPaths(document, swaggerDocumentModel);
            PdfDocumentRenderer renderer = new PdfDocumentRenderer();
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(swaggerDocumentModel.PdfDocumentPath);
        }

        private void DrawInfo(Document pdfDocument, SwaggerPdfDocumentModel swaggerDocumentModel)
        {

        }

        private void DrawPaths(Document pdfDocument, SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            foreach (var docEntry in swaggerDocumentModel.DocumentationEntries)
            {
                var pathSection = pdfDocument.AddSection();

                DrawHttpEndpointTable(docEntry, pathSection);
                pathSection.AddParagraph();
                DrawUrlParameters(docEntry, pathSection);
                pathSection.AddParagraph();
                DrawBodyParameters(docEntry, pathSection);
                pathSection.AddParagraph();
                DrawResponses(docEntry, pathSection);
            }
        }

        private void DrawResponses(EndpointInfo docEntry, Section pathSection)
        {
            if (docEntry.Responses != null)
            {
                pathSection.AddParagraph("Response body");
                var table = pathSection.AddTable();
                table.Borders = new Borders();
                table.Borders.Color = BlackColor;
                table.AddColumn(Unit.FromCentimeter(18));

                foreach (var response in docEntry.Responses)
                {
                    var row = table.AddRow();
                    row[0].AddParagraph(SwaggerPdfJsonConvert.SerializeObject(response.Schema));
                }
            }

        }

        private void DrawHttpEndpointTable(EndpointInfo docEntry, Section pathSection)
        {
            var table = pathSection.AddTable();
            table.Borders = new Borders();
            table.Borders.Color = BlackColor;
            table.AddColumn(Unit.FromCentimeter(3));
            table.AddColumn(Unit.FromCentimeter(10));
            var row = table.AddRow();
            row[0].AddParagraph("Http method");
            row[1].AddParagraph("Endpoint path");

            row = table.AddRow();
            row[0].AddParagraph(docEntry.HttpMethod);
            row[1].AddParagraph(docEntry.EndpointPath);
        }

        private void DrawBodyParameters(EndpointInfo docEntry, Section pathSection)
        {
            if (docEntry.BodyParameters != null && docEntry.BodyParameters.Any())
            {
                pathSection.AddParagraph("Body parameters");
                var table = pathSection.AddTable();
                table.Borders = new Borders();
                table.Borders.Color = BlackColor;
                table.AddColumn(Unit.FromCentimeter(18));

                foreach (var bodyParameter in docEntry.BodyParameters)
                {
                    var row = table.AddRow();
                    row[0].AddParagraph(SwaggerPdfJsonConvert.SerializeObject(bodyParameter.Schema));
                }
            }
        }

        private void DrawUrlParameters(EndpointInfo docEntry, Section pathSection)
        {
            pathSection.AddParagraph("Query parameters");
            if (docEntry.UrlParameters == null)
            {
                pathSection.AddParagraph("No query parameters.");
            }
            else
            {
                var table = pathSection.AddTable();
                table.Borders = new Borders();
                table.Borders.Color = BlackColor;
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));

                var row = table.AddRow();
                row[0].AddParagraph("Name");
                row[1].AddParagraph("Type");
                row[2].AddParagraph("Description");

                foreach (var queryParameter in docEntry.UrlParameters)
                {
                    row = table.AddRow();
                    row[0].AddParagraph(queryParameter.Name ?? "");
                    row[1].AddParagraph(SwaggerPdfJsonConvert.SerializeObject(queryParameter.Schema) ?? "");
                    row[1].AddParagraph(queryParameter.Description ?? "");
                }
            }
        }

        private static readonly Color BlackColor = Color.FromCmyk(100, 100, 100, 100);
    }
}
