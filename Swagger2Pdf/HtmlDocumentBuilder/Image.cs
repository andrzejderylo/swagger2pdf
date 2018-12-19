using System.Text;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public sealed class Image : HtmlElement
    {
        public Image() : base("img")
        {
        }

        protected override void WriteStartTag(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append("<");
            htmlStringBuilder.Append(ElementName);
        }

        protected override void WriteEndTag(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append("/>");
        }

        public Image Src(string source)
        {
            SetAttribute("src", source);
            return this;
        }
    }
}