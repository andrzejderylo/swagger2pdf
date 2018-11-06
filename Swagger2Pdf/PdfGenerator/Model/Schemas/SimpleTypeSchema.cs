namespace Swagger2Pdf.PdfGenerator.Model.Schemas
{
    public class SimpleTypeSchema : Schema
    {
        public SimpleTypeSchema(string type, string format, object exampleValue)
        {
            Type = type;
            Format = format;
            ExampleValue = exampleValue;
        }

        public string Type { get; set; }
        public string Format { get; set; }
        public object ExampleValue { get; set; }
    }
}