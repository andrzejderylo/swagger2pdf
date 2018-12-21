using System.Collections.Generic;
using System.IO;
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
            var assembly = typeof(Program).Assembly;
            using (var stream = assembly.GetManifestResourceStream("Swagger2Pdf.Assets.github-markdown.css"))
            using(var reader = new StreamReader(stream))
            {
                htmlStringBuilder.Append(reader.ReadToEnd());
            }

            
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
        public TextContainerElement P() => AddTextElement();
        public TextContainerElement H1() => AddTextElement();
        public TextContainerElement H2() => AddTextElement();
        public TextContainerElement H3() => AddTextElement();
        public TextContainerElement H4() => AddTextElement();
        public TextContainerElement H5() => AddTextElement();
        public TextContainerElement H6() => AddTextElement();
        public TextContainerElement Pre() => AddTextElement();
        public TextContainerElement Code() => AddTextElement();
        public TextContainerElement Label() => AddTextElement();

        public Link A()
        {
            Link element = HtmlElement.A();
            _bodyElements.Add(element);
            return element;
        }

        public Image Img()
        {
            Image element = HtmlElement.Img();
            _bodyElements.Add(element);
            return element;
        }

        public TableElement Table()
        {
            var table = HtmlElement.Table();
            _bodyElements.Add(table);
            return table;
        }

        public HtmlElement Ul()
        {
            var ul = HtmlElement.Ul();
            _bodyElements.Add(ul);
            return ul;
        }

        public HtmlElement Ol()
        {
            var ul = HtmlElement.Ol();
            _bodyElements.Add(ul);
            return ul;
        }

        public void AddCustomPage(StringBuilder getStringBuilder)
        {
            Div().AddChildElement(new RawContent(getStringBuilder));
        }

        private HtmlElement AddElement([CallerMemberName] string elementName = "div")
        {
            var element = new HtmlElement(elementName);
            _bodyElements.Add(element);
            return element;
        }

        private TextContainerElement AddTextElement([CallerMemberName] string elementName = "label")
        {
            var textElement = new TextContainerElement(elementName);
            _bodyElements.Add(textElement);
            return textElement;
        }
    }
}
