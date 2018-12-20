namespace Swagger2Pdf.PdfModel.Model.Schemas
{
    public class ArraySchema : Schema
    {
        public ArraySchema(Schema arrayItemSchema, string collectionFormat)
        {
            ArrayItemSchema = arrayItemSchema;
            CollectionFormat = collectionFormat;
            Example = GetExampleString(collectionFormat);
            Description = GetDescription(collectionFormat);
        }

        private static string GetDescription(string collectionFormat)
        {
            switch (collectionFormat)
            {
                case "ssv": return "Space-separated values.";
                case "tsv": return "Tab-separated values.";
                case "pipes": return "Pipe-separated values.";
                case "multi": return "Multiple parameter instances rather than multiple values. This is only supported for the query string and formData parameters.";
                default: return "Comma-separated values.";
            }
        }

        private static string GetExampleString(string collectionFormat)
        {
            switch (collectionFormat)
            {
                case "ssv": return "foo bar baz";
                case "tsv": return "foo\\tbar\\tbaz";
                case "pipes": return "foo|bar|baz";
                case "multi": return "foo=value&foo=another_value";
                default: return "foo,bar,baz";
            }
        }

        public string Description { get; }
        public Schema ArrayItemSchema { get; }
        public string CollectionFormat { get; }
        public string Example { get; }
    }
}