using Newtonsoft.Json;
using Swagger2Pdf.PdfGenerator.Schemas;

namespace Swagger2Pdf.Model
{
    public abstract class PropertyBase
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        public abstract Schema CreateSchema();
    }
}