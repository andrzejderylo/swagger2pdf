using System;
using System.Text;

namespace Swagger2Pdf.PdfGenerator.Model.Schemas
{
    public class SimpleTypeSchema : Schema
    {
        public SimpleTypeSchema(string type, string format, object exampleValue)
        {
            Type = type;
            Format = format;
            ExampleValue = exampleValue ?? GetExampleValue();
        }

        public string Type { get; }
        public string Format { get; }
        public object ExampleValue { get; }

        private object GetExampleValue()
        {
            switch (Type)
            {
                case "string":
                    switch (Format)
                    {
                        case "byte": return Convert.ToBase64String(Encoding.Unicode.GetBytes("example byte-encoded value"));
                        case "binary": return "binary string";
                        case "date": return DateTime.Now.Date;
                        case "date-time": return DateTime.Now;
                        case "password": return "***********";
                        default: return "string";
                    }
                case "integer": return 12345;
                case "number": return 123.45D;
                case "object": return new object();
                case "boolean": return true;
                default: return null;
            }
        }
    }
}