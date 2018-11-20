using System;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Swagger2Pdf.PdfGenerator
{
    internal static class PdfHelpers
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

        public static void AddParagraph(this Document document, Action<Paragraph> p = null)
        {
            var paragraph = new Paragraph();
            p?.Invoke(paragraph);
            document.Add(paragraph);
        }
        
        public static void AddParagraph(this Document document, string text, Action<Paragraph> p = null)
        {
            var paragraph = new Paragraph(text);
            p?.Invoke(paragraph);
            document.Add(paragraph);
        }

        public static Paragraph AddLineBreak(this Paragraph paragraph)
        {   
            return paragraph.Add("\r\n");
        }

        public static void AddAreaBreak(this Document document)
        {
            document.Add(new AreaBreak());
        }

        public static Image AddImage(this Document document, string imagePath)
        {
            var imgData = ImageDataFactory.Create(new Uri(imagePath));
            var image = new Image(imgData);
            document.Add(image);
            return image;
        }

        public static void AddPageBreakableParagraph(this Document document, string text, Action<Paragraph> p = null)
        {
            document.AddParagraph(text, p);
        }

        public static Paragraph AddBorders(this Paragraph paragraph)
        {
            paragraph.AddStyle(BorderedStyle());
            return paragraph;
        }

        public static Paragraph AsFixedCharLength(this Paragraph paragraph)
        {
            paragraph.AddStyle(FixedCharLengthStyle());
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

        public static Text AddFormattedText(this Paragraph paragraph, Style style)
        {
            var text = new Text(string.Empty).AddStyle(style);
            paragraph.Add(text);
            return text;
        }

        public static Table CreateBorderedTable(float[] columns)
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

        public static PdfFont FixedCharLengthFont => PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.COURIER);
        public static PdfFont NormalLengthFont => PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.TIMES_ROMAN);

        public static Style BorderedStyle()
        {
            var border = new SolidBorder(ColorConstants.BLACK, 1);
            var borderedStyle = new Style().SetBorder(border);
            return borderedStyle;
        }
    }

    public static class CellHelpers
    {
        public static Cell AddParagraph(this Cell cell, string text)
        {
            var p = new Paragraph(text);
            cell.Add(p);
            return cell;
        }
    }


}
