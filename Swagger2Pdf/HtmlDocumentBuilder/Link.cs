using System.Text;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public sealed class Link : HtmlElement
    {
        public Link() : base("a")
        {
        }

        protected override void WriteStartTag(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append("<");
            htmlStringBuilder.Append(ElementName);
            WriteAttributes(htmlStringBuilder);
            WriteDictionaryAttributes(htmlStringBuilder);
        }

        protected override void WriteEndTag(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append("/>");
        }

        public Link Href(string source)
        {
            SetAttribute("href", source);
            return this;
        }
    }
}