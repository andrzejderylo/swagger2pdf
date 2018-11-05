namespace Swagger2Pdf.PdfGenerator.Schemas
{
    public class EnumTypeSchema : Schema
    {
        public EnumTypeSchema(string type, object[] enumValues, object @default, string collectionFormat)
        {
            Type = type;
            EnumValues = enumValues;
            Default = @default;
            CollectionFormat = collectionFormat;
        }

        public string Type { get; }
        public object[] EnumValues { get; }
        public object Default { get; }
        public string CollectionFormat { get; }
    }
}