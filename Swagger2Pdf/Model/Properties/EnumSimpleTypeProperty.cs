using Newtonsoft.Json;
using Swagger2Pdf.PdfModel.Model;
using Swagger2Pdf.PdfModel.Model.Schemas;

namespace Swagger2Pdf.Model.Properties
{
    public class EnumSimpleTypeProperty : SimpleTypeProperty
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