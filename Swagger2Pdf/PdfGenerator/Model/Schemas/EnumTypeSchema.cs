using iText.Layout.Element;

namespace Swagger2Pdf.PdfGenerator.Model.Schemas
{
    public class EnumTypeSchema : Schema
    {
        public EnumTypeSchema(string type, object[] enumValues, object defaultValue, string collectionFormat)
        {
            Type = type;
            EnumValues = enumValues;
            DefaultValue = defaultValue;
            CollectionFormat = collectionFormat;
        }

        public string Type { get; }
        public object[] EnumValues { get; }
        public object DefaultValue { get; }
        public string CollectionFormat { get; }

        public override void WriteDetailedDescription(Paragraph paragraph)
        {
            if (CollectionFormat == "multi")
            {   
                paragraph.Add("Multiple values allowed");
                paragraph.AddLineBreak();
            }

            if (DefaultValue != null)
            {
                paragraph.Add("Default value: ");
                var txt = paragraph.AddFormattedText(PdfHelpers.FixedCharLengthStyle());
                txt.SetText(SwaggerPdfJsonConvert.SerializeObject(DefaultValue));
                paragraph.AddLineBreak();
            }

            paragraph.Add("Allowed values:");
            var formattedText = paragraph.AddFormattedText(PdfHelpers.FixedCharLengthStyle());
            formattedText.SetText(SwaggerPdfJsonConvert.SerializeObject(EnumValues));
        }
    }
}