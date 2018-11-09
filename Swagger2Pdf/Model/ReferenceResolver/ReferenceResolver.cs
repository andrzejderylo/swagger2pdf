using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Swagger2Pdf.Model.ReferenceResolver
{
    public class ReferenceResolver
    {
        [JsonProperty("definitions")]
        public Dictionary<string, Definition> Definitions { get; set; } = new Dictionary<string, Definition>();

        public Definition ResolveReference(object context, string reference)
        {   
            if(Definitions.TryGetValue(GetReferenceKey(reference), out var definition))
            {
                return definition;
            }

            return null;
        }
        
        private static string GetReferenceKey(string reference)
        {
            return reference.Split('/').Last();
        }
    }

}