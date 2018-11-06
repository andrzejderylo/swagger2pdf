using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using Font = MigraDoc.DocumentObjectModel.Font;

namespace Swagger2Pdf.PdfGenerator
{
    internal static class MigradocHelpers
    {
        public static Cell VerticallyCenteredContent(this Cell cell)
        {   
            cell.VerticalAlignment = VerticalAlignment.Center;
            return cell;
        }

        public static Paragraph PullRight(this Paragraph paragraph)
        {
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            return paragraph;
        }

        public static Paragraph AddPageBreakableParagraph(this Section cell)
        {
            var paragraph = cell.AddParagraph();
            paragraph.AddBorders();
            paragraph.Format.KeepTogether = false;
            return paragraph;
        }

        public static Paragraph AddPageBreakableParagraph(this Section cell, string paragraphText)
        {
            var paragraph = cell.AddParagraph(paragraphText);
            paragraph.AddBorders();
            paragraph.Format.KeepTogether = false;
            return paragraph;
        }

        public static Paragraph AddBorders(this Paragraph paragraph)
        {   
            paragraph.Format.Borders = new Borders
            {
                Color = Colors.Black
            };
            return paragraph;
        }

        public static Paragraph AsFixedCharLength(this Paragraph paragraph)
        {
            paragraph.Style = "FixedCharLengthStyle";
            return paragraph;
        }

        public static Paragraph AsHeader(this Paragraph paragraph)
        {
            paragraph.Style = "Header";
            return paragraph;
        }

        public static Paragraph AsTitle(this Paragraph paragraph)
        {
            paragraph.Style = "Title";
            return paragraph;
        }

        public static Paragraph Centered(this Paragraph paragraph)
        {
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            return paragraph;
        }

        public static Paragraph AsSubHeader(this Paragraph paragraph)
        {
            paragraph.Style = "SubHeader";
            return paragraph;
        }

        public static Paragraph Bold(this Paragraph paragraph)
        {
            paragraph.Format.Font.Bold = true;
            return paragraph;
        }

        public static Table AddBorderedTable(this Section section)
        {
            var table = section.AddTable();
            table.Borders = new Borders { Color = Colors.Black };
            return table;
        }

        public static Style FixedCharLengthStyle()
        {
            return new Style("FixedCharLengthStyle", "Normal")
            {
                Font = FixedCharLengthFont
            };
        }

        public static Font FixedCharLengthFont => new Font("Courier New")
        {
            Size = Unit.FromPoint(9)
        };

        public static Style Header()
        {
            var style = new Style("Header", "Normal")
            {
                Font =
                {
                    Size = Unit.FromPoint(13),
                    Bold = true
                }
            };
            return style;
        }

        public static Style SubHeader()
        {
            var style = new Style("SubHeader", "Normal")
            {
                Font =
                {
                    Size = Unit.FromPoint(11),
                }
            };
            return style;
        }

        private static Style Title()
        {
            
            var style = new Style("Title", "Normal")
            {
                Font =
                {
                    Size = Unit.FromPoint(20),
                    Bold = true
                }
            };
            return style;
        }

        public static void DefineStyles(this Document document)
        {
            if (document.Styles == null)
            {
                document.Styles = new Styles();
            }

            document.Styles.Add(FixedCharLengthStyle());
            document.Styles.Add(SubHeader());
            document.Styles.Add(Header());
            document.Styles.Add(Title());
        }
    }

}
