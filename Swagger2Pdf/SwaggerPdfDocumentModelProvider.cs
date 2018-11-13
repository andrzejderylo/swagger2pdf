using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Swagger2Pdf.Filters;
using Swagger2Pdf.Model;
using Swagger2Pdf.Model.Converters;
using Swagger2Pdf.Model.Properties;
using Swagger2Pdf.Model.ReferenceResolver;
using Swagger2Pdf.PdfGenerator.Model;
using Parameter = Swagger2Pdf.Model.Parameter;
using Response = Swagger2Pdf.Model.Response;

namespace Swagger2Pdf
{
    public class SwaggerPdfDocumentModelProvider
    {
        private readonly SwaggerJsonProvider _swaggerJsonProvider;
        private ReferenceResolver _referenceResolver;

        public SwaggerPdfDocumentModelProvider()
        {
            _swaggerJsonProvider = new SwaggerJsonProvider();
        }

        public SwaggerPdfDocumentModel PrepareSwaggerPdfModel(CommandLineInputParameters parameters)
        {
            var docModel = new SwaggerPdfDocumentModel();
            var swaggerJsonString = _swaggerJsonProvider.GetSwaggerJsonString(parameters.InputFileName);

            var swaggerJsonInfo = ParseSwaggerJsonInfo(swaggerJsonString);

            docModel.PdfDocumentPath = parameters.OutputFileName;
            docModel.WelcomePageImage = parameters.WelcomePageImagePath;
            docModel.Title = parameters.Title ?? swaggerJsonInfo.Info.Title;
            docModel.Version = parameters.Version ?? swaggerJsonInfo.Info.Version;
            docModel.Author = parameters.Author ?? "";
            docModel.DocumentDate = DateTime.Now;
            docModel.DocumentationEntries = PrepareDocumentationEntries(parameters.EndpointFilters, swaggerJsonInfo);
            docModel.AuthorizationInfos = PrepareAuthorizationInfos(swaggerJsonInfo);

            return docModel;
        }

        private SwaggerInfo ParseSwaggerJsonInfo(string jsonString)
        {
            _referenceResolver = JsonConvert.DeserializeObject<ReferenceResolver>(jsonString);

            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                if (settings.Converters == null)
                {
                    settings.Converters = new List<JsonConverter>();
                }

                settings.Converters.Add(new PropertyBaseConverter());
                settings.Converters.Add(new SecurityDefinitionConverter());
                return settings;
            };

            var swaggerInfo = JsonConvert.DeserializeObject<SwaggerInfo>(jsonString);
            return swaggerInfo;

        }

        private List<EndpointInfo> PrepareDocumentationEntries(IEnumerable<string> endpointFilters, SwaggerInfo swaggerJsonInfo)
        {
            var endpointFilter = endpointFilters?.Select(EndpointFilterFactory.CreateEndpointFilter).ToList() ?? new List<EndpointFilter>();
            var schemaResolutionContext = _referenceResolver.CreateResolutionContext();

            var swaggerPdfEndpointList = swaggerJsonInfo.Paths.SelectMany(path => path.Value.Select(httpMethod =>
                new EndpointInfo
                {
                    EndpointPath = path.Key,
                    HttpMethod = httpMethod.Key.ToUpper(),
                    Deprecated = httpMethod.Value.Deprecated,
                    Summary = httpMethod.Value.Summary,
                    UrlParameters = httpMethod.Value.Parameters?.Where(x => x.In == "query")
                        .Select(parameter => BuildParameter(parameter, schemaResolutionContext))
                        .ToList(),
                    BodyParameters = httpMethod.Value.Parameters?.Where(x => x.In == "body")
                        .Select(parameter => BuildParameter(parameter, schemaResolutionContext))
                        .ToList(),
                    FormDataParameters = httpMethod.Value.Parameters?.Where(x => x.In == "formData")
                        .Select(parameter => BuildParameter(parameter, schemaResolutionContext))
                        .ToList(),
                    PathParameters = httpMethod.Value.Parameters?.Where(x => x.In == "path")
                        .Select(parameter => BuildParameter(parameter, schemaResolutionContext))
                        .ToList(),
                    Responses = httpMethod.Value.Responses?
                        .Select(response => BuildResponse(response, schemaResolutionContext))
                        .ToList(),
                })).ToList();

            if (!endpointFilter.Any())
            {
                return swaggerPdfEndpointList;
            }

            var swaggerFilteredPdfEndpointList = new List<EndpointInfo>(swaggerPdfEndpointList.Capacity);
            foreach (var f in endpointFilter)
            {
                swaggerFilteredPdfEndpointList.AddRange(swaggerPdfEndpointList.Where(e => f.MatchEndpoint(e.HttpMethod, e.EndpointPath)));
            }

            return swaggerFilteredPdfEndpointList;

        }
        
        private static Dictionary<string, AuthorizationInfo> PrepareAuthorizationInfos(SwaggerInfo swaggerJsonInfo)
        {
            return swaggerJsonInfo.SecurityDefinitions.ToDictionary(x => x.Key, x => x.Value.CreateAuthorizationInfo());
        }

        private static PdfGenerator.Model.Parameter BuildParameter(Parameter parameter, SchemaResolutionContext resolutionContext)
        {
            return new PdfGenerator.Model.Parameter
            {
                Name = parameter.Name,
                IsRequired = parameter.ParameterRequired,
                Schema = parameter.Schema?.ResolveSchema(resolutionContext) ?? parameter.Items?.ResolveSchema(resolutionContext),
                Description = parameter.Description,
                Type = parameter.Type,
            };
        }

        private static PdfGenerator.Model.Response BuildResponse(KeyValuePair<string, Response> responseKvp, SchemaResolutionContext resolutionContext)
        {
            return new PdfGenerator.Model.Response
            {
                Code = responseKvp.Key,
                Description = responseKvp.Value.Description,
                Schema = responseKvp.Value.Schema?.ResolveSchema(resolutionContext)
            };
        }
    }
}