using Newtonsoft.Json;
using Swagger2Pdf.PdfGenerator.Model;
using Swagger2Pdf.PdfGenerator.Model.AuthorizationInformation;

namespace Swagger2Pdf.Model.SecurityDefinitions
{
    public class ParameterSecurityDefinition : SecurityDefinition
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("in")]
        public string In { get; set; }

        public override AuthorizationInfo CreateAuthorizationInfo()
        {
            return new ParameterAuthorizationInfo
            {
                Type = Type,
                In = In,
                Name = Name
            };
        }
    }
}
