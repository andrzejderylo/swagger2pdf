using iText.Layout.Element;

namespace Swagger2Pdf.PdfModel.Model
{
    public abstract class Schema
    {
        public virtual void WriteDetailedDescription(IAuthorizationWriter writer)
        {
        }
    }
}
