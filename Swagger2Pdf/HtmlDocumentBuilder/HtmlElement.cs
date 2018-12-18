using System;
using System.Collections.Generic;
using System.Text;

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

        public virtual void WriteStartTag(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append("<");
            htmlStringBuilder.Append(ElementName);
            WriteDictionaryAttributes(htmlStringBuilder);
            WriteAttributes(htmlStringBuilder);
            htmlStringBuilder.Append(">");
        }

        public virtual void WriteAttributes(StringBuilder htmlStringBuilder)
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

        public void WriteDictionaryAttributes(StringBuilder htmlStringBuilder)
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

        public virtual void WriteEndTag(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append("</");
            htmlStringBuilder.Append(ElementName);
            htmlStringBuilder.Append(">");
        }

        public virtual void WriteContent(StringBuilder htmlStringBuilder)
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
    }
}