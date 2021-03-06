﻿using System;
using System.Collections.Generic;

namespace Swagger2Pdf.PdfModel.Model
{
    public class SwaggerPdfDocumentModel
    {
        public List<EndpointInfo> DocumentationEntries { get; set; }
        public string PdfDocumentPath { get; set; }
        public string WelcomePageImage { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public DateTime DocumentDate { get; set; }
        public Dictionary<string, AuthorizationInfo> AuthorizationInfo { get; set; }
        public string CustomPageName { get; set; }
    }
}
