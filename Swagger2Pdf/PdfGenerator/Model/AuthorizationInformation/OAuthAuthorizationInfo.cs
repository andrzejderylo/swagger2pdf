using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;

namespace Swagger2Pdf.PdfGenerator.Model.AuthorizationInformation
{
    public class OAuthAuthorizationInfo : AuthorizationInfo
    {
        public string AuthorizationUrl { get; set; }
        public string Flow { get; set; }
        public Dictionary<string, string> Scopes { get; set; }

        public override void WriteAuthorizationInfo(Paragraph paragraph)
        {
            paragraph.AddText($"Authorization type: {Type}");
            paragraph.AddLineBreak();
            paragraph.AddText($"Authorization url: {AuthorizationUrl}");
            paragraph.AddLineBreak();
            paragraph.AddText($"Authorization flow: {Flow}");
            paragraph.AddLineBreak();
            var scopesTable = paragraph.Section.AddBorderedTable();
            scopesTable.AddColumn(Unit.FromCentimeter(6));
            scopesTable.AddColumn(Unit.FromCentimeter(6));
            var row = scopesTable.AddRow();
            row[0].AddParagraph("Scope key");
            row[1].AddParagraph("Description");
            foreach (var scope in Scopes)
            {
                row = scopesTable.AddRow();
                row[0].AddParagraph(scope.Key ?? "");
                row[1].AddParagraph(scope.Value ?? "");
            }
        }
    }
}
