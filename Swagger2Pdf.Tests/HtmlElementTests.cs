using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Swagger2Pdf.HtmlDocumentBuilder;

namespace Swagger2Pdf.Tests
{
    [TestFixture]
    public class HtmlElementTests
    {
        [Test]
        public void HtmlElement_ShouldCreate_ProperElement()
        {
            HtmlElement element = new HtmlElement("dIv");
            element.ToString().Should().BeEquivalentTo("<div></div>");
        }

        [Test]
        public void HtmlElement_ShouldCreate_DivWithAttribute()
        {
            HtmlElement element = new HtmlElement("div");
            element.SetAttribute("class", "example-class");
            element.ToString().Should().BeEquivalentTo("<div class=\"example-class\"></div>");
        }

        [Test]
        public void HtmlElement_ShouldCreateDivWithChildElement()
        {
            HtmlElement element = new HtmlElement("div");
            element.AddChildElement(new HtmlElement("p"));
            element.ToString().Should().BeEquivalentTo("<div><p></p></div>");
        }

        [Test]
        public void TextElement_ShouldWriteProperly()
        {
            TextContent text = new TextContent("example textt");
            text.ToString().Should().BeEquivalentTo("example textt");
        }

        [Test]
        public void TableElement_ShouldBeCreatedProperly()
        {
            TableElement element = new TableElement();
            element.AddColumns(new TextContent("column1"), new TextContent("column2"));
            element.AddRow(new TextContent("text1"), new TextContent("text2"));

            element.ToString().Should()
                .BeEquivalentTo(
                    "<table><thead><th>column1</th><th>column2</th></thead><tbody><tr><td>text1</td><td>text2</td></tr></tbody></table>");
        }

        [Test]
        public void OrderedList_ShouldBeCreatedProperly()
        {
            var ol = new OrderedHtmlList();
            ol.AddChildElement(new TextContent("item"));
            ol.ToString().Should().BeEquivalentTo("<ol><li>item</li></ol>");
        }


        [Test]
        public void UnorderedList_ShouldBeCreatedProperly()
        {
            var ul = new UnorderedHtmlList();
            ul.AddChildElement(new TextContent("item"));
            ul.ToString().Should().BeEquivalentTo("<ul><li>item</li></ul>");
        }
    }
}
