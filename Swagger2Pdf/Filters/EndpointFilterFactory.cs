using System;
using System.Text.RegularExpressions;

namespace Swagger2Pdf.Filters
{
    public static class EndpointFilterFactory
    {
        private static readonly Regex EndpointFilterRegex = new Regex(@"^[A-Z]*:?\/[a-zA-Z0-9\/]*$");

        public static EndpointFilter CreateEndpointFilter(string endpointFilterString)
        {   
            if (!EndpointFilterRegex.IsMatch(endpointFilterString))
            {
                throw new ArgumentException($"Invalid format of endpoint filter. Must be POST:/api/Pet or :/api/Pet or /api/Pet. Current format: {endpointFilterString}");
            }

            var splittedEndpointFilterString = endpointFilterString.Split(':');
            if (splittedEndpointFilterString.Length == 2)
            {
                var httpMethod = string.IsNullOrEmpty(splittedEndpointFilterString[0]) ? null : splittedEndpointFilterString[0].ToUpper();
                var httpEndpoint = splittedEndpointFilterString[1];

                return new EndpointFilter(httpMethod, httpEndpoint);
            }

            return new EndpointFilter(null, splittedEndpointFilterString[0]);
        }
    }

}
