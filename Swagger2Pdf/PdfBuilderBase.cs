using System;
using System.Collections.Generic;
using System.IO;
using Common.Logging;
using Swagger2Pdf.PdfGenerator.Model;

namespace Swagger2Pdf
{
    public abstract class PdfBuilderBase
    {
        public static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        public void BuildPdf(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            Logger.Info("Building pdf document");

            Logger.Info("Drawing welcome page");
            DrawWelcomePage(swaggerDocumentModel);
            BeginNewPage();
            Logger.Info("Drawing welcome page done.");

            Logger.Info("Drawing custom page");
            DrawCustomPage(swaggerDocumentModel);
            BeginNewPage();
            Logger.Info("Drawing custom page done");

            Logger.Info("Drawing authorization info page");
            DrawAuthorizationInfoPage(swaggerDocumentModel);
            BeginNewPage();
            Logger.Info("Drawing authorization info page done.");

            Logger.Info("Drawing drawing endpoint documentation");
            DrawEndpointDocumentation(swaggerDocumentModel);
            Logger.Info("Drawing drawing endpoint documentation done.");

            Logger.Info("Rendering PDF document");
            var fi = new FileInfo(swaggerDocumentModel.PdfDocumentPath);
            Logger.Info($"Saving PDF document to: {fi.FullName}");
            SaveDocument(swaggerDocumentModel);
            Logger.Info("Done");
        }

        private void DrawCustomPage(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            var customPagePath = swaggerDocumentModel.CustomPageName;
            if (string.IsNullOrEmpty(customPagePath))
            {
                Logger.Info("No custom page");
                return;
            }
            FileInfo customPageFileInfo = new FileInfo(customPagePath);
            if (!customPageFileInfo.Exists) throw new ArgumentException($"File {customPagePath} does not exist");
            if (customPageFileInfo.Extension != ".md") throw new ArgumentException($"Only markdown (.md) files are supported. Current: {customPageFileInfo.Extension}");
            Logger.Info("Writing custom page");
            using (var reader = new StreamReader(customPageFileInfo.FullName))
            using (var writer = new StringWriter())
            {
                CommonMark.CommonMarkConverter.Convert(reader, writer);
                WriteCustomPage(writer);
            }
        }

        private void DrawEndpointDocumentation(SwaggerPdfDocumentModel swaggerDocumentModel)
        {
            foreach (var docEntry in swaggerDocumentModel.DocumentationEntries)
            {   
                DrawEndpointHeader(docEntry);
                DrawPathParameters(docEntry.PathParameters ?? new List<Parameter>());
                DrawQueryParameters(docEntry.QueryParameter ?? new List<Parameter>());
                DrawFormDataParameters(docEntry.FormDataParameters ?? new List<Parameter>());
                DrawBodyParameters(docEntry.BodyParameters ?? new List<Parameter>());
                DrawResponses(docEntry.Responses);
                BeginNewPage();
            }
        }

        protected abstract void WriteCustomPage(StringWriter writer);

        protected abstract void DrawResponses(List<Response> docEntryResponses);

        protected abstract void DrawBodyParameters(List<Parameter> docEntryBodyParameters);

        protected abstract void DrawFormDataParameters(List<Parameter> docEntryFormDataParameters);

        protected abstract void DrawQueryParameters(List<Parameter> docEntryQueryParameter);

        protected abstract void DrawPathParameters(List<Parameter> docEntryPathParameters);

        protected abstract void DrawEndpointHeader(EndpointInfo docEntry);

        protected abstract void DrawWelcomePage(SwaggerPdfDocumentModel swaggerDocumentModel);

        protected abstract void BeginNewPage();

        protected abstract void SaveDocument(SwaggerPdfDocumentModel swaggerDocumentModel);

        protected abstract void DrawAuthorizationInfoPage(SwaggerPdfDocumentModel swaggerDocumentModel);
    }
}
