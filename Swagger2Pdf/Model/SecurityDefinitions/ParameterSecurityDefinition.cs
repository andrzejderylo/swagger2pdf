using Newtonsoft.Json;
using Swagger2Pdf.PdfModel.Model;
using Swagger2Pdf.PdfModel.Model.AuthorizationInformation;

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
