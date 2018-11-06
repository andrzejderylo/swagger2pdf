using System;
using System.IO;
using Newtonsoft.Json;
using Swagger2Pdf.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using Swagger2Pdf.Filters;
using Swagger2Pdf.Model.Properties;
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
#if !DEBUG
            Perform(new CommandLineInputParameters
            {
                InputFileName = "https://petstore.swagger.io/v2/swagger.json",
                OutputFileName = "./documentation.pdf",
                WelcomePageImagePath = "./image.png",
                Author = "Author Authorski"
            });
#else
            var commandLineArgsParser = new Parser(ConfigureCommandLineParser);
            commandLineArgsParser.ParseArguments<CommandLineInputParameters>(args)
                .WithParsed(Perform);
#endif
        }

        private static void ConfigureCommandLineParser(ParserSettings obj)
        {
            obj.HelpWriter = Console.Out;
        }

        private static void Perform(CommandLineInputParameters parameters)
        {
            var swaggerJsonString = GetSwaggerJsonString(parameters.InputFileName);
            var swaggerJsonInfo = ParseSwaggerJsonInfo(swaggerJsonString);
            var swaggerPdfModel = PrepareSwaggerPdfModel(parameters, swaggerJsonInfo);
            var swaggerPdfDocumentBuilder = new SwaggerPdfDocumentBuilder();
            swaggerPdfDocumentBuilder.BuildPdf(swaggerPdfModel);
        }

        private static SwaggerPdfDocumentModel PrepareSwaggerPdfModel(CommandLineInputParameters parameters, SwaggerInfo swaggerJsonInfo)
        {
            var docModel = new SwaggerPdfDocumentModel();

            docModel.PdfDocumentPath = parameters.OutputFileName;
            docModel.WelcomePageImage = parameters.WelcomePageImagePath;
            docModel.Title = parameters.Title ?? swaggerJsonInfo.Info.Title;
            docModel.Version = parameters.Version ?? swaggerJsonInfo.Info.Version;
            docModel.Author = parameters.Author ?? "";
            docModel.DocumentDate = DateTime.Now;
            docModel.DocumentationEntries = PrepareDocumentationEntries(parameters.IncludeOnlyEndpoints, swaggerJsonInfo);
            docModel.AuthorizationInfos = PrepareAuthorizationInfos(swaggerJsonInfo);

            return docModel;
        }

        private static Dictionary<string, AuthorizationInfo> PrepareAuthorizationInfos(SwaggerInfo swaggerJsonInfo)
        {
            return swaggerJsonInfo.SecurityDefinitions.ToDictionary(x => x.Key, x => x.Value.CreateAuthorizationInfo());
        }

        private static List<EndpointInfo> PrepareDocumentationEntries(IEnumerable<string> includeOnlyEndpoints, SwaggerInfo swaggerJsonInfo)
        {
            var endpointsToInclude = includeOnlyEndpoints?.ToList() ?? new List<string>();
            endpointsToInclude.Select(EndpointFilterFactory.CreateEndpointFilter).ToList();
            

            return swaggerJsonInfo.Paths.SelectMany(path => path.Value.Select(httpMethod =>
                 new EndpointInfo
                 {
                     EndpointPath = path.Key,
                     HttpMethod = httpMethod.Key,
                     Deprecated = httpMethod.Value.Deprecated,
                     Summary = httpMethod.Value.Summary,
                     UrlParameters = httpMethod.Value.Parameters?.Where(x => x.In == "query").Select(BuildParameter)
                         .ToList(),
                     BodyParameters = httpMethod.Value.Parameters?.Where(x => x.In == "body").Select(BuildParameter)
                         .ToList(),
                     FormDataParameters = httpMethod.Value.Parameters?.Where(x => x.In == "formData")
                         .Select(BuildParameter).ToList(),
                     PathParameters = httpMethod.Value.Parameters?.Where(x => x.In == "path").Select(BuildParameter)
                         .ToList(),
                     Responses = httpMethod.Value.Responses?.Select(BuildResponse).ToList(),
                 })).ToList();
        }

        private static SwaggerInfo ParseSwaggerJsonInfo(string jsonString)
        {
            JsonConvert.DeserializeObject<SwaggerInfoDefinitions>(jsonString, new Model.Converters.PropertyBaseConverter());

            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.ReferenceResolverProvider = () => new ReferenceResolver();
                return settings;
            };

            return JsonConvert.DeserializeObject<SwaggerInfo>(jsonString, new Model.Converters.PropertyBaseConverter(), new Model.Converters.SecurityDefinitionConverter());
        }

        private static string GetSwaggerJsonString(string inputFileName)
        {
            if (inputFileName.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                return GetRemoteSwaggerJsonString(new Uri(inputFileName));
            }

            return GetLocalSwaggerJson(inputFileName);
        }

        private static string GetLocalSwaggerJson(string inputFileName)
        {
            var swaggerJsonFileInfo = new FileInfo(inputFileName);
            if (!swaggerJsonFileInfo.Exists)
            {
                throw new ArgumentException($"Swagger json does not exist: {inputFileName}");
            }

            return File.ReadAllText(swaggerJsonFileInfo.FullName);
        }

        private static string GetRemoteSwaggerJsonString(Uri swaggerJsonUri)
        {
            using (
                HttpClient client = new HttpClient(new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                }))
            {
                var task = client.GetAsync(swaggerJsonUri);
                Task.WaitAll(task);
                task.Result.EnsureSuccessStatusCode();
                var readTask = task.Result.Content.ReadAsStringAsync();
                Task.WaitAll(readTask);
                return readTask.Result;
            }
        }

        private static PdfGenerator.Model.Parameter BuildParameter(Parameter parameter) =>
            new PdfGenerator.Model.Parameter
            {
                Name = parameter.Name,
                IsRequired = parameter.ParameterRequired,
                Schema = parameter.Schema?.CreateSchema() ?? parameter.Items?.CreateSchema(),
                Description = parameter.Description,
                Type = parameter.Type,
            };

        private static PdfGenerator.Model.Response BuildResponse(KeyValuePair<string, Response> x) =>
            new PdfGenerator.Model.Response
            {
                Code = x.Key,
                Description = x.Value.Description,
                Schema = x.Value.Schema?.CreateSchema()
            };
    }
}
