using Swagger2Pdf.PdfModel.Model;
using Swagger2Pdf.PdfModel.Model.AuthorizationInformation;

namespace Swagger2Pdf.Model.SecurityDefinitions
{
    public class BasicSecurityDefinition : SecurityDefinition
    {
        public override AuthorizationInfo CreateAuthorizationInfo()
        {
            return new HttpAuthorizationInfo
            {
                Type = "http",
                Scheme = "Basic"
            };
        }
    }
}