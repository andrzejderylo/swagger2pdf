using Swagger2Pdf.PdfModel.Model.AuthorizationInformation;

namespace Swagger2Pdf.PdfModel.Model
{
    public abstract class AuthorizationInfo
    {   
        public string Type { get; set; }

        public abstract void WriteAuthorizationInfo(IAuthorizationWriter writer);
    }
}
