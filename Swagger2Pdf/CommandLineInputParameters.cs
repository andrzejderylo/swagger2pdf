using System.Collections.Generic;
using CommandLine;

namespace Swagger2Pdf
{
    public class CommandLineInputParameters
    {
        [Option('i', "input", Required = true, HelpText = "Input swagger.json file name")]
        public string InputFileName { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output pdf file name")]
        public string OutputFileName { get; set; }

        [Option('f', "filter", Required = false, HelpText = "")]
        public List<string> IncludeOnlyEndpoints { get; set; } = new List<string>();

        [Option('i', "image", Required = false, HelpText = "First page company logo image. Maximum recommended width is 600px")]
        public string WelcomePageImagePath { get; set; }

        [Option('a', "Author", Required = false, HelpText = "Overrides author name obtained from swagger.json")]
        public string Author { get; set; }

        [Option('t', "title", Required = false, HelpText = "Overrides title obrainted from swagger.json")]
        public string Title { get; set; }

        [Option('v', "version", Required = false, HelpText = "Overrides version obtained from swagger.json")]
        public string Version { get; set; }
    }
}
