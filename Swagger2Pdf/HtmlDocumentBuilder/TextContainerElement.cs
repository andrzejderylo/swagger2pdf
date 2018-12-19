using System;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public class TextContainerElement : HtmlElement
    {
        private HtmlElement _lastElement;

        public TextContainerElement(string elementName) : base(elementName)
        {
            _lastElement = this;
        }

        public TextContainerElement Emphasize() => AddNestedElement("em");

        public TextContainerElement Bold() => AddNestedElement("strong");

        public TextContainerElement Italic() => AddNestedElement("i");

        public TextContainerElement Deleted() => AddNestedElement("del");

        public TextContainerElement Small() => AddNestedElement("small");

        public TextContainerElement Mark() => AddNestedElement("mark");

        public TextContainerElement Subscript() => AddNestedElement("sub");

        public TextContainerElement Superscript() => AddNestedElement("sup");

        public TextContainerElement Underline() => AddNestedElement("ins");

        public TextContainerElement SetText(string text)
        {
            _lastElement.AddChildElement(new TextContent(text));
            _lastElement = this;
            return this;
        }

        public TextContainerElement HorizontalCenter()
        {
            SetStyle("text-align", "center");
            return this;
        }

        public TextContainerElement Left()
        {

            SetStyle("text-align", "left");
            return this;
        }

        public TextContainerElement Right()
        {

            SetStyle("text-align", "right");
            return this;
        }

        public TextContainerElement VerticalCenter()
        {
            SetStyle("vertical-align", "middle");
            return this;
        }

        public TextContainerElement Top()
        {
            SetStyle("vertical-align", "top");
            return this;
        }

        public TextContainerElement Bottom()
        {
            SetStyle("vertical-align", "bottom");
            return this;
        }

        public TextContainerElement SetColor(string color)
        {
            SetStyle("font-color", color);
            return this;
        }

        private TextContainerElement AddNestedElement(string tagName)
        {
            HtmlElement element = new HtmlElement(tagName);
            _lastElement.AddChildElement(element);
            _lastElement = element;
            return this;
        }
    }
}