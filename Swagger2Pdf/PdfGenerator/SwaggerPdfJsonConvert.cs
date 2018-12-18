using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Swagger2Pdf.PdfGenerator.Model.Schemas.Serialization;

namespace Swagger2Pdf.PdfGenerator
{
    public static class SwaggerPdfJsonConvert
    {
        private static JsonSerializerSettings SerializerSettingsFactory()
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
            if (settings.Converters == null)
            {
                settings.Converters = new List<JsonConverter>();
            }

            settings.Converters.Add(new ArraySchemaJsonConverter());
            settings.Converters.Add(new SimpleTypeSchemaJsonConverter());
            settings.Converters.Add(new ComplexTypeSchemaJsonConverter());
            settings.Converters.Add(new SchemaBaseJsonConverter());
            settings.Converters.Add(new EnumTypeSchemaConverter());

            return settings;
        }

        public static string SerializeObject(object obj)
        {
            using (TextWriter sw = new StringWriter())
            {
                using (JsonTextWriterEx jw = new JsonTextWriterEx(sw))
                {
                    
                    jw.Indentation = 2;
                    jw.NewLine = "\u000A";
                    jw.IndentChar = '\u00A0';
                    jw.Formatting = Formatting.Indented;
                    JsonSerializer serializer = JsonSerializer.Create(SerializerSettingsFactory());
                    serializer.Serialize(jw, obj);
                    return sw.ToString();
                }
            }
        }

        public class JsonTextWriterEx : JsonTextWriter
        {
            public string NewLine { get; set; }

            public JsonTextWriterEx(TextWriter textWriter) : base(textWriter)
            {
                NewLine = Environment.NewLine;
            }

            protected override void WriteIndent()
            {
                if (Formatting == Formatting.Indented)
                {
                    WriteWhitespace(NewLine);
                    int currentIndentCount = Top * Indentation;
                    for (int i = 0; i < currentIndentCount; i++)
                        WriteIndentSpace();
                }
            }
        }
    }
}