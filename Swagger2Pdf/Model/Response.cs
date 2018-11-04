using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class Response
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("schema")]
        public PropertyBase Schema { get; set; }
    }
}