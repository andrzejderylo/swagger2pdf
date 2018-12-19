using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using iText.Html2pdf.Attach.Impl.Tags;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public class HtmlElement
    {
        private readonly List<HtmlElement> _contentElements = new List<HtmlElement>();
        private readonly Dictionary<string, string> _attributes = new Dictionary<string, string>();
        private readonly Dictionary<string, Dictionary<string, string>> _dictionaryAttributes = new Dictionary<string, Dictionary<string, string>>();
        private static readonly HashSet<string> ProhibitedAttributes = new HashSet<string> { "style" };

        public HtmlElement(string elementName)
        {
            ElementName = elementName.ToLower();
        }

        public string ElementName { get; }

        public HtmlElement SetAttribute(string attribute, string value)
        {
            attribute = attribute.ToLower();
            if (ProhibitedAttributes.Contains(attribute)) throw new ArgumentException("Prohibited argument, please use appropriate method to set it.");

            _attributes[attribute] = value.ToLower();
            return this;
        }

        public HtmlElement SetStyle(string attribute, string value)
        {
            if (_dictionaryAttributes.TryGetValue("style", out var styleDict))
            {
                styleDict[attribute.ToLower()] = value.ToLower();
                return this;
            }

            _dictionaryAttributes["style"] = new Dictionary<string, string>
            {
                [attribute.ToLower()] = value.ToLower()
            };

            return this;
        }

        public virtual HtmlElement AddChildElement(HtmlElement content)
        {
            _contentElements.Add(content);
            return this;
        }

        protected virtual void WriteStartTag(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append("<");
            htmlStringBuilder.Append(ElementName);
            WriteDictionaryAttributes(htmlStringBuilder);
            WriteAttributes(htmlStringBuilder);
            htmlStringBuilder.Append(">");
        }

        protected virtual void WriteAttributes(StringBuilder htmlStringBuilder)
        {
            foreach (var attribute in _attributes)
            {
                htmlStringBuilder.Append(" ");
                htmlStringBuilder.Append(attribute.Key);
                htmlStringBuilder.Append("=\"");
                htmlStringBuilder.Append(attribute.Value);
                htmlStringBuilder.Append("\"");
            }
        }

        protected void WriteDictionaryAttributes(StringBuilder htmlStringBuilder)
        {
            foreach (var attribute in _dictionaryAttributes)
            {
                htmlStringBuilder.Append(" ");
                htmlStringBuilder.Append(attribute.Key);
                htmlStringBuilder.Append("=\"");
                foreach (var attributeValue in attribute.Value)
                {
                    htmlStringBuilder.Append(attributeValue.Key);
                    htmlStringBuilder.Append(":");
                    htmlStringBuilder.Append(attributeValue.Value);
                    htmlStringBuilder.Append("; ");
                }

                htmlStringBuilder.Append("\"");
            }
        }

        protected virtual void WriteEndTag(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append("</");
            htmlStringBuilder.Append(ElementName);
            htmlStringBuilder.Append(">");
        }

        protected virtual void WriteContent(StringBuilder htmlStringBuilder)
        {
            foreach (var childElement in _contentElements)
            {
                childElement.WriteElement(htmlStringBuilder);
            }
        }

        public void WriteElement(StringBuilder htmlStringBuilder)
        {
            WriteStartTag(htmlStringBuilder);
            WriteContent(htmlStringBuilder);
            WriteEndTag(htmlStringBuilder);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            WriteElement(sb);
            return sb.ToString();
        }

        public static HtmlElement Div() => AddElement();
        public static TextContainerElement P() => AddTextElement();
        public static TextContainerElement H1() => AddTextElement();
        public static TextContainerElement H2() => AddTextElement();
        public static TextContainerElement H3() => AddTextElement();
        public static TextContainerElement H4() => AddTextElement();
        public static TextContainerElement H5() => AddTextElement();
        public static TextContainerElement H6() => AddTextElement();
        public static TextContainerElement Pre() => AddTextElement();
        public static TextContainerElement Code() => AddTextElement();
        public static TextContainerElement Label() => AddTextElement();
        public static TextContent Text(string text) => new TextContent(text);

        public static Link A() => new Link();
        public static Image Img() => new Image();
        public static TableElement Table() => new TableElement();
        public static UnorderedHtmlList Ul() => new UnorderedHtmlList();
        public static OrderedHtmlList Ol() => new OrderedHtmlList();
        private static HtmlElement AddElement([CallerMemberName] string elementName = "div") => new HtmlElement(elementName);
        private static TextContainerElement AddTextElement([CallerMemberName] string elementName = "label") => new TextContainerElement(elementName);
    }
}