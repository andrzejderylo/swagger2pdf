using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Html2pdf;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Swagger2Pdf.PdfGenerator;
using Swagger2Pdf.PdfGenerator.Helpers;
using Swagger2Pdf.PdfGenerator.Model;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public class HtmlPdfBuilder : PdfBuilderBase
    {
        private readonly HtmlDocumentBuilder document = new HtmlDocumentBuilder();

        protected override void WriteCustomPage(StringWriter writer)
        {
            document.AddCustomPage(writer.GetStringBuilder());
        }

        protected override void DrawResponses(List<Response> docEntryResponses)
        {
            if(!docEntryResponses.Any()) return;

            document.P();
            document.H2().AddChildElement(new TextContent("Responses"));

            foreach (var response in docEntryResponses)
            {
                document.P()
                    .AddChildElement(new TextContent($"{response.Code}: {response.Description}"))
                    .SetStyle("border", "2px solid black");

                if (response.Schema != null)
                {
                    var responseBody = SwaggerPdfJsonConvert.SerializeObject(response.Schema);
                    document.P().SetStyle("border", "2px solid black")
                        .AddChildElement(new HtmlElement("pre").AddChildElement(new TextContent(responseBody)));
                }
            }
        }

        protected override void DrawBodyParameters(List<Parameter> docEntryBodyParameters)
        {
            if(!docEntryBodyParameters.Any()) return;

            document.P();
            document.H2().AddChildElement(new TextContent("Request body"));

            foreach (var bodyParameter in docEntryBodyParameters)
            {
                var bodyParameterJson = new HtmlElement("pre");
                bodyParameterJson
                    .AddChildElement(new TextContent(SwaggerPdfJsonConvert.SerializeObject(bodyParameter.Schema)));

                document.P().AddChildElement(bodyParameterJson).SetStyle("border", "1px solid black");
            }
        }

        protected override void DrawFormDataParameters(List<Parameter> docEntryFormDataParameters)
        {
            if (!docEntryFormDataParameters.Any()) return;

            document.P();
            document.H2().AddChildElement(new TextContent("Path parameters"));
            var table = document.Table();
            table.AddColumns(new TextContent("Name"), new TextContent("Type"), new TextContent("Description"));
            foreach (var parameter in docEntryFormDataParameters)
            {
                var nameCell = new TextContent(parameter.Name ?? "");
                var typeCell = new TextContent(parameter.Type ?? "");
                var descriptionCell = new HtmlElement("p");
                WriteDetailedDescription(descriptionCell, parameter);
                table.AddRow(nameCell, typeCell, descriptionCell);
            }
        }

        protected override void DrawQueryParameters(List<Parameter> docEntryQueryParameter)
        {
            if (!docEntryQueryParameter.Any()) return;

            document.P();
            document.H2().AddChildElement(new TextContent("Path parameters"));
            var table = document.Table();
            table.AddColumns(new TextContent("Name"), new TextContent("Type"), new TextContent("Schema"), new TextContent("Description"));
            foreach (var parameter in docEntryQueryParameter)
            {
                var nameCell = new TextContent(parameter.Name ?? "");
                var typeCell = new TextContent(parameter.Type ?? "");
                HtmlElement schemaCell = new TextContent("");
                if (parameter.Schema != null)
                {
                    var schema = SwaggerPdfJsonConvert.SerializeObject(parameter.Schema);
                    schemaCell = new HtmlElement("pre").AddChildElement(new TextContent(schema));
                }
                var descriptionCell = new HtmlElement("p");
                WriteDetailedDescription(descriptionCell, parameter);
                table.AddRow(nameCell, typeCell, schemaCell, descriptionCell);
            }
        }

        protected override void DrawPathParameters(List<Parameter> docEntryPathParameters)
        {
            if (!docEntryPathParameters.Any()) return;

            document.P();
            document.H2().AddChildElement(new TextContent("Path parameters"));
            var table = document.Table();
                table.AddColumns(new TextContent("Name"), new TextContent("Type"), new TextContent("Schema"), new TextContent("Description"));
            foreach (var parameter in docEntryPathParameters)
            {
                var nameCell = new TextContent(parameter.Name ?? "");
                var typeCell = new TextContent(parameter.Type ?? "");
                HtmlElement schemaCell = new TextContent("");
                if (parameter.Schema != null)
                {
                    var schema = SwaggerPdfJsonConvert.SerializeObject(parameter.Schema);
                    schemaCell = new HtmlElement("pre").AddChildElement(new TextContent(schema));
                }
                var descriptionCell = new HtmlElement("p");
                WriteDetailedDescription(descriptionCell, parameter);
                table.AddRow(nameCell, typeCell, schemaCell, descriptionCell);
            }
        }

        protected override void DrawEndpointHeader(EndpointInfo docEntry)
        {
            if (docEntry.Deprecated)
            {
                document.H1()
                    .AddChildElement(new HtmlElement("strong").AddChildElement(new TextContent($"{docEntry.HttpMethod} {docEntry.EndpointPath}")))
                    .SetStyle("text-decoration", "line-through");
                document.P()
                    .AddChildElement(new HtmlElement("strong").AddChildElement(new TextContent(docEntry.Summary)))
                    .SetStyle("text-decoration", "line-through");
                document.P().AddChildElement(new HtmlElement("strong").AddChildElement(new TextContent("Deprecated")));
            }
            else
            {
                document.H1()
                    .AddChildElement(new HtmlElement("strong").AddChildElement(new TextContent($"{docEntry.HttpMethod} {docEntry.EndpointPath}")));
                document.P()
                    .AddChildElement(new HtmlElement("strong").AddChildElement(new TextContent(docEntry.Summary)));
            }
        }

        protected override void DrawWelcomePage(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            if (!string.IsNullOrEmpty(swaggerDocumentModel.WelcomePageImage))
            {
                var imageFile = new FileInfo(swaggerDocumentModel.WelcomePageImage);
                if (imageFile.Exists)
                {
                    document.Img().SetAttribute("src", imageFile.FullName).SetStyle("margin-top", "150px");
                }
            }
            else
            {
                document.Div().SetStyle("width", "100px").SetStyle("height", "300").SetStyle("display", "block");
            }


            document.H1().AddChildElement(new TextContent(swaggerDocumentModel.Title)).SetStyle("text-align", "center");
            document.H2().AddChildElement(new TextContent(swaggerDocumentModel.Author)).SetStyle("text-align", "center");
            document.H3().AddChildElement(new TextContent(swaggerDocumentModel.Version)).SetStyle("text-align", "center");
            document.H3().AddChildElement(new TextContent(swaggerDocumentModel.DocumentDate.ToShortDateString())).SetStyle("text-align", "center");
        }

        protected override void BeginNewPage()
        {
            document.AddPageBreak();
        }

        protected override void SaveDocument(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            var documentString = document.GetDocumentString();
            var pdfDocument = new PdfDocument(new PdfWriter(swaggerDocumentModel.PdfDocumentPath));
            HtmlConverter.ConvertToDocument(documentString, pdfDocument, new ConverterProperties());
            pdfDocument.Close();
        }

        protected override void DrawAuthorizationInfoPage(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
        }

        private static void WriteDetailedDescription(HtmlElement element, Parameter p)
        {
            if (p.Deprecated)
            {
                element.AddChildElement(new TextContent("Deprecated").SetStyle("font-color", "red"));
                p.Description.IfNotNull(x => element.AddChildElement(new TextContent(x)).SetStyle("font-color", "red").SetStyle("text-decoration", "line-through"));
            }
            else
            {
                p.Description.IfNotNull(x => element.AddChildElement(new TextContent(x)));
            }

            if (p.IsRequired)
            {
                element.AddChildElement(new HtmlElement("strong").AddChildElement(new TextContent("Required")));
            }

            p.Pattern.IfNotNull(x => element.AddChildElement(new TextContent($"Pattern: {x}")));
            p.GetMinMaxString().IfNotNull(x => element.AddChildElement(new TextContent(x)));
            p.GetExclusiveMinMaxString().IfNotNull(x => element.AddChildElement(new TextContent(x)));
            p.GetMinMaxItems().IfNotNull(x => element.AddChildElement(new TextContent(x)));
            p.GetMinMaxProperties().IfNotNull(x => element.AddChildElement(new TextContent(x)));
        }
    }
}
