using iText.Layout.Element;
using iText.Layout.Properties;

namespace Swagger2Pdf.PdfGenerator.Helpers
{
    public static class CellHelper
    {
        public static Cell VerticallyCentered(this Cell cell)
        {   
            cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            return cell;
        }

        public static Cell AddParagraph(this Cell cell, string text)
        {
            var p = new Paragraph(text);
            cell.Add(p);
            return cell;
        }
    }


}
