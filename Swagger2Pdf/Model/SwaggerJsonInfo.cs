using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class SwaggerJsonInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("termsOfService")]
        public string TermsOfService { get; set; }

        [JsonProperty("contact")]
        public SwaggerJsonContact Contact { get; set; }

        [JsonProperty("license")]
        public SwaggerJsonLicense License { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}