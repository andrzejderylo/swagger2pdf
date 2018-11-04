using System.Collections.Generic;
using Newtonsoft.Json;
using Swagger2Pdf.Model.Converters;

namespace Swagger2Pdf.Model
{
    public class Definition
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties", ItemConverterType = typeof(PropertyBaseConverter))]
        public Dictionary<string, PropertyBase> Properties { get; set; }
    }

}