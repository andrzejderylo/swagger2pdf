using System.Collections.Generic;

namespace Swagger2Pdf.PdfModel.Model
{
    public class Endpoint
    {
        public List<Parameter> QueryParameters { get; set; }
        public List<Parameter> BodyParameters { get; set; }
        public List<Parameter> FormDataParameters { get; set; }
        public List<Response> Responses { get; set; }
        public List<Parameter> PathParameters { get; set; }

        public string EndpointPath { get; set; }
        public string HttpMethod { get; set; }
        public bool Deprecated { get; set; }
        public string Summary { get; set; }
    }
}