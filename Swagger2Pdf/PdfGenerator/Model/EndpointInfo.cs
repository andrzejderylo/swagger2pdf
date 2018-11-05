using System.Collections.Generic;

namespace Swagger2Pdf.PdfGenerator.Model
{
    public class EndpointInfo
    {
        public string EndpointPath { get; set; }
        public List<Parameter> UrlParameters { get; set; }
        public List<Parameter> BodyParameters { get; set; }
        public List<Parameter> FormDataParameters { get; set; }
        public List<Response> Responses { get; set; }
        public string HttpMethod { get; set; }
        public bool Deprecated { get; set; }
        public string Summary { get; set; }
        public List<Parameter> PathParameters { get; internal set; }
    }
}