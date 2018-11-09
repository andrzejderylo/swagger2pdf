using Swagger2Pdf.PdfGenerator.Model;
using Swagger2Pdf.PdfGenerator.Model.Schemas;

namespace Swagger2Pdf.Model.Properties
{
    public sealed class ReferenceProperty : PropertyBase
    {
        public string Ref { get; set; }

        public override Schema ResolveSchema(SchemaResolutionContext resolutionContext)
        {
            var complexTypeSchema = new ComplexTypeSchema();
            var definition = resolutionContext.ReferenceResolver.ResolveReference(null, Ref);

            foreach (var property in definition.Properties)
            {   
                complexTypeSchema.AddProperty(property.Key, property.Value.ResolveSchema(resolutionContext));
            }

            return complexTypeSchema;
        }
    }

    public class SchemaResolutionContext
    {
        public ReferenceResolver.ReferenceResolver ReferenceResolver { get; }

        public SchemaResolutionContext(ReferenceResolver.ReferenceResolver referenceResolver)
        {
            ReferenceResolver = referenceResolver;
        }
    }
}