using System.Collections.Generic;
using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class SwaggerJsonModel
    {
        [JsonProperty("swagger")]
        public string Swagger { get; set; }

        [JsonProperty("info")]
        public SwaggerJsonInfo SwaggerJsonInfo { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("basePath")]
        public string BasePath { get; set; }

        [JsonProperty("tags")]
        public Tag[] Tags { get; set; }

        [JsonProperty("schemes")]
        public string[] Schemes { get; set; }

        [JsonProperty("paths")]
        public Dictionary<string, Dictionary<string, Operation>> Paths { get; set; }

        [JsonProperty("securityDefinitions")]
        public Dictionary<string, SecurityDefinition> SecurityDefinitions { get; set; }
    }
}