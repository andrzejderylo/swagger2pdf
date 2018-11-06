using System;
using MigraDoc.DocumentObjectModel;

namespace Swagger2Pdf.PdfGenerator.Model.AuthorizationInformation
{
    public class ParameterAuthorizationInfo : AuthorizationInfo
    {
        public string Name { get; set; }
        public string In { get; set; }

        public override void WriteAuthorizationInfo(Paragraph paragraph)
        {
            paragraph.AddText($"Authorization type: {Type}");
            paragraph.AddLineBreak();
            paragraph.AddText($"Authorization parameter name: {Name}");
            paragraph.AddLineBreak();
            paragraph.AddText($"Authorization parameter request location: {In}");
        }
    }
}