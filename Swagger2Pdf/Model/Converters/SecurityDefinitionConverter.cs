using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swagger2Pdf.Model.SecurityDefinitions;

namespace Swagger2Pdf.Model.Converters
{
    public class SecurityDefinitionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(SecurityDefinition) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jObject = JToken.ReadFrom(reader);
            string type = jObject["type"]?.ToString();
            if (type == "oauth2")
            {
                return jObject.ToObject<OAuthSecurityDefinition>();
            }

            return jObject.ToObject<ParameterSecurityDefinition>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;

        public override bool CanRead => true;
    }
}
