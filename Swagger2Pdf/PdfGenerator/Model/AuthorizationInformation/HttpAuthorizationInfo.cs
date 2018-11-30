using iText.Layout.Element;
using Swagger2Pdf.PdfGenerator.Helpers;

namespace Swagger2Pdf.PdfGenerator.Model.AuthorizationInformation
{
    public class HttpAuthorizationInfo : AuthorizationInfo
    {
        public string Scheme { get; set; }

        public override void WriteAuthorizationInfo(Paragraph paragraph)
        {
            paragraph.Add($"Authorization type: {Type}");
            paragraph.AddLineBreak();
            paragraph.Add($"Authorization scheme: {Scheme}");
            paragraph.AddLineBreak();
        }
    }
}