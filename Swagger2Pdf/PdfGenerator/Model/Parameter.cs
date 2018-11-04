using Swagger2Pdf.PdfGenerator.Schemas;

namespace Swagger2Pdf.PdfGenerator.Model
{
    public class Parameter
    {
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public string Description { get; set; }
        public Schema Schema { get; set; }
    }
}