using System.Collections.Generic;
using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class Operation
    {
        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("externalDocs")]
        public ExternalDoc ExternalDocs { get; set; }
        
        [JsonProperty("operationId")]
        public string OperationId { get; set; }
        
        [JsonProperty("parameters")]
        public OperationParameter[] OperationParameters { get; set; }

        [JsonProperty("consumes")]
        public string[] Consumes { get; set; }

        [JsonProperty("produces")]
        public string[] Produces { get; set; }

        [JsonProperty("responses")]
        public Dictionary<string, OperationResponse> Responses { get; set; }

        [JsonProperty("deprecated")]
        public bool Deprecated { get; set; }
    }
}