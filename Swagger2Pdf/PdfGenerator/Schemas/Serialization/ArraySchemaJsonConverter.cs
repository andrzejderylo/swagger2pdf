using Newtonsoft.Json;

namespace Swagger2Pdf.PdfGenerator.Schemas.Serialization
{
    public class ArraySchemaJsonConverter : SchemaJsonConverter<ArraySchema>
    {
        protected override void WriteJson(JsonWriter writer, ArraySchema value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            serializer.Serialize(writer, value.ArrayItemSchema);
            writer.WriteEndArray();
        }
    }
}
