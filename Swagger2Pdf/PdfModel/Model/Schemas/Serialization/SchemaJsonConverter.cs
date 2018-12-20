using System;
using Newtonsoft.Json;

namespace Swagger2Pdf.PdfModel.Model.Schemas.Serialization
{
    public abstract class SchemaJsonConverter<TSchema> : JsonConverter
        where TSchema : Schema
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            WriteJson(writer, value as TSchema, serializer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TSchema);
        }

        public override bool CanRead => false;

        public override bool CanWrite => true;

        protected abstract void WriteJson(JsonWriter writer, TSchema value, JsonSerializer serializer);
    }
}