using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class SwaggerInfo
    {
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
    }
}