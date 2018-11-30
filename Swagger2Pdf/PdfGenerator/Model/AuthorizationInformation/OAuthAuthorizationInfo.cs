using iText.Layout.Element;
using System.Collections.Generic;
using iText.Layout.Properties;
using Swagger2Pdf.PdfGenerator.Helpers;

namespace Swagger2Pdf.PdfGenerator.Model.AuthorizationInformation
{
    public class OAuthAuthorizationInfo : AuthorizationInfo
    {
        public string AuthorizationUrl { get; set; }
        public string Flow { get; set; }
        public Dictionary<string, string> Scopes { get; set; }

        public override void WriteAuthorizationInfo(Paragraph paragraph)
        {
            paragraph.Add($"Authorization type: {Type}");
            paragraph.AddLineBreak();
            paragraph.Add($"Authorization url: {AuthorizationUrl}");
            paragraph.AddLineBreak();
            paragraph.Add($"Authorization flow: {Flow}");
            paragraph.AddLineBreak();
            var scopesTable = new Table(new float[] {50, 50}).AddStyle(ParagraphHelper.BorderedStyle());
            scopesTable.SetWidth(UnitValue.CreatePercentValue(100));
            
            scopesTable.AddCell("Scope key");
            scopesTable.AddCell("Description");
            foreach (var scope in Scopes)
            {
                scopesTable = scopesTable.StartNewRow();
                scopesTable.AddCell(scope.Key ?? "");
                scopesTable.AddCell(scope.Value ?? "");
            }

            paragraph.Add(scopesTable);
        }
    }
}
