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
        public IEnumerable<string> IncludeOnlyEndpoints { get; set; }

        [Option('p', "picture", Required = false, HelpText = "First page company logo picture. Maximum recommended width is 600px")]
        public string WelcomePageImagePath { get; set; }

        [Option('a', "Author", Required = false, HelpText = "Includes documentation author")]
        public string Author { get; set; }

        [Option('t', "title", Required = false, HelpText = "Overrides title obtained from swagger.json")]
        public string Title { get; set; }

        [Option('v', "version", Required = false, HelpText = "Overrides version obtained from swagger.json")]
        public string Version { get; set; }
    }
}
