using Newtonsoft.Json;

namespace Swagger2Pdf.PdfGenerator.Schemas.Serialization
{
    public class EnumTypeSchemaConverter : SchemaJsonConverter<EnumTypeSchema>
    {
        protected override void WriteJson(JsonWriter writer, EnumTypeSchema value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            serializer.Serialize(writer, value.Type);
            writer.WriteEndArray();
        }
    }
}