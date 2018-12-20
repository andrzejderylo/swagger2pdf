using System.Linq;
using Swagger2Pdf.PdfModel;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public class HtmlAuthorizationWriter : IAuthorizationWriter
    {
        private readonly HtmlElement _element;
        private TextContent _textContent;
        private TableElement _table;

        public HtmlAuthorizationWriter(HtmlElement element)
        {
            _element = element;
        }

        public void AppendText(string text)
        {
            if (_textContent == null)
            {
                _textContent = new TextContent();
                _element.AddChildElement(HtmlElement.Label().AddChildElement(_textContent));
            }

            _textContent.AppendText(text);
        }

        public void AddLineBreak()
        {
            _textContent.LineBreak();
        }

        public void AddTable(params string[] headers)
        {
            if (_table == null)
            {
                _table = HtmlElement.Table();
                _element.AddChildElement(_table);
            }

            _table.AddColumns(headers.Select(x => new TextContent(x)).ToArray());
        }

        public void AddTableRow(params string[] cellContent)
        {
            if (_table == null)
            {
                _table = HtmlElement.Table();
                _element.AddChildElement(_table);
            }

            _table.AddRow(cellContent.Select(x => new TextContent(x)).ToArray());
        }

        public void AddFixedSizeCharElement(string text)
        {
            _element.AddChildElement(HtmlElement.Pre().SetText(text));
            _textContent = null;
        }
    }
}