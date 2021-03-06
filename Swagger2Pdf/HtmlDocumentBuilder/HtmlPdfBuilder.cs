﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using log4net;
using Swagger2Pdf.PdfModel;
using Swagger2Pdf.PdfModel.Model;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public class HtmlPdfBuilder : PdfBuilderBase
    {
        private readonly HtmlDocumentBuilder _document = new HtmlDocumentBuilder();
        
        public HtmlPdfBuilder() : base(LogManager.GetLogger(typeof(Program)))
        {
        }

        protected override void WriteCustomPage(StringWriter writer)
        {
            _document.AddCustomPage(writer.GetStringBuilder());
        }

        protected override void DrawResponses(List<Response> docEntryResponses)
        {
            if(!docEntryResponses.Any()) return;

            _document.P();
            _document.H2().SetText("Responses");

            foreach (var response in docEntryResponses)
            {
                var p = _document.P().SetText($"{response.Code}: {response.Description}")
                    .SetStyle("border", "1px solid #c6cbd1");

                if (response.Schema != null)
                {
                    var responseBody = PdfModelJsonConverter.SerializeObject(response.Schema);
                    p.AddChildElement(HtmlElement.Pre().SetText(responseBody));
                }
            }
        }

        protected override void DrawBodyParameters(List<Parameter> docEntryBodyParameters)
        {
            if(!docEntryBodyParameters.Any()) return;

            _document.P();
            _document.H2().SetText("Request body parameters");

            foreach (var bodyParameter in docEntryBodyParameters)
            {
                var schema = PdfModelJsonConverter.SerializeObject(bodyParameter.Schema);
                _document.Pre().SetText(schema);
            }
        }

        protected override void DrawFormDataParameters(List<Parameter> docEntryFormDataParameters)
        {
            if (!docEntryFormDataParameters.Any()) return;

            _document.P();
            _document.H2().SetText("Form data parameters");
            var table = _document.Table();
            table.AddColumns(new TextContent("Name"), new TextContent("Type"), new TextContent("Description"));
            foreach (var parameter in docEntryFormDataParameters)
            {
                var nameCell = new TextContent(parameter.Name ?? "");
                var typeCell = new TextContent(parameter.Type ?? "");
                var descriptionCell = HtmlElement.P();
                WriteDetailedDescription(descriptionCell, parameter);
                table.AddRow(nameCell, typeCell, descriptionCell);
            }
        }

        protected override void DrawQueryParameters(List<Parameter> docEntryQueryParameter)
        {
            if (!docEntryQueryParameter.Any()) return;

            _document.P();
            _document.H2().SetText("Query string parameters");
            var table = _document.Table();
            table.AddColumns(new TextContent("Name"), new TextContent("Type"), new TextContent("Schema"), new TextContent("Description"));
            foreach (var parameter in docEntryQueryParameter)
            {
                var nameCell = new TextContent(parameter.Name ?? "");
                var typeCell = new TextContent(parameter.Type ?? "");
                HtmlElement schemaCell = new TextContent("");
                if (parameter.Schema != null)
                {
                    var schema = PdfModelJsonConverter.SerializeObject(parameter.Schema);
                    schemaCell = HtmlElement.Pre().SetText(schema);
                }

                var descriptionCell = HtmlElement.P();
                WriteDetailedDescription(descriptionCell, parameter);
                table.AddRow(nameCell, typeCell, schemaCell, descriptionCell);
            }
        }

        protected override void DrawPathParameters(List<Parameter> docEntryPathParameters)
        {
            if (!docEntryPathParameters.Any()) return;

            _document.P();
            _document.H2().SetText("Path parameters");
            var table = _document.Table();
            table.AddColumns(new TextContent("Name"), new TextContent("Type"), new TextContent("Schema"), new TextContent("Description"));
            foreach (var parameter in docEntryPathParameters)
            {
                var nameCell = new TextContent(parameter.Name ?? "");
                var typeCell = new TextContent(parameter.Type ?? "");
                HtmlElement schemaCell = new TextContent("");
                if (parameter.Schema != null)
                {
                    var schema = PdfModelJsonConverter.SerializeObject(parameter.Schema);
                    schemaCell = HtmlElement.Pre().SetText(schema);
                }
                var descriptionCell = HtmlElement.P();
                WriteDetailedDescription(descriptionCell, parameter);
                table.AddRow(nameCell, typeCell, schemaCell, descriptionCell);
            }
        }

        protected override void DrawEndpointHeader(EndpointInfo docEntry)
        {
            if (docEntry.Deprecated)
            {
                _document.H1().Bold().Deleted().SetText($"{docEntry.HttpMethod} {docEntry.EndpointPath}");
                _document.P().Bold().Deleted().SetText(docEntry.Summary);
                _document.P().Bold().SetText("Deprecated");
            }
            else
            {
                _document.H1().Bold().SetText($"{docEntry.HttpMethod} {docEntry.EndpointPath}");
                _document.P().Bold().SetText(docEntry.Summary);
            }
        }

        protected override void DrawWelcomePage(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            if (!string.IsNullOrEmpty(swaggerDocumentModel.WelcomePageImage))
            {
                var imageFile = new FileInfo(swaggerDocumentModel.WelcomePageImage);
                if (imageFile.Exists)
                {
                    var companyLogo = HtmlElement.Img().Src(imageFile.Name).SetStyle("margin-top", "150px");
                    _document.Div().AddChildElement(companyLogo).SetStyle("text-align", "center");
                }
            }
            else
            {
                _document.Div().SetStyle("width", "100px").SetStyle("height", "300").SetStyle("display", "block");
            }


            swaggerDocumentModel.Title.IfNotNull(x => _document.H1().HorizontalCenter().SetText(x));
            swaggerDocumentModel.Author.IfNotNull(x => _document.H2().HorizontalCenter().SetText($"Author: {x}"));
            swaggerDocumentModel.Version.IfNotNull(x => _document.H3().HorizontalCenter().SetText($"Version: {x}"));
            swaggerDocumentModel.DocumentDate.ToShortDateString().IfNotNull(x => _document.H3().HorizontalCenter().SetText(x));
        }

        protected override void BeginNewPage()
        {
            _document.AddPageBreak();
        }

        protected override void SaveDocument(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            var documentString = _document.GetDocumentString();
            
            var pdfDocument = new PdfDocument(new PdfWriter(swaggerDocumentModel.PdfDocumentPath));
            var properties = new ConverterProperties();
            if (!string.IsNullOrEmpty(swaggerDocumentModel.WelcomePageImage))
            {
                var imageFile = new FileInfo(swaggerDocumentModel.WelcomePageImage);
                properties.SetBaseUri(imageFile.DirectoryName + "\\");
            }
            HtmlConverter.ConvertToDocument(documentString, pdfDocument, properties);
            pdfDocument.Close();
        }

        protected override void DrawAuthorizationInfoPage(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            _document.H1().SetText("Authorization information");
            _document.P();
            foreach (var authorizationInfo in swaggerDocumentModel.AuthorizationInfo)
            {
                _document.H2().Bold().SetText($"Authorization option: {authorizationInfo.Key}");
                _document.P();

                var infoParagraph = _document.Div();
                authorizationInfo.Value.WriteAuthorizationInfo(new HtmlAuthorizationWriter(infoParagraph));
                _document.P();
            }
        }

        private static void WriteDetailedDescription(HtmlElement element, Parameter p)
        {
            if (p.Deprecated)
            {
                element.AddChildElement(HtmlElement.Label().SetText("Deprecated").SetColor("red")).AddChildElement(HtmlElement.Br());
                p.Description.IfNotNull(x => element.AddChildElement(HtmlElement.Label().SetText(x).Deleted().SetColor("red")).AddChildElement(HtmlElement.Br()));
            }
            else
            {
                p.Description.IfNotNull(x => element.AddChildElement(HtmlElement.Label().SetText(x)).AddChildElement(HtmlElement.Br()));
            }

            if (p.IsRequired)
            {
                element.AddChildElement(HtmlElement.Label().Bold().SetText("Required")).AddChildElement(HtmlElement.Br());
            }

            p.Schema?.WriteDetailedDescription(new HtmlAuthorizationWriter(element));
            element.AddChildElement(HtmlElement.Br());

            p.Pattern.IfNotNull(x => element.AddChildElement(HtmlElement.Label().SetText($"Pattern: {x}")).AddChildElement(HtmlElement.Br()));
            p.GetMinMaxString().IfNotNull(x => element.AddChildElement(HtmlElement.Label().SetText(x)).AddChildElement(HtmlElement.Br()));
            p.GetExclusiveMinMaxString().IfNotNull(x => element.AddChildElement(HtmlElement.Label().SetText(x)).AddChildElement(HtmlElement.Br()));
            p.GetMinMaxItems().IfNotNull(x => element.AddChildElement(HtmlElement.Label().SetText(x)).AddChildElement(HtmlElement.Br()));
            p.GetMinMaxProperties().IfNotNull(x => element.AddChildElement(HtmlElement.Label().SetText(x)).AddChildElement(HtmlElement.Br()));
            
        }
    }
}
