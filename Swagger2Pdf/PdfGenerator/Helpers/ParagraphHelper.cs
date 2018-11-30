using System;
using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Swagger2Pdf.PdfGenerator.Helpers
{
    internal static class ParagraphHelper
    {
        public static Paragraph AddLineBreak(this Paragraph paragraph)
        {   
            return paragraph.Add("\r\n");
        }

        public static Paragraph AddBorders(this Paragraph paragraph)
        {
            paragraph.AddStyle(BorderedStyle());
            return paragraph;
        }

        public static Text AddText(this Paragraph paragraph)
        {
            return paragraph.AddText(string.Empty);
        }

        public static Text AddText(this Paragraph paragraph, string text)
        {
            var textControl = new Text(text);
            paragraph.Add(textControl);
            return textControl;
        }

        public static Table CreateBorderedTable(float[] columns)
        {
            var table = new Table(columns);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.AddStyle(BorderedStyle());
            return table;
        }

        public static Style BorderedStyle()
        {
            var border = new SolidBorder(ColorConstants.BLACK, 1);
            var borderedStyle = new Style().SetBorder(border);
            return borderedStyle;
        }
    }
}
