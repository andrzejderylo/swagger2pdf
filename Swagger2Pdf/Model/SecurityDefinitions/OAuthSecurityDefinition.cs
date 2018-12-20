using System.Collections.Generic;
using Newtonsoft.Json;
using Swagger2Pdf.PdfModel.Model;
using Swagger2Pdf.PdfModel.Model.AuthorizationInformation;

namespace Swagger2Pdf.Model.SecurityDefinitions
{
    public class OAuthSecurityDefinition : SecurityDefinition
    {
        [JsonProperty("authorizationUrl")]
        public string AuthorizationUrl { get; set; }

        [JsonProperty("flow")]
        public string Flow { get; set; }

        [JsonProperty("scopes")]
        public Dictionary<string, string> Scopes { get; set; }

        public override AuthorizationInfo CreateAuthorizationInfo()
        {
            return new OAuthAuthorizationInfo
            {
                AuthorizationUrl = AuthorizationUrl,
                Flow = Flow,
                Scopes = new Dictionary<string, string>(Scopes),
                Type = Type
            };
        }
    }
}