using Newtonsoft.Json;
using Swagger2Pdf.PdfGenerator.Schemas;

namespace Swagger2Pdf.Model
{
    public class Property : PropertyBase
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public override Schema CreateSchema()
        {
            return new SimpleTypeSchema(Format, Type, GetExampleValue(Type));
        }

        private static object GetExampleValue(string type)
        {
            switch (type)
            {
                case "string": return "string";
                case "integer": return 0;
                case "object": return new object();
                default: return null;
            }
        }
    }
}