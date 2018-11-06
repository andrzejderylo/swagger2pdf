using System.Data;
using MigraDoc.DocumentObjectModel;

namespace Swagger2Pdf.PdfGenerator.Model
{
    public abstract class AuthorizationInfo
    {   
        public string Type { get; set; }

        public abstract void WriteAuthorizationInfo(Paragraph paragraph);
    }
}
