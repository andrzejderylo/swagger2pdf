using System.Text;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public sealed class TextContent : HtmlElement
    {
        private readonly string _text;

        public TextContent(string text) : base("")
        {
            _text = text;
        }

        public override void WriteStartTag(StringBuilder htmlStringBuilder)
        {
            // This is a raw text element so there is no need to implement this
        }

        public override void WriteEndTag(StringBuilder htmlStringBuilder)
        {
            // This is a raw text element so there is no need to implement this
        }

        public override void WriteAttributes(StringBuilder htmlStringBuilder)
        {
            // This is a raw text element so there is no need to implement this
        }

        public override void WriteContent(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append(_text);
        }
    }
}