using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Swagger2Pdf.Model
{
    public class Definition
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, PropertyBase> Properties { get; set; }
    }

    public class DefinitionReferenceProperty 
    {
        [JsonProperty("$ref")]
        public string Ref { get; set; }

        [JsonProperty("example")]
        public object Example { get; set; }

        [JsonProperty("readOnly")]
        public bool? ReadOnly { get; set; }

        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("enum")]
        public IList<object> Enum { get; set; }

        [JsonProperty("required")]
        public IList<string> Required { get; set; }

        [JsonProperty("minProperties")]
        public int? MinProperties { get; set; }

        [JsonProperty("maxProperties")]
        public int? MaxProperties { get; set; }

        [JsonProperty("uniqueItems")]
        public bool? UniqueItems { get; set; }

        [JsonProperty("minItems")]
        public int? MinItems { get; set; }

        [JsonProperty("maxItems")]
        public int? MaxItems { get; set; }

        [JsonProperty("pattern")]
        public string Pattern { get; set; }

        [JsonProperty("minLength")]
        public int? MinLength { get; set; }

        [JsonProperty("maxLength")]
        public int? MaxLength { get; set; }

        [JsonProperty("exclusiveMinimum")]
        public bool? ExclusiveMinimum { get; set; }

        [JsonProperty("minimum")]
        public int? Minimum { get; set; }

        [JsonProperty("exclusiveMaximum")]
        public bool? ExclusiveMaximum { get; set; }

        [JsonProperty("maximum")]
        public int? Maximum { get; set; }

        [JsonProperty("multipleOf")]
        public int? MultipleOf { get; set; }

        [JsonProperty("default")]
        public object Default { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }
    }

}