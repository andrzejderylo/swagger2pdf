using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class OperationResponse
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("schema")]
        public PropertyBase Schema { get; set; }
    }
}