using System.IO;
using Newtonsoft.Json;
using Swagger2Pdf.Model;
using System.Collections.Generic;
using System.Linq;
using Swagger2Pdf.PdfGenerator;
using Swagger2Pdf.PdfGenerator.Model;
using Parameter = Swagger2Pdf.Model.Parameter;
using Response = Swagger2Pdf.Model.Response;

namespace Swagger2Pdf
{
    public class Program
    {
        static void Main(string[] args)
        {
            FileInfo swaggerFile = new FileInfo("./swagger.json");
            if (swaggerFile.Exists)
            {
                var jsonString = File.ReadAllText(swaggerFile.FullName);

                JsonConvert.DeserializeObject<SwaggerInfoDefinitions>(jsonString, new Model.Converters.PropertyBaseConverter());

                JsonConvert.DefaultSettings = () =>
                {
                    var settings = new JsonSerializerSettings();
                    settings.ReferenceResolverProvider = () => new ReferenceResolver();
                    return settings;
                };

                var obj = JsonConvert.DeserializeObject<SwaggerInfo>(jsonString, new Model.Converters.PropertyBaseConverter());
                SwaggerPdfDocumentBuilder builder = new SwaggerPdfDocumentBuilder();

                SwaggerPdfDocumentModel docModel = new SwaggerPdfDocumentModel();
                docModel.PdfDocumentPath = "./documentation.pdf";

                docModel.DocumentationEntries = obj.Paths.SelectMany(path => path.Value.Select( httpMethod =>
                    new EndpointInfo
                    {
                        EndpointPath = path.Key,
                        HttpMethod = httpMethod.Key,
                        UrlParameters = httpMethod.Value.Parameters?.Where(x => x.In == "query").Select(BuildParameter).ToList(),
                        BodyParameters = httpMethod.Value.Parameters?.Where(x => x.In == "body").Select(BuildParameter).ToList(),
                        Responses = httpMethod.Value.Responses?.Select(BuildResponse).ToList()
                    })).ToList();

                builder.BuildPdf(docModel);
            }
        }

        private static PdfGenerator.Model.Parameter BuildParameter(Parameter parameter) =>
            new PdfGenerator.Model.Parameter
            {
                Name = parameter.Name,
                IsRequired = parameter.ParameterRequired,
                Schema = parameter.Schema?.CreateSchema()
            };

        private static PdfGenerator.Model.Response BuildResponse(KeyValuePair<string, Response> x) =>
            new PdfGenerator.Model.Response
            {
                Code = x.Key,
                Description = x.Value.Description,
                Schema = x.Value.Schema.CreateSchema()
            };
    }
}
