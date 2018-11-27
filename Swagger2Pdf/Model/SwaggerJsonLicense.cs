using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class SwaggerJsonLicense
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
    }

}