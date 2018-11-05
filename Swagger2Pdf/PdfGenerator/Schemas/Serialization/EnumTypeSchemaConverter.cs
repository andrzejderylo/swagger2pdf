using System.Linq;
using Newtonsoft.Json;

namespace Swagger2Pdf.PdfGenerator.Schemas.Serialization
{
    public class EnumTypeSchemaConverter : SchemaJsonConverter<EnumTypeSchema>
    {
        protected override void WriteJson(JsonWriter writer, EnumTypeSchema value, JsonSerializer serializer)
        {
            if (value.CollectionFormat == "multi")
            {
                serializer.Serialize(writer, value.EnumValues);
                return;
            }

            serializer.Serialize(writer, value.Default ?? value.EnumValues.FirstOrDefault());
        }
    }
}