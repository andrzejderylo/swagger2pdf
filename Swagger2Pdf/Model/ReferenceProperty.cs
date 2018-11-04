using Newtonsoft.Json;
using Swagger2Pdf.PdfGenerator.Schemas;

namespace Swagger2Pdf.Model
{
    public sealed class ReferenceProperty : PropertyBase
    {
        private string _ref;
        private Definition _definition;

        [JsonProperty("$ref")]
        public string Ref
        {
            get => _ref;
            set { _ref = value; TryResolveReference(); }
        }

        private void TryResolveReference()
        {
            _definition = SwaggerInfoDefinitions.ResolveReference(Ref);
        }

        public Definition Definition
        {
            get
            {
                if (_definition == null)
                {
                    TryResolveReference();
                }
                return _definition;
            }
            set => _definition = value;
        }

        public override Schema CreateSchema()
        {
            var complexTypeSchema = new ComplexTypeSchema();
            foreach (var property in Definition.Properties)
            {
                complexTypeSchema.AddProperty(property.Key, property.Value.CreateSchema());
            }

            return complexTypeSchema;
        }
    }
}