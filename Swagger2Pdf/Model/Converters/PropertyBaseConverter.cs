using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swagger2Pdf.Model.Properties;

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
            var jObject = JToken.ReadFrom(reader);
            var @ref = jObject["$ref"]?.ToString();
            var @enum = jObject["enum"]?.ToString();
            var type = jObject["type"]?.ToString();

            if (!string.IsNullOrEmpty(@ref))
            {   
                return ResolveReferenceProperty(@ref, jObject, serializer);
            }
            
            if (!string.IsNullOrEmpty(@enum))
            {
                var enumProperty = jObject.ToObject<EnumProperty>();
                return enumProperty;
            }

            if (!string.IsNullOrEmpty(type) && type == "array")
            {   
                return new ArrayProperty
                {
                    Description = jObject["description"]?.ToString(),
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

            var arrayItemReference = jObject["items"].ToObject<ReferenceProperty>();
            return arrayItemReference;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override bool CanWrite => true;

        public override bool CanRead => true;

        protected virtual ReferenceProperty ResolveReferenceProperty(string reference, JToken jObject, JsonSerializer serializer)
        {
            return new ReferenceProperty
            {
                Ref = reference,
                Description = jObject["description"]?.ToString()
            };

            //var refResolver = serializer.ReferenceResolver;
            //if (!refResolver.IsReferenced(null, reference))
            //{   
            //    refResolver.AddReference(null, reference, new object());
            //}
            //
            //return refResolver.ResolveReference(null, reference) as ReferenceProperty;
        }
    }
}