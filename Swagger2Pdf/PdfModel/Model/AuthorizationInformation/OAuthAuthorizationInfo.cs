using System.Collections.Generic;

namespace Swagger2Pdf.PdfModel.Model.AuthorizationInformation
{
    public class OAuthAuthorizationInfo : AuthorizationInfo
    {
        public string AuthorizationUrl { get; set; }
        public string Flow { get; set; }
        public Dictionary<string, string> Scopes { get; set; }

        public override void WriteAuthorizationInfo(IAuthorizationWriter writer)
        {
            writer.AppendText($"Authorization type: {Type}");
            writer.AddLineBreak();
            writer.AppendText($"Authorization url: {AuthorizationUrl}");
            writer.AddLineBreak();
            writer.AppendText($"Authorization flow: {Flow}");
            writer.AddLineBreak();

            writer.AddTable("Scope key", "Description");
            foreach (var scope in Scopes)
            {
                writer.AddTableRow(scope.Key ?? "", scope.Value ?? "");
            }
        }
    }
}
