using Newtonsoft.Json;

namespace Swagger2Pdf.PdfModel.Model.Schemas.Serialization
{
    public class SimpleTypeSchemaJsonConverter : SchemaJsonConverter<SimpleTypeSchema>
    {
        protected override void WriteJson(JsonWriter writer, SimpleTypeSchema value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ExampleValue);
        }
    }
}