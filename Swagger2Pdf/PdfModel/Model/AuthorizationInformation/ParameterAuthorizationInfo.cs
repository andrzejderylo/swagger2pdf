namespace Swagger2Pdf.PdfModel.Model.AuthorizationInformation
{
    public class ParameterAuthorizationInfo : AuthorizationInfo
    {
        public string Name { get; set; }
        public string In { get; set; }

        public override void WriteAuthorizationInfo(IAuthorizationWriter writer)
        {
            writer.AppendText($"Authorization type: {Type}");
            writer.AddLineBreak();
            writer.AppendText($"Authorization parameter name: {Name}");
            writer.AddLineBreak();
            writer.AppendText($"Authorization parameter request location: {In}");
        }
    }
}