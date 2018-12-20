namespace Swagger2Pdf.PdfModel
{
    public interface IAuthorizationWriter
    {
        void AppendText(string text);
        void AddLineBreak();
        void AddTable(params string[] headers);
        void AddTableRow(params string[] cellContent);
        void AddFixedSizeCharElement(string text);
    }
}
