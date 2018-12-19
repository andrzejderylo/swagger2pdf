using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public sealed class TableElement : HtmlElement
    {
        private readonly List<List<HtmlElement>> _rows = new List<List<HtmlElement>>();
        private readonly List<HtmlElement> _header = new List<HtmlElement>();

        public TableElement() : base("table")
        {
        }

        public TableElement AddRow(params HtmlElement[] cell)
        {
            _rows.Add(cell.ToList());
            return this;
        }

        public TableElement AddColumns(params HtmlElement[] headers)
        {
            _header.AddRange(headers);
            return this;
        }

        protected override void WriteContent(StringBuilder htmlStringBuilder)
        {
            var tableHead = new HtmlElement("thead");
            foreach (var header in _header)
            {
                tableHead.AddChildElement(new HtmlElement("th").AddChildElement(header));
            }
            tableHead.WriteElement(htmlStringBuilder);

            var tableBody = new HtmlElement("tbody");
            foreach (var row in _rows)
            {
                var tableRow = new HtmlElement("tr");
                tableBody.AddChildElement(tableRow);
                foreach (var cell in row)
                {
                    tableRow.AddChildElement(new HtmlElement("td").AddChildElement(cell));
                }
            }
            tableBody.WriteElement(htmlStringBuilder);
        }

        public override HtmlElement AddChildElement(HtmlElement content)
        {
            throw new NotSupportedException("Please use AddRow() or AddColumns() methods");
        }
    }
}