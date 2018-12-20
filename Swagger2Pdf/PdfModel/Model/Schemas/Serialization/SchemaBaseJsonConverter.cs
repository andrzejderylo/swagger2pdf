using Newtonsoft.Json;

namespace Swagger2Pdf.PdfModel.Model.Schemas.Serialization
{
    public class SchemaBaseJsonConverter : SchemaJsonConverter<Schema>
    {
        protected override void WriteJson(JsonWriter writer, Schema value, JsonSerializer serializer)
        {
        }
    }
}