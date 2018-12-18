using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public class HtmlDocumentBuilder
    {   
        private readonly StringBuilder _htmlStringBuilder = new StringBuilder();
        private readonly List<HtmlElement> _bodyElements = new List<HtmlElement>();

        public void Init(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append("<!DOCTYPE html><html><head><meta charset=\"UTF-8\"><style>");
            htmlStringBuilder.Append(Properties.Resources.github_markdown);
            htmlStringBuilder.Append("</style></head><body class=\"markdown-body\">");
        }

        private void End(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append("</body></html>");
        }

        public void AddPageBreak()
        {
            Div().SetStyle("page-break-after", "always");
        }

        public void WriteDocument()
        {
            Init(_htmlStringBuilder);

            foreach (var bodyElement in _bodyElements)
            {
                bodyElement.WriteElement(_htmlStringBuilder);
            }

            End(_htmlStringBuilder);
        }

        public string GetDocumentString()
        {
            if(_htmlStringBuilder.Length == 0)
                WriteDocument();

            return _htmlStringBuilder.ToString();
        } 

        public HtmlElement Div() => AddElement();
        public HtmlElement H1() => AddElement();
        public HtmlElement H2() => AddElement();
        public HtmlElement H3() => AddElement();
        public HtmlElement H4() => AddElement();
        public HtmlElement H5() => AddElement();
        public HtmlElement H6() => AddElement();
        public HtmlElement A() => AddElement();
        public HtmlElement Img() => AddElement();
        public HtmlElement P() => AddElement();
        public HtmlElement Pre() => AddElement();
        public HtmlElement Code() => AddElement();

        public TableElement Table()
        {
            var table = new TableElement();
            _bodyElements.Add(table);
            return table;
        }

        public HtmlElement Ul()
        {
            var ul = new UnorderedHtmlList();
            _bodyElements.Add(ul);
            return ul;
        }

        public HtmlElement Ol()
        {
            var ul = new OrderedHtmlList();
            _bodyElements.Add(ul);
            return ul;
        }

        public HtmlElement AddElement([CallerMemberName] string elementName = "div")
        {
            var element = new HtmlElement(elementName);
            _bodyElements.Add(element);
            return element;
        }

        public void AddCustomPage(StringBuilder getStringBuilder)
        {
            Div().AddChildElement(new TextContent(getStringBuilder.ToString()));
        }
    }
}
