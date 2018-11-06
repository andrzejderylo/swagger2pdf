namespace Swagger2Pdf.Filters
{
    public sealed class EndpointFilter
    {
        public EndpointFilter(string httpMethod, string httpEndpoint)
        {
            HttpMethod = httpMethod;
            HttpEndpoint = httpEndpoint;
        }

        public string HttpMethod { get; }
        public string HttpEndpoint { get; }

        public bool MatchEndpoint(string httpMethod, string httpEndpoint)
        {
            if (HttpMethod == null) return httpEndpoint == HttpMethod;

            return HttpMethod == httpMethod && httpEndpoint == HttpEndpoint;
        }
    }

}
