using Newtonsoft.Json;

namespace Swagger2Pdf.Model
{
    public class Info
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}