using System;
using System.Reflection;
using CommandLine;
using log4net;
using log4net.Config;
using Swagger2Pdf.HtmlDocumentBuilder;
using Swagger2Pdf.PdfGenerator;

namespace Swagger2Pdf
{
    public static class Program
    {
        public static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        private static void Main(string[] args)
        {
            ConfigureLogger();
            var commandLineArgsParser = new Parser(ConfigureCommandLineParser);
            commandLineArgsParser.ParseArguments<CommandLineInputParameters>(args)
                .WithParsed(Perform);
        }

        private static void ConfigureLogger()
        {
            var loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            BasicConfigurator.Configure(loggerRepository);
        }

        private static void ConfigureCommandLineParser(ParserSettings obj)
        {
            obj.HelpWriter = Console.Out;
        }

        private static void Perform(CommandLineInputParameters parameters)
        {
            try
            {
                Logger.Info("Processing started");
                var swaggerPdfDocumentProvider = new SwaggerPdfDocumentModelProvider();
                var swaggerPdfModel = swaggerPdfDocumentProvider.PrepareSwaggerPdfModel(parameters);
                PdfBuilderBase swaggerPdfDocumentBuilder = new HtmlPdfBuilder();
                swaggerPdfDocumentBuilder.BuildPdf(swaggerPdfModel);
                Logger.Info("Processing successful");
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Logger.Fatal("Unexpected exception occured", e);
                Environment.Exit(1);
            }
        }
    }
}
