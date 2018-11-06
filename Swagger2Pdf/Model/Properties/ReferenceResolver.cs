using Newtonsoft.Json.Serialization;

namespace Swagger2Pdf.Model.Properties
{
    public class ReferenceResolver : IReferenceResolver
    {
        public object ResolveReference(object context, string reference)
        {
            return new ReferenceProperty
            {
                Ref = reference
            };
        }

        public string GetReference(object context, object value)
        {
            ReferenceProperty schema = value as ReferenceProperty;
            return schema.Ref;
        }

        public bool IsReferenced(object context, object value)
        {
            ReferenceProperty schema = value as ReferenceProperty;
            return SwaggerInfoDefinitions.ResolveReference(schema.Ref) != null;
        }

        public void AddReference(object context, string reference, object value)
        {
            throw new System.NotImplementedException();
        }
    }

}