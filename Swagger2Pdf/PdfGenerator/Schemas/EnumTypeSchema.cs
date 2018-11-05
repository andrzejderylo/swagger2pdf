using MigraDoc.DocumentObjectModel;

namespace Swagger2Pdf.PdfGenerator.Schemas
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
                paragraph.AddText("Multiple values allowed");
                paragraph.AddLineBreak();
            }

            if (DefaultValue != null)
            {
                paragraph.AddText("Defrault value: ");
                var txt = paragraph.AddFormattedText(MigradocHelpers.FixedCharLengthFont);
                txt.AddText(SwaggerPdfJsonConvert.SerializeObject(DefaultValue));
                paragraph.AddLineBreak();
            }

            paragraph.AddText("Allowed values:");
            var formattedText = paragraph.AddFormattedText(SwaggerPdfJsonConvert.SerializeObject(EnumValues));
            formattedText.Font = MigradocHelpers.FixedCharLengthFont;
        }
    }
}