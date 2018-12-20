namespace Swagger2Pdf.PdfModel.Model.AuthorizationInformation
{
    public class HttpAuthorizationInfo : AuthorizationInfo
    {
        public string Scheme { get; set; }

        public override void WriteAuthorizationInfo(IAuthorizationWriter writer)
        {
            writer.AppendText($"Authorization type: {Type}");
            writer.AddLineBreak();
            writer.AppendText($"Authorization scheme: {Scheme}");
            writer.AddLineBreak();
        }
    }
}