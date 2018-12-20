using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Swagger2Pdf.PdfModel.Model.Schemas
{
    public class ComplexTypeSchema : Schema
    {
        private readonly Dictionary<string, Schema> _documentationObject;

        public ComplexTypeSchema()
        {
            _documentationObject = new Dictionary<string, Schema>();
        }

        public void AddProperty(string propertyName, Schema propertySchema)
        {
            if (!_documentationObject.ContainsKey(propertyName))
            {
                _documentationObject.Add(propertyName, propertySchema);
            }
            _documentationObject[propertyName] = propertySchema;
        }

        public IReadOnlyDictionary<string, Schema> Properties => new ReadOnlyDictionary<string, Schema>(_documentationObject);
    }
}