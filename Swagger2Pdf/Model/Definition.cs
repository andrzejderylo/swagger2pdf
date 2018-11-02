using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Swagger2Pdf.Model.Converters;

namespace Swagger2Pdf.Model
{
    public class Definition
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]//, ItemConverterType = typeof(DefinitionPropertyJsonConverter))]
        public Dictionary<string, object> Properties { get; set; }

        //[JsonIgnore]
        //public Dictionary<string, Property> SimpleTypeProperties =>
        //    Properties?.Where(x => x.Value.GetType() == typeof(Property))
        //        .ToDictionary(x => x.Key, x => x.Value as Property);
        //
        //[JsonIgnore]
        //public Dictionary<string, DefinitionReference> DefinedTypeProperties => Properties?.Where(x => x.Value.GetType() == typeof(DefinitionReference))
        //    .ToDictionary(x => x.Key, x => x.Value as DefinitionReference);
        //
        //[JsonIgnore]
        //public Dictionary<string, ArrayProperty> ArrayProperties => Properties?.Where(x => x.Value.GetType() == typeof(ArrayProperty))
        //    .ToDictionary(x => x.Key, x => x.Value as ArrayProperty);
    }
}