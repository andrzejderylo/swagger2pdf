using System;
using System.IO;
using MigraDoc.DocumentObjectModel;
using Newtonsoft.Json;
using Swagger2Pdf.Model;
using System.Collections.Generic;
using System.Linq;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

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
                PdfDocBuilder builder = new PdfDocBuilder();

                SwaggerPdfDocumentModel docModel = new SwaggerPdfDocumentModel();
                docModel.DocumentationEntries = obj.Paths.SelectMany(path => path.Value.Select(httpMethod =>
                    new EndpointInfo
                    {
                        EndpointPath = path.Key,
                        HttpMethod = httpMethod.Key,
                        UrlParameters = httpMethod.Value.Parameters?.Where(x => x.In == "query").Select(x => new Parameter
                        {
                            Name = x.Name,
                            IsRequired = x.ParameterRequired,
                            Type = x.Type,
                            Schema = x.Schema
                        }).ToList(),
                        BodyParameters = httpMethod.Value.Parameters?.Where(x => x.In == "body").Select(x => new Parameter
                        {
                            Name = x.Name,
                            IsRequired = x.ParameterRequired,
                            Type = x.Type,
                            Schema = x.Schema
                        }).ToList(),
                        Responses = httpMethod.Value.Responses?.Select(x => new Response
                        {
                            Code = x.Key,
                            Description = x.Value.Description,
                            Schema = x.Value.Schema
                        }).ToList()
                    })).ToList();

                builder.BuildPdf(docModel);
            }
        }
    }


    public class SwaggerPdfDocumentModel
    {
        public List<EndpointInfo> DocumentationEntries { get; set; }
    }

    public class EndpointInfo
    {
        public string EndpointPath { get; set; }
        public List<Parameter> UrlParameters { get; set; }
        public List<Parameter> BodyParameters { get; set; }
        public List<Response> Responses { get; set; }
        public string HttpMethod { get; set; }
    }

    public class Response
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public object Schema { get; set; }
    }

    public class Parameter
    {
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public object Schema { get; set; }
    }

    public class PdfDocBuilder
    {
        public void BuildPdf(SwaggerPdfDocumentModel info)
        {
            Document document = new Document();
            DrawInfo(document, info);
            DrawPaths(document, info);
            PdfDocumentRenderer renderer = new PdfDocumentRenderer();
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save("./documentation.pdf");
        }

        private void DrawInfo(Document doc, SwaggerPdfDocumentModel info)
        {

        }

        private void DrawPaths(Document doc, SwaggerPdfDocumentModel info)
        {
            foreach (var docEntry in info.DocumentationEntries)
            {
                var pathSection = doc.AddSection();

                DrawHttpEndpointTable(docEntry, pathSection);
                pathSection.AddParagraph();
                DrawUrlParameters(docEntry, pathSection);
                pathSection.AddParagraph();
                DrawBodyParameters(docEntry, pathSection);
                pathSection.AddParagraph();
                DrawResponses(docEntry, pathSection);
            }
        }

        private void DrawResponses(EndpointInfo docEntry, Section pathSection)
        {
            if (docEntry.Responses != null)
            {
                pathSection.AddParagraph("Response body");
                var table = pathSection.AddTable();
                table.Borders = new Borders();
                table.Borders.Color = BlackColor;
                table.AddColumn(Unit.FromCentimeter(18));

                foreach (var response in docEntry.Responses)
                {
                    var row = table.AddRow();
                    row[0].AddParagraph(SerializeObject(response.Schema));
                }
            }

        }

        private void DrawHttpEndpointTable(EndpointInfo docEntry, Section pathSection)
        {
            var table = pathSection.AddTable();
            table.Borders = new Borders();
            table.Borders.Color = BlackColor;
            table.AddColumn(Unit.FromCentimeter(3));
            table.AddColumn(Unit.FromCentimeter(10));
            var row = table.AddRow();
            row[0].AddParagraph("Http method");
            row[1].AddParagraph("Endpoint path");

            row = table.AddRow();
            row[0].AddParagraph(docEntry.HttpMethod);
            row[1].AddParagraph(docEntry.EndpointPath);
        }

        private void DrawBodyParameters(EndpointInfo docEntry, Section pathSection)
        {
            if (docEntry.BodyParameters != null && docEntry.BodyParameters.Any())
            {
                pathSection.AddParagraph("Body parameters");
                var table = pathSection.AddTable();
                table.Borders = new Borders();
                table.Borders.Color = BlackColor;
                table.AddColumn(Unit.FromCentimeter(18));

                foreach (var bodyParameter in docEntry.BodyParameters)
                {
                    var row = table.AddRow();
                    var paragraph = row[0].AddParagraph(SerializeObject(bodyParameter.Schema));
                }
            }
        }

        private void DrawUrlParameters(EndpointInfo docEntry, Section pathSection)
        {
            pathSection.AddParagraph("Query parameters");
            if (docEntry.UrlParameters == null)
            {
                pathSection.AddParagraph("No query parameters.");
            }
            else
            {
                var table = pathSection.AddTable();
                table.Borders = new Borders();
                table.Borders.Color = BlackColor;
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));

                var row = table.AddRow();
                row[0].AddParagraph("Name");
                row[1].AddParagraph("Type");
                row[2].AddParagraph("Description");

                foreach (var queryParameter in docEntry.UrlParameters)
                {
                    row = table.AddRow();
                    row[0].AddParagraph(queryParameter.Name ?? "");
                    row[1].AddParagraph(queryParameter.Type ?? "");
                    row[1].AddParagraph(queryParameter.Description ?? "");
                }
            }
        }

        private static readonly Color BlackColor = Color.FromCmyk(100, 100, 100, 100);

        private readonly Func<JsonSerializerSettings> SerializerSettingsFactory = () =>
        {
            return new JsonSerializerSettings
            {
                //ReferenceResolverProvider = () => new ReferenceResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
        };

        private string SerializeObject(object obj)
        {
            using (TextWriter sw = new StringWriter())
            {
                using (JsonTextWriter jw = new JsonTextWriter(sw))
                {
                    jw.Indentation = 1;
                    jw.IndentChar = '\t';                    
                    jw.Formatting = Formatting.Indented;
                    JsonSerializer serializer = JsonSerializer.Create(SerializerSettingsFactory());
                    serializer.Serialize(jw, obj);
                    return sw.ToString();
                }
            }
        }

    }
}
