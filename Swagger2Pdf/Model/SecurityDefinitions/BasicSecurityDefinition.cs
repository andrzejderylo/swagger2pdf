using Swagger2Pdf.PdfGenerator.Model;
using Swagger2Pdf.PdfGenerator.Model.AuthorizationInformation;

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