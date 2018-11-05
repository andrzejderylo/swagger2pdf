using MigraDoc.DocumentObjectModel;

namespace Swagger2Pdf.PdfGenerator.Schemas
{
    public class ArraySchema : Schema
    {
        public ArraySchema(Schema arrayItemSchema)
        {
            ArrayItemSchema = arrayItemSchema;
        }

        public Schema ArrayItemSchema { get; }
    }
}