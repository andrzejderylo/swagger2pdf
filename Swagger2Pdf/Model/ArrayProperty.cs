using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class ArrayProperty : PropertyBase
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        public PropertyBase Items { get; set; }
    }
}