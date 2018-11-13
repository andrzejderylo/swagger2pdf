using System;
using Newtonsoft.Json;
using Swagger2Pdf.PdfGenerator.Model;
using Swagger2Pdf.PdfGenerator.Model.Schemas;

namespace Swagger2Pdf.Model.Properties
{
    public class Property : PropertyBase
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("example")]
        public object ExampleValue { get; set; }

        public override Schema ResolveSchema(SchemaResolutionContext resolutionContext)
        {
            return new SimpleTypeSchema(Format, Type, GetExampleValue());
        }

        private object GetExampleValue()
        {
            if (ExampleValue != null)
            {
                return ExampleValue;
            }

            switch (Type)
            {
                case "string":
                    switch (Format)
                    {
                        case "date-time": return DateTime.Now;
                        default: return "string";
                    }
                case "integer": return 0;
                case "object": return new object();
                case "boolean": return true;
                default: return null;
            }
        }
    }
}