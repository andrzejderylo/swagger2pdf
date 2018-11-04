using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class Property : PropertyBase
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}