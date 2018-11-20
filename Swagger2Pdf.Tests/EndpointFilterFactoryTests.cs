using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Swagger2Pdf.Filters;

namespace Swagger2Pdf.Tests
{
    [TestFixture]
    public class EndpointFilterFactoryTests
    {
        [TestCase("POST:/pet")]
        [TestCase("GET:/api/pet/{petId}")]
        [TestCase("PUT:/api/pet/{petId}/{petStatus}")]
        [TestCase("PUT:/pet")]
        [TestCase(":/pet")]
        [TestCase("/pet")]
        [TestCase("/pet*")]
        [TestCase("/p*t")]
        [TestCase("POST:/*")]
        public void EndpointFilterFactory_ShouldParseEndpoints(string endpoint)
        {
            // Act
            Assert.DoesNotThrow(() => EndpointFilterFactory.CreateEndpointFilter(endpoint));
        }

        [TestCase("")]
        [TestCase("GET")]
        [TestCase("POST:")]
        [TestCase("POST:PUT:")]
        [TestCase("POST:*")]
        [TestCase(":api/")]
        [TestCase(":/api:")]
        [TestCase("api")]
        [TestCase("post:/api/pets")]
        public void EndpointFilterFactory_ShouldThrowException_DueToInvalidFilterFormat(string endpointFilterString)
        {
            // Act
            var exception = Assert.Throws<ArgumentException>(() => EndpointFilterFactory.CreateEndpointFilter(endpointFilterString));

            // Assert
            exception.Message.Should().BeEquivalentTo($"Invalid format of endpoint filter. Must be POST:/api/Pet or :/api/Pet or /api/Pet. Current format: {endpointFilterString}");
        }

        [Test]
        public void EndpointFilterFactory_ShouldParse_MethodColonAndEndpoint()
        {
            // Arrange

            // Act
            var endpointFilter = EndpointFilterFactory.CreateEndpointFilter("POST:/api/Pets");

            // Assert
            endpointFilter.Should().BeEquivalentTo(new EndpointFilter("POST", "/api/Pets", "POST:/api/Pets"));
        }

        [Test]
        public void EndpointFilterFactory_ShouldParse_ColonAndEndpoint()
        {
            // Arrange

            // Act
            var endpointFilter = EndpointFilterFactory.CreateEndpointFilter(":/api/Pets");

            // Assert
            endpointFilter.Should().BeEquivalentTo(new EndpointFilter(null, "/api/Pets", ":/api/Pets"));
        }


        [Test]
        public void EndpointFilterFactory_ShouldParse_EndpointOnly()
        {
            // Arrange

            // Act
            var endpointFilter = EndpointFilterFactory.CreateEndpointFilter("/api/Pets");

            // Assert
            endpointFilter.Should().BeEquivalentTo(new EndpointFilter(null, "/api/Pets", "/api/Pets"));
        }

        [Test]
        public void EndpointFilterFactory_Should_CheckStartsWithWildcard()
        {
            // Arrange
            var endpointFilter = EndpointFilterFactory.CreateEndpointFilter("/api/Pets*");
            var endpoints = GetPetstoreEndpoints();

            // Act
            var filteredEndpoints = endpoints.Where(p => endpointFilter.MatchEndpoint(p.HttpMethod, p.EndpointPath)).ToList();

            // Assert
            filteredEndpoints.All(x => x.EndpointPath.StartsWith("/api/Pets")).Should().BeTrue();
        }

        [Test]
        public void EndpointFilterFactory_Should_CheckWildcardInMiddle()
        {
            // Arrange
            var endpointFilter = EndpointFilterFactory.CreateEndpointFilter("/api/P*}");
            var endpoints = GetPetstoreEndpoints();

            // Act
            var filteredEndpoints = endpoints.Where(p => endpointFilter.MatchEndpoint(p.HttpMethod, p.EndpointPath)).ToList();

            // Assert
            filteredEndpoints.All(x => x.EndpointPath.StartsWith("/api/P") && x.EndpointPath.EndsWith("}")).Should().BeTrue();
        }

