using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class SwaggerInfo
    {
        [JsonProperty("definitions")]
        public Dictionary<string, Definition> Definitions { get; set; } = new Dictionary<string, Definition>();

        [JsonProperty("swagger")]
        public string Swagger { get; set; }

        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("schemes")]
        public string[] Schemes { get; set; }

        [JsonProperty("paths")]
        public Dictionary<string, Dictionary<string, Request>> Paths { get; set; }

        [JsonProperty("securityDefinitions")]
        public Dictionary<string, SecurityDefinition> SecurityDefinitions { get; set; }
    }
}