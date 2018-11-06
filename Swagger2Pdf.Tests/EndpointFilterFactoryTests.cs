using System;
using FluentAssertions;
using NUnit.Framework;
using Swagger2Pdf.Filters;

namespace Swagger2Pdf.Tests
{
    [TestFixture]
    public class EndpointFilterFactoryTests
    {
        [TestCase("POST:/pet")]
        [TestCase("PUT:/pet")]
        [TestCase(":/pet")]
        [TestCase("/pet")]
        public void EndpointFilterFactory_ShouldParseEndpoints(string endpoint)
        {
            // Act
            Assert.DoesNotThrow(() => EndpointFilterFactory.CreateEndpointFilter(endpoint));
        }

        [TestCase("POST:")]
        [TestCase("POST:POST:")]
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
            endpointFilter.Should().BeEquivalentTo(new EndpointFilter("POST", "/api/Pets"));
        }
        
        [Test]
        public void EndpointFilterFactory_ShouldParse_ColonAndEndpoint()
        {
            // Arrange

            // Act
            var endpointFilter = EndpointFilterFactory.CreateEndpointFilter(":/api/Pets");

            // Assert
            endpointFilter.Should().BeEquivalentTo(new EndpointFilter(null, "/api/Pets"));
        }

        
        [Test]
        public void EndpointFilterFactory_ShouldParse_EndpointOnly()
        {
            // Arrange

            // Act
            var endpointFilter = EndpointFilterFactory.CreateEndpointFilter("/api/Pets");

            // Assert
            endpointFilter.Should().BeEquivalentTo(new EndpointFilter(null, "/api/Pets"));
        }
    }
}
