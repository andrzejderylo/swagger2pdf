using MigraDoc.DocumentObjectModel;

namespace Swagger2Pdf.PdfGenerator.Schemas
{
    public abstract class Schema
    {
        public virtual void WriteDetailedDescription(Paragraph paragraph)
        {
        }
    }
}
