using System;
using System.Text;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public sealed class Br : HtmlElement
    {
        public Br() : base("br")
        { 
        }

        protected override void WriteStartTag(StringBuilder htmlStringBuilder)
        {
            htmlStringBuilder.Append("<br>");
        }

        protected override void WriteEndTag(StringBuilder htmlStringBuilder)
        {   
        }

        public override HtmlElement AddChildElement(HtmlElement content)
        {
            throw new NotSupportedException("That operation on <br> element is not supprted");
        }
    }
}