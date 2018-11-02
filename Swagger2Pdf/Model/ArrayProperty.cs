using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class ArrayProperty
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public object Items { get; set; }
    }
}