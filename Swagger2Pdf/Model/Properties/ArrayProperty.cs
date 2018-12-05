using Newtonsoft.Json;
using Swagger2Pdf.PdfGenerator.Model;
using Swagger2Pdf.PdfGenerator.Model.Schemas;

namespace Swagger2Pdf.Model.Properties
{
    public class ArrayProperty : PropertyBase
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        public PropertyBase Items { get; set; }

        public string CollectionFormat { get; set; }

        public override Schema ResolveSchema(SchemaResolutionContext resolutionContext)
        {
            return new ArraySchema(Items.ResolveSchema(resolutionContext), CollectionFormat);
        }
    }
}