using System.Collections.Generic;
using Swagger2Pdf.PdfGenerator.Schemas;

namespace Swagger2Pdf.PdfGenerator.Model
{
    public class Response
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public Schema Schema { get; set; }
    }
}