using Newtonsoft.Json;

namespace Swagger2Pdf.PdfGenerator.Model.Schemas.Serialization
{
    public class ComplexTypeSchemaJsonConverter : SchemaJsonConverter<ComplexTypeSchema>
    {
        protected override void WriteJson(JsonWriter writer, ComplexTypeSchema value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.Properties);
        }
    }
}