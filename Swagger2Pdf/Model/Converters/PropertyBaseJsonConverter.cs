using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swagger2Pdf.Model.Properties;

namespace Swagger2Pdf.Model.Converters
{
    public class PropertyBaseJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PropertyBase);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JToken.ReadFrom(reader);
            var @ref = jObject["$ref"]?.ToString();
            var @enum = jObject["enum"]?.ToString();
            var type = jObject["type"]?.ToString();

            if (!string.IsNullOrEmpty(@ref))
            {   
                return ResolveReferenceProperty(jObject);
            }
            
            if (!string.IsNullOrEmpty(@enum))
            {
                return CreateEnumProperty(jObject);
            }

            if (!string.IsNullOrEmpty(type) && type == "array")
            {   
                return new ArrayProperty
                {
                    Description = jObject["description"]?.ToString(),
                    Type = "array",
                    Items = CreateItemsProperty(jObject["items"])
                };
            }

            return CreateSimpleProperty(jObject);
        }

        private EnumSimpleTypeProperty CreateEnumProperty(JToken jObject)
        {
            return new EnumSimpleTypeProperty
            {
                Type = jObject["type"]?.ToString(),
                Format = jObject["format"]?.ToString(),
                ExampleValue = jObject["exampleValue"]?.ToObject<object>(),
                CollectionFormat = jObject["collectionFormat"]?.ToString(),
                Default = jObject["default"]?.ToObject<object>(),
                Description = jObject["description"]?.ToString(),
                EnumValues = jObject["enumValues"]?.ToObject<object[]>()
            };
        }

        private SimpleTypeProperty CreateSimpleProperty(JToken jObject)
        {
            return new SimpleTypeProperty
            {
                Type = jObject["type"]?.ToString(),
                Format = jObject["format"]?.ToString(),
                Description = jObject["description"]?.ToString(),
                ExampleValue = jObject["exampleValue"]?.ToObject<object>()
            };
        }

        private PropertyBase CreateItemsProperty(JToken jObject)
        {
            var arrayRef = jObject["$ref"]?.ToString();
            var @enum = jObject["enum"]?.ToString();

            if (arrayRef != null) return ResolveReferenceProperty(jObject);

            if (@enum != null)
            {
                return CreateEnumProperty(jObject);
            }

            return CreateSimpleProperty(jObject);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override bool CanWrite => true;

        public override bool CanRead => true;

        protected virtual ReferenceProperty ResolveReferenceProperty(JToken jObject)
        {
            return new ReferenceProperty
            {
                Ref = jObject["$ref"]?.ToString(),
                Description = jObject["description"]?.ToString()
            };
        }
    }
}