using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class Parameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("in")]
        public string In { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("required")]
        public bool ParameterRequired { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("schema")]
        public PropertyBase Schema { get; set; }

        [JsonProperty("items")]
        public PropertyBase Items { get; set; }
    }
}