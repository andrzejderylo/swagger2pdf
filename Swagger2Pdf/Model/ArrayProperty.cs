using Newtonsoft.Json;
using Swagger2Pdf.PdfGenerator.Schemas;

namespace Swagger2Pdf.Model
{
    public class ArrayProperty : PropertyBase
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        public PropertyBase Items { get; set; }

        public override Schema CreateSchema()
        {
            return new ArraySchema(Items.CreateSchema());
        }
    }
}