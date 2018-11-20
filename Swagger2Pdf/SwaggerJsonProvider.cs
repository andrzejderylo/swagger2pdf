using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace Swagger2Pdf
{
    public class SwaggerJsonProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        public string GetSwaggerJsonString(string inputFileName)
        {
            Logger.Info($"Getting swagger json file: {inputFileName}");
            if (inputFileName.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                return GetRemoteSwaggerJsonString(new Uri(inputFileName));
            }

            return GetLocalSwaggerJson(inputFileName);
        }

        private static string GetLocalSwaggerJson(string inputFileName)
        {
            Logger.Info("Obtaining swagger.json from local file");
            var swaggerJsonFileInfo = new FileInfo(inputFileName);
            if (!swaggerJsonFileInfo.Exists)
            {
                throw new ArgumentException($"Swagger json does not exist: {inputFileName}");
            }

            return File.ReadAllText(swaggerJsonFileInfo.FullName);
        }

        private static string GetRemoteSwaggerJsonString(Uri swaggerJsonUri)
        {
            Logger.Info("Obtaining swagger.json from remote");
            using (HttpClient client = CreateHttpClient())
            {
                var task = client.GetAsync(swaggerJsonUri);
                Task.WaitAll(task);
                task.Result.EnsureSuccessStatusCode();
                var readTask = task.Result.Content.ReadAsStringAsync();
                Task.WaitAll(readTask);
                return readTask.Result;
            }
        }

        private static HttpClient CreateHttpClient()
        {
            return new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });
        }
    }
}