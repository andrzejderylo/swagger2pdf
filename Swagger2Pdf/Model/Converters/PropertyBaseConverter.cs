using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Swagger2Pdf.Model.Converters
{
    public class PropertyBaseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PropertyBase);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jObject = JToken.ReadFrom(reader);
            string description = jObject["description"]?.ToString();

            if (jObject["$ref"] != null)
            {
                return new ReferenceProperty
                {   
                    Description = description,
                    Ref = jObject["$ref"].ToString()
                };
            }

            string type = jObject["type"]?.ToString();

            if (!string.IsNullOrEmpty(type) && type == "array")
            {
                return new ArrayProperty
                {
                    Description = description,
                    Type = "array",
                    Items = CreateItemsProperty(jObject)
                };
            }

            return jObject.ToObject<Property>();
        }

        private PropertyBase CreateItemsProperty(JToken jObject)
        {
            var arrayRef = jObject["items"]["$ref"]?.ToString();
            if (arrayRef == null)
            {
                var collectionFormat = jObject["collectionFormat"]?.ToString();
                var property = jObject["items"].ToObject<Property>();
                property.CollectionFormat = collectionFormat;
                return property;
            }

            return new ReferenceProperty { Ref = arrayRef };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override bool CanWrite => true;

        public override bool CanRead => true;
    }
}