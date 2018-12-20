namespace Swagger2Pdf.PdfModel.Model.Schemas
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

        public override void WriteDetailedDescription(IAuthorizationWriter writer)
        {
            if (CollectionFormat == "multi")
            {   
                writer.AppendText("Multiple values allowed");
                writer.AddLineBreak();
            }

            if (DefaultValue != null)
            {
                writer.AppendText("Default value: ");
                writer.AddFixedSizeCharElement(PdfModelJsonConverter.SerializeObject(DefaultValue));
                writer.AddLineBreak();
            }

            writer.AppendText("Allowed values:");
            writer.AddFixedSizeCharElement(PdfModelJsonConverter.SerializeObject(EnumValues));
        }
    }
}