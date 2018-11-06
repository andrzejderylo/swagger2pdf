using System.Collections.Generic;

namespace Swagger2Pdf.PdfGenerator.Model
{
    public class Response
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public Schema Schema { get; set; }
    }
}