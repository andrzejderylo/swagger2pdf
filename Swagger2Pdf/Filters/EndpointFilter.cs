using System;

namespace Swagger2Pdf.Filters
{
    public sealed class EndpointFilter
    {
        public EndpointFilter(string httpMethod, string httpEndpoint)
        {
            HttpMethod = httpMethod;
            HttpEndpoint = httpEndpoint;
            _httpEndpointParts = httpEndpoint.Split('*');
            _httpEndpointComparator = GetHttpEndpointComparator(httpEndpoint);
        }

        public string HttpMethod { get; }
        public string HttpEndpoint { get; }

        private readonly Func<string, bool> _httpEndpointComparator;
        private readonly string[] _httpEndpointParts;

        public bool MatchEndpoint(string httpMethod, string httpEndpoint)
        {
            if (HttpMethod == null) return _httpEndpointComparator(httpEndpoint);

            return HttpMethod == httpMethod && _httpEndpointComparator(httpEndpoint);
        }

        private Func<string, bool> GetHttpEndpointComparator(string httpEndpoint)
        {
            if (_httpEndpointParts.Length == 2 && !string.IsNullOrEmpty(_httpEndpointParts[0]) && !string.IsNullOrEmpty(_httpEndpointParts[1]))
            {
                return s => s.StartsWith(_httpEndpointParts[0]) && s.EndsWith(_httpEndpointParts[1]);
            }

            if(_httpEndpointParts.Length == 2 && !string.IsNullOrEmpty(_httpEndpointParts[1]))
            {
                return s => s.EndsWith(_httpEndpointParts[1]);
            }

            if (_httpEndpointParts.Length == 2 && !string.IsNullOrEmpty(_httpEndpointParts[0]))
            {
                return s =>
                    s.StartsWith(_httpEndpointParts[0]);
            }

            if (_httpEndpointParts.Length == 1 && !string.IsNullOrEmpty(_httpEndpointParts[0]))
            {
                return s => s == _httpEndpointParts[0];
            }

            throw new ArgumentException($"Unable to determine endpoint wildcard search: {httpEndpoint}");
        }
    }

}