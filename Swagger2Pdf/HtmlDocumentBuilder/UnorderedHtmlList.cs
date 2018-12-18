namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public sealed class UnorderedHtmlList : HtmlElement
    {
        public UnorderedHtmlList() : base("ul")
        {
        }

        public override HtmlElement AddChildElement(HtmlElement content)
        {
            return base.AddChildElement(new HtmlElement("li").AddChildElement(content));
        }
    }
}