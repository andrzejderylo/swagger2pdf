using System;
using CommandLine;
using Swagger2Pdf.PdfGenerator;

namespace Swagger2Pdf
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var commandLineArgsParser = new Parser(ConfigureCommandLineParser);
            commandLineArgsParser.ParseArguments<CommandLineInputParameters>(args)
                .WithParsed(Perform);
        }

        private static void ConfigureCommandLineParser(ParserSettings obj)
        {
            obj.HelpWriter = Console.Out;
        }

        private static void Perform(CommandLineInputParameters parameters)
        {
            var swaggerPdfDocumentProvider = new SwaggerPdfDocumentModelProvider();
            var swaggerPdfModel = swaggerPdfDocumentProvider.PrepareSwaggerPdfModel(parameters);
            var swaggerPdfDocumentBuilder = new SwaggerPdfDocumentBuilder();
            swaggerPdfDocumentBuilder.BuildPdf(swaggerPdfModel);
        }
    }
}