        [Test]
        public void EndpointFilterFactory_Should_CheckWildcardOnStart()
        {
            // Arrange
            var endpointFilter = EndpointFilterFactory.CreateEndpointFilter("/*}");
            var endpoints = GetPetstoreEndpoints();

            // Act
            var filteredEndpoints = endpoints.Where(p => endpointFilter.MatchEndpoint(p.HttpMethod, p.EndpointPath)).ToList();

            // Assert
            filteredEndpoints.All(x => x.EndpointPath.EndsWith("}")).Should().BeTrue();
        }

        [Test]
        public void EndpointFilterFactory_Should_GetAllEndpoints()
        {
            // Arrange
            var endpointFilter = EndpointFilterFactory.CreateEndpointFilter("/*");
            var endpoints = GetPetstoreEndpoints();

            // Act
            var filteredEndpoints = endpoints.Where(p => endpointFilter.MatchEndpoint(p.HttpMethod, p.EndpointPath)).ToList();

            // Assert
            filteredEndpoints.Should().HaveCount(endpoints.Count);
            filteredEndpoints.Should().BeEquivalentTo(endpoints);
        }

        [Test]
        public void EndpointFilter_Should_GetEndpointsOnlyWithHttpMethodAndWildcardedEndpoint()
        {
            // Arrange
            var endpointFilter = EndpointFilterFactory.CreateEndpointFilter("GET:/api/Pets*");
            List<Endpoint> endpoints = GetPetstoreEndpoints();

            // Act
            var filteredEndpoints = endpoints.Where(x => endpointFilter.MatchEndpoint(x.HttpMethod, x.EndpointPath)).ToList();

            // Assert
            filteredEndpoints.All(x => x.HttpMethod == "GET" && x.HttpMethod.StartsWith("/api/Pets")).Should().BeTrue();
        }

        [Test]
        public void EndpointFilter_Should_GetEndpointsOnlyWithHttpMethodAndWildcardedEndpoint2()
        {
            // Arrange
            var endpointFilter = EndpointFilterFactory.CreateEndpointFilter("POST:/api*}");
            List<Endpoint> endpoints = GetPetstoreEndpoints();

            // Act
            var filteredEndpoints = endpoints.Where(x => endpointFilter.MatchEndpoint(x.HttpMethod, x.EndpointPath)).ToList();

            // Assert
            filteredEndpoints.All(x => x.HttpMethod == "POST" && x.HttpMethod.StartsWith("}")).Should().BeTrue();
        }

        private static List<Endpoint> GetPetstoreEndpoints()
        {
            return new List<Endpoint>
            {
                new Endpoint("POST", "/pet"),
                new Endpoint("PUT", "/pet/findByStatus"),
                new Endpoint("GET", "/pet/findByTags"),
                new Endpoint("GET", "/pet/{petId}"),
                new Endpoint("POST", "/pet/{petId}"),
                new Endpoint("DELETE", "/pet/{petId}"),
                new Endpoint("POST", "/pet/{petId}/uploadImage"),
                new Endpoint("GET", "/store/inventory"), 
                new Endpoint("POST", "/store/order"),
                new Endpoint("GET", "/store/order/{orderId}"),
                new Endpoint("DELETE", "/store/order/{orderId}"),
                new Endpoint("POST", "/user"),
                new Endpoint("POST", "/user/createWithArray"),
                new Endpoint("POST", "/user/createWithList"),
                new Endpoint("GET", "/user/login"),
                new Endpoint("GET", "/user/logout"),
                new Endpoint("GET", "/user/{username}"),
                new Endpoint("PUT", "/user/{username}"),
                new Endpoint("DELETE", "/user/{username}"),
            };
        }

        private class Endpoint
        {
            public Endpoint(string httpMethod, string endpointPath)
            {
                EndpointPath = endpointPath;
                HttpMethod = httpMethod;
            }

            public string EndpointPath { get; }
            public string HttpMethod { get; }
        }

    }
}
