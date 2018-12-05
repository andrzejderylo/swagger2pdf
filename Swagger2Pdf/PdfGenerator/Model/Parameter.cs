using iText.Kernel.Colors;
using iText.Layout.Element;
using Swagger2Pdf.PdfGenerator.Helpers;

namespace Swagger2Pdf.PdfGenerator.Model
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deprecated { get; set; }
        public bool AllowEmptyValue { get; set; }
        public string Title { get; set; }
        public string MultipleOf { get; set; }

        public string Pattern { get; set; }

        public string Maximum { get; set; }
        public string Minimum { get; set; }

        public string ExclusiveMinimum { get; set; }
        public string ExclusiveMaximum { get; set; }

        public string MinLength { get; set; }
        public string MaxLength { get; set; }
        
        
        public string MaxItems { get; set; }
        public string MinItems { get; set; }

        public string MaxProperties { get; set; }
        public string MinProperties { get; set; }

        public bool UniqueItems { get; set; }

        public bool IsRequired { get; set; }
        public string Enum { get; set; }
        public Schema Schema { get; set; }
        public string Type { get; set; }

        public void WriteDetailedDescription(Paragraph p)
        {
            if (Deprecated)
            {
                p.AddText("Deprecated").SetFontColor(ColorConstants.RED);
                Description.IfNotNull(x => p.AddLineBreak().SetFontColor(ColorConstants.RED).SetLineThrough().AddText(x));
            }
            else
            {
                Description.IfNotNull(x => p.AddText(x));
            }

            if (IsRequired)
            {
                p.AddLineBreak().AddText("Required").Bold();
            }

            Pattern.IfNotNull(x => p.AddLineBreak().AddText($"Pattern: {x}"));
            GetMinMaxString().IfNotNull(x => p.AddLineBreak().AddText(x));
            GetExclusiveMinMaxString().IfNotNull(x => p.AddLineBreak().AddText(x));
            GetMinMaxItems().IfNotNull(x => p.AddLineBreak().AddText(x));
            GetMinMaxProperties().IfNotNull(x => p.AddLineBreak().AddText(x));
        }

        private string GetMinMaxString()
        {
            return GetTwoPartString("Minimum", Minimum, "maximum", Maximum);
        }

        private string GetExclusiveMinMaxString()
        {
            return GetTwoPartString("Exclusive minimum", ExclusiveMinimum, "exclusive maximum", ExclusiveMaximum);
        }

        private string GetMinMaxItems()
        {
            return GetTwoPartString("Minimum items", MinItems, "maximum items", MaxItems);
        }

        private string GetMinMaxProperties()
        {
            return GetTwoPartString("Minimum properties", MinProperties, "maximum properties", MaxProperties);
        }

        private static string GetTwoPartString(string leftPartLabel, string leftPart, string rightPartLabel, string rightPart)
        {
            if (leftPart.IsNullOrEmpty() && rightPart.IsNullOrEmpty())
            {
                return null;
            }

            if (!leftPart.IsNullOrEmpty() && !rightPart.IsNullOrEmpty())
            {
                return $"{leftPartLabel.FirstCharToUpper()}: {leftPart}, {rightPartLabel.ToLower()}: {rightPart}";
            }

            if (!leftPart.IsNullOrEmpty())
            {
                return $"{leftPartLabel.FirstCharToUpper()}: {leftPart}";
            }

            if (!rightPart.IsNullOrEmpty())
            {
                return $"{rightPartLabel.FirstCharToUpper()}: {rightPart}";
            }

            return null;
        }
    }
}