using System;
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
        public string Maximum { get; set; }
        public string ExclusiveMaximum { get; set; }
        public string Minimum { get; set; }
        public string ExclusiveMinimum { get; set; }
        public string MaxLength { get; set; }
        public string MinLength { get; set; }
        public string Pattern { get; set; }
        public string MaxItems { get; set; }
        public string MinItems { get; set; }
        public string UniqueItems { get; set; }
        public string MaxProperties { get; set; }
        public string MinProperties { get; set; }
        public bool IsRequired { get; set; }
        public string Enum { get; set; }
        public Schema Schema { get; set; }
        public string Type { get; set; }

        public void WriteDetailedDescription(Paragraph p)
        {
            p.AddText(Description);
            if (Deprecated)
            {
                p.AddText("Deprecated").SetFontColor(ColorConstants.RED);
            }

            Pattern.IfNotNull(x => p.AddLineBreak().AddText($"Pattern: {x}"));
            
        }
    }

    public static class StringHelper
    {
        public static void IfNotNull(this string s, Action<string> func = null)
        {
            if (!s.IsNullOrEmpty())
            {
                func?.Invoke(s);
            }
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
    }

}