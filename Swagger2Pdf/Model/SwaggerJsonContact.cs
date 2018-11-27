using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class SwaggerJsonContact
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
    }

}