using System.Text;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public sealed class RawContent : HtmlElement
    {
        private readonly StringBuilder _rawContentStringBuilder;

        public RawContent(StringBuilder rawContentStringBuilder) : base("")
        {
            _rawContentStringBuilder = rawContentStringBuilder;
        }

        protected override void WriteStartTag(StringBuilder htmlStringBuilder)
        {
            // This is a raw text element so there is no need to implement this
        }

        protected override void WriteEndTag(StringBuilder htmlStringBuilder)
        {
            // This is a raw text element so there is no need to implement this
        }

        protected override void WriteAttributes(StringBuilder htmlStringBuilder)
        {
            // This is a raw text element so there is no need to implement this
        }

        protected override void WriteContent(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append(_rawContentStringBuilder);
        }
    }
}