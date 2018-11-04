using Newtonsoft.Json;

namespace Swagger2Pdf.PdfGenerator.Schemas.Serialization
{
    public class SimpleTypeSchemaJsonConverter : SchemaJsonConverter<SimpleTypeSchema>
    {
        protected override void WriteJson(JsonWriter writer, SimpleTypeSchema value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ExampleValue);
        }
    }
}