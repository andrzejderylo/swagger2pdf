using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class ExternalDoc
    {
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
    }

}