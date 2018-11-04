using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class ReferenceProperty : PropertyBase
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
                if(_definition == null)
                {
                    TryResolveReference();
                }
                return _definition;
            }
            set
            {
                _definition = value;
            }
        }
    }
}