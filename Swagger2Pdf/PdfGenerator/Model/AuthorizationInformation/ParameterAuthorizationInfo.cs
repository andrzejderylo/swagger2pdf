using iText.Layout.Element;

namespace Swagger2Pdf.PdfGenerator.Model.AuthorizationInformation
{
    public class ParameterAuthorizationInfo : AuthorizationInfo
    {
        public string Name { get; set; }
        public string In { get; set; }

        public override void WriteAuthorizationInfo(Paragraph paragraph)
        {
            paragraph.Add($"Authorization type: {Type}");
            paragraph.AddLineBreak();
            paragraph.Add($"Authorization parameter name: {Name}");
            paragraph.AddLineBreak();
            paragraph.Add($"Authorization parameter request location: {In}");
        }
    }
}