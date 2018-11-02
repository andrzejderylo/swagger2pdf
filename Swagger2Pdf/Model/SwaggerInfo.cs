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
        public Dictionary<string, Dictionary<string, Path>> Paths { get; set; }

    }

    public class SwaggerInfoDefinitions
    {
        private Dictionary<string, Definition> _definitions;

        [JsonProperty("definitions")]
        public Dictionary<string, Definition> Definitions
        {
            get => _definitions;
            set
            {
                _definitions = value;
                DefinitionsStatic = value;
            }
        }

        public static Dictionary<string, Definition> DefinitionsStatic { get; set; }
    }


}