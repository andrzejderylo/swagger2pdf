using System.Collections.Generic;

namespace Swagger2Pdf.PdfGenerator.Model
{
    public class SwaggerPdfDocumentModel
    {
        public List<EndpointInfo> DocumentationEntries { get; set; }
        public string PdfDocumentPath { get; set; }
    }
}
