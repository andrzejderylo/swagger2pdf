using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class OperationParameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("in")]
        public string In { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("deprecated")]
        public bool Deprecated { get; set; }

        [JsonProperty("allowEmptyValue")]
        public bool AllowEmptyValue { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("multipleOf")]
        public string MultipleOf { get; set; }
        
        [JsonProperty("maximum")]
        public string Maximum { get; set; }
        
        [JsonProperty("exclusiveMaximum")]
        public string ExclusiveMaximum { get; set; }
        
        [JsonProperty("minimum")]
        public string Minimum { get; set; }
        
        [JsonProperty("exclusiveMinimum")]
        public string ExclusiveMinimum { get; set; }

        [JsonProperty("maxLength")]
        public string MaxLength { get; set; }

        [JsonProperty("minLength")]
        public string MinLength { get; set; }
        
        [JsonProperty("pattern")]
        public string Pattern { get; set; }
        
        [JsonProperty("maxItems")]
        public string MaxItems { get; set; }
        
        [JsonProperty("minItems")]
        public string MinItems { get; set; }

        [JsonProperty("uniqueItems")]
        public bool UniqueItems { get; set; }
        
        [JsonProperty("maxProperties")]
        public string MaxProperties { get; set; }
        
        [JsonProperty("minProperties")]
        public string MinProperties { get; set; }
        
        [JsonProperty("required")]
        public bool IsRequired { get; set; }
        
        [JsonProperty("enum")]
        public string Enum { get; set; }

        [JsonProperty("schema")]
        public PropertyBase Schema { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}