using System.Collections.Generic;
using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class Request
    {
        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("operationId")]
        public string OperationId { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("consumes")]
        public string[] Consumes { get; set; }

        [JsonProperty("produces")]
        public string[] Produces { get; set; }

        [JsonProperty("parameters")]
        public Parameter[] Parameters { get; set; }

        [JsonProperty("responses")]
        public Dictionary<string, Response> Responses { get; set; }

        [JsonProperty("deprecated")]
        public bool Deprecated { get; set; }
    }
}