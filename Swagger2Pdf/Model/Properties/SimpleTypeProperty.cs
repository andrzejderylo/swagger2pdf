using Newtonsoft.Json;
using Swagger2Pdf.PdfModel.Model;
using Swagger2Pdf.PdfModel.Model.Schemas;

namespace Swagger2Pdf.Model.Properties
{
    public class SimpleTypeProperty : PropertyBase
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("example")]
        public object ExampleValue { get; set; }

        public override Schema ResolveSchema(SchemaResolutionContext resolutionContext)
        {
            return new SimpleTypeSchema(Type, Format, ExampleValue);
        }
    }
}