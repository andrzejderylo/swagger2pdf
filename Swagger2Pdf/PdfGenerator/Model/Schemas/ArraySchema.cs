namespace Swagger2Pdf.PdfGenerator.Model.Schemas
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