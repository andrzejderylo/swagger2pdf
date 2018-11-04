using Swagger2Pdf.PdfGenerator.Schemas;

namespace Swagger2Pdf.Model
{
    public abstract class PropertyBase
    {
        public abstract Schema CreateSchema();
    }
}