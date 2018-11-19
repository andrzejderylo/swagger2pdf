using System.Collections.Specialized;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Swagger2Pdf.PdfGenerator
{
    internal static class MigradocHelpers
    {
        public static Cell VerticallyCenteredContent(this Cell cell)
        {   
            cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            return cell;
        }

        public static Paragraph PullRight(this Paragraph paragraph)
        {
            paragraph.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
            return paragraph;
        }

        public static Paragraph AddPageBreakableParagraph(this BitVector32.Section cell)
        {
            var paragraph = cell.AddParagraph();
            paragraph.AddBorders();
            paragraph.Format.KeepTogether = false;
            return paragraph;
        }

        public static Paragraph AddPageBreakableParagraph(this BitVector32.Section cell, string paragraphText)
        {
            var paragraph = cell.AddParagraph(paragraphText);
            paragraph.AddBorders();
            paragraph.Format.KeepTogether = false;
            return paragraph;
        }

        public static Paragraph AddBorders(this Paragraph paragraph)
        {
            paragraph.AddStyle(BorderedStyle());
            return paragraph;
        }

        public static Paragraph AsFixedCharLength(this Paragraph paragraph)
        {
            paragraph.SetFont(FixedCharLengthFont);
            return paragraph;
        }

        public static Paragraph AsHeader(this Paragraph paragraph)
        {
            paragraph.SetFont(NormalLengthFont);
            paragraph.SetFontSize(13);
            paragraph.SetBold();
            return paragraph;
        }

        public static Paragraph AsTitle(this Paragraph paragraph)
        {
            paragraph.SetFont(NormalLengthFont);
            paragraph.SetFontSize(20);
            paragraph.SetBold();
            return paragraph;
        }

        public static Paragraph Centered(this Paragraph paragraph)
        {
            paragraph.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            return paragraph;
        }

        public static Paragraph AsSubHeader(this Paragraph paragraph)
        {
            paragraph.SetFontSize(11);
            return paragraph;
        }

        public static Paragraph Bold(this Paragraph paragraph)
        {
            paragraph.SetBold();
            return paragraph;
        }

        public static Table AddBorderedTable(this BitVector32.Section section, float[] columns)
        {
            var table = new Table(columns);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.AddStyle(BorderedStyle());
            return table;
        }

        public static Style FixedCharLengthStyle()
        {
            var style = new Style();
            style.SetFont(FixedCharLengthFont);
            style.SetFontSize(9);
            return style;
        }

        public static PdfFont FixedCharLengthFont => PdfFontFactory.CreateFont("Courier New");
        public static PdfFont NormalLengthFont => PdfFontFactory.CreateFont("Times New Roman");

        public static Style BorderedStyle()
        {
            var border = new SolidBorder(ColorConstants.BLACK, 1);
            var borderedStyle = new Style().SetBorder(border);
            return borderedStyle;
        }
    }

}
