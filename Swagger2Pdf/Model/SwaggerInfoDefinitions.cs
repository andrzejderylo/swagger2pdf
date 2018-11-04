using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
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

        internal static Definition ResolveReference(string reference)
        {
            var key = reference.Split('/').Last();

            if(DefinitionsStatic == null || !DefinitionsStatic.TryGetValue(key, out var definitionValue))
            {
                return null;
            }

            return definitionValue;
        }
    }


}