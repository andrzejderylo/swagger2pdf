using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class Tag
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("externalDocs")]
        public ExternalDoc ExternalDocs { get; set; }

        public ExternalDoc get_ExternalDocs()
        {
        }
    }
}