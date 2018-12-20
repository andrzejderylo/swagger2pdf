using System;
using System.Text;

namespace Swagger2Pdf.PdfModel.Model.Schemas
{
    public class SimpleTypeSchema : Schema
    {
        public SimpleTypeSchema(string type, string format, object exampleValue)
        {
            Type = type;
            Format = format;
            ExampleValue = exampleValue ?? GetExampleValue(type, format);
        }

        private static int CurrentIdentifier = 0;
        private static double CurrentDouble = 0.5;

        public string Type { get; }
        public string Format { get; }
        public object ExampleValue { get; }

        private static object GetExampleValue(string type, string format)
        {
            switch (type)
            {
                case "string":
                    switch (format)
                    {
                        case "byte": return Convert.ToBase64String(Encoding.Unicode.GetBytes("example byte-encoded value"));
                        case "binary": return "binary string";
                        case "date": return DateTime.Now.Date;
                        case "date-time": return DateTime.Now;
                        case "password": return "***********";
                        default: return "string";
                    }
                case "integer": return ++CurrentIdentifier;
                case "number": return CurrentDouble += 1;
                case "object": return new object();
                case "boolean": return true;
                default: return null;
            }
        }
    }
}