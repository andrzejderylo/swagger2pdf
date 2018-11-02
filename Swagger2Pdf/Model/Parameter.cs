using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class Parameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("in")]
        public string In { get; set; }

        [JsonProperty("required")]
        public bool ParameterRequired { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        public string Format { get; set; }

        [JsonProperty("schema")]
        public ComplexTypeSchema Schema { get; set; }
    }

    public class ComplexTypeSchema
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("items")]
        public object Items { get; set; }
    }

}