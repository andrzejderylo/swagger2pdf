using Newtonsoft.Json;
using Swagger2Pdf.PdfGenerator.Model;

namespace Swagger2Pdf.Model
{
    public abstract class SecurityDefinition
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        public abstract AuthorizationInfo CreateAuthorizationInfo();
    }
}
