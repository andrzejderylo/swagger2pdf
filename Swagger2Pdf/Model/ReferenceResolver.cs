using System.Linq;
using Newtonsoft.Json.Serialization;

namespace Swagger2Pdf.Model
{
    public class ReferenceResolver : IReferenceResolver
    {
        public object ResolveReference(object context, string reference)
        {
            var key = reference.Split('/').Last();
            var result = SwaggerInfoDefinitions.DefinitionsStatic[key];
            return result;
        }

        public string GetReference(object context, object value)
        {
            throw new System.NotImplementedException();
        }

        public bool IsReferenced(object context, object value)
        {
            throw new System.NotImplementedException();
        }

        public void AddReference(object context, string reference, object value)
        {
            throw new System.NotImplementedException();
        }
    }

}