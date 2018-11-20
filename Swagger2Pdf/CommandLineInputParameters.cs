using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Swagger2Pdf
{
    public class CommandLineInputParameters
    {
        [Option('i', "input", Required = true, HelpText = "Input swagger.json file name")]
        public string InputFileName { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output pdf file name")]
        public string OutputFileName { get; set; }

        [Option('f', "filter", Required = false, HelpText = "Prints only specified endpoints. Wildcard (*) supported.")]
        public IEnumerable<string> EndpointFilters { get; set; }

        [Option('p', "picture", Required = false, HelpText = "First page company logo picture. Maximum recommended width is 600px")]
        public string WelcomePageImagePath { get; set; }

        [Option('a', "Author", Required = false, HelpText = "Includes documentation author")]
        public string Author { get; set; }

        [Option('t', "title", Required = false, HelpText = "Overrides title obtained from swagger.json")]
        public string Title { get; set; }

        [Option('v', "version", Required = false, HelpText = "Overrides version obtained from swagger.json")]
        public string Version { get; set; }

        [Usage(ApplicationAlias = "Swagger2Pdf.exe")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Normal scenario", new CommandLineInputParameters { InputFileName = "https://petstore.swagger.io/v2/swagger.json", OutputFileName = "./petstore.pdf"});
                yield return new Example("Using local swagger.json file", new CommandLineInputParameters { InputFileName = "./swagger.json", OutputFileName = "./petstore.pdf"});
                yield return new Example("Include company logo", new CommandLineInputParameters { InputFileName = "https://petstore.swagger.io/v2/swagger.json", OutputFileName = "./petstore.pdf", WelcomePageImagePath = "./image.png"});
                yield return new Example("Filtering endpoints", new CommandLineInputParameters { InputFileName = "https://petstore.swagger.io/v2/swagger.json", OutputFileName = "./petstore.pdf", EndpointFilters = new List<string>
                {
                    "/pet",
                    "GET:/store/inventory"
                }});
                yield return new Example("Filtering endpoints with wildcard", new CommandLineInputParameters { InputFileName = "https://petstore.swagger.io/v2/swagger.json", OutputFileName = "./petstore.pdf", EndpointFilters = new List<string>
                {
                    "GET:/pet*",
                    "/store/*",
                }});
            }
        }

    }
}
