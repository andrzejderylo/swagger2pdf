using Newtonsoft.Json;
using Swagger2Pdf.PdfGenerator.Model;
using Swagger2Pdf.PdfGenerator.Model.Schemas;

namespace Swagger2Pdf.Model.Properties
{
    public class EnumProperty : Property
    {
        [JsonProperty("default")]
        public object Default { get; set; }
        
        [JsonProperty("enum")]
        public object[] EnumValues { get; set; }

        public string CollectionFormat { get; set; }

        public override Schema ResolveSchema(SchemaResolutionContext resolutionContext)
        {
            return new EnumTypeSchema(Type, EnumValues, Default, CollectionFormat);
        }
    }
}