namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public sealed class OrderedHtmlList : HtmlElement
    {
        public OrderedHtmlList() : base("ol")
        {
        }

        public override HtmlElement AddChildElement(HtmlElement content)
        {
            return base.AddChildElement(new HtmlElement("li").AddChildElement(content));
        }
    }
}