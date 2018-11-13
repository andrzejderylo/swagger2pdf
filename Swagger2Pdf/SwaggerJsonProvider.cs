using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Swagger2Pdf
{
    public class SwaggerJsonProvider
    {
        public string GetSwaggerJsonString(string inputFileName)
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