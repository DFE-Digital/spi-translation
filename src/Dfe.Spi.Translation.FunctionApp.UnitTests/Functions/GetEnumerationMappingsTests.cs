namespace Dfe.Spi.Translation.FunctionApp.UnitTests.Functions
{
    using Dfe.Spi.Common.Http.Server;
    using Dfe.Spi.Common.Http.Server.Definitions;
    using Dfe.Spi.Common.UnitTesting.Infrastructure;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
    using Dfe.Spi.Translation.Application.Models.Processors;
    using Dfe.Spi.Translation.Domain.Models;
    using Dfe.Spi.Translation.FunctionApp.Functions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class GetEnumerationMappingsTests
    {
        private GetEnumerationMappings getEnumerationMappings;
        private LoggerWrapper loggerWrapper;
        private Mock<IGetEnumerationMappingsProcessor> mockGetEnumerationMappingsProcessor;
        private Mock<IHttpErrorBodyResultProvider> mockHttpErrorBodyResultProvider;

        [SetUp]
        public void Arrange()
        {
            this.mockGetEnumerationMappingsProcessor =
                new Mock<IGetEnumerationMappingsProcessor>();
            this.mockHttpErrorBodyResultProvider =
                new Mock<IHttpErrorBodyResultProvider>();
            Mock<IHttpSpiExecutionContextManager> mockHttpSpiExecutionContextManager =
                new Mock<IHttpSpiExecutionContextManager>();

            IGetEnumerationMappingsProcessor getEnumerationMappingsProcessor =
                this.mockGetEnumerationMappingsProcessor.Object;
            IHttpErrorBodyResultProvider httpErrorBodyResultProvider =
                this.mockHttpErrorBodyResultProvider.Object;
            IHttpSpiExecutionContextManager httpSpiExecutionContextManager =
                mockHttpSpiExecutionContextManager.Object;
            this.loggerWrapper = new LoggerWrapper();

            this.getEnumerationMappings = new GetEnumerationMappings(
                getEnumerationMappingsProcessor,
                httpErrorBodyResultProvider,
                httpSpiExecutionContextManager,
                this.loggerWrapper);
        }

        [Test]
        public void RunAsync_PostWithoutHttpRequest_ThrowsNullArgumentException()
        {
            // Arrange
            HttpRequest httpRequest = null;
            string name = null;
            string adapter = null;
            CancellationToken cancellationToken = CancellationToken.None;

            AsyncTestDelegate asyncTestDelegate =
                async () =>
                {
                    // Act
                    await this.getEnumerationMappings.RunAsync(
                        httpRequest,
                        name,
                        adapter,
                        cancellationToken);
                };

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(asyncTestDelegate);
        }

        [Test]
        public async Task RunAsync_NoResultsReturnedFromStorage_ReturnsNotFoundStatusCode()
        {
            // Arrange
            GetEnumerationMappingsResponse getEnumerationMappingsResponse =
                new GetEnumerationMappingsResponse()
                {
                    MappingsResult = new MappingsResult()
                    {
                        Mappings = new Dictionary<string, string[]>()
                        {
                            // Empty result set.
                        },
                    },
                };

            this.mockGetEnumerationMappingsProcessor
                .Setup(x => x.GetEnumerationMappingsAsync(It.IsAny<GetEnumerationMappingsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getEnumerationMappingsResponse);

            HttpErrorBodyResult httpErrorBodyResult = new HttpErrorBodyResult(
                HttpStatusCode.NotFound,
                null,
                null);
            int? expectedHttpStatusCode = (int)httpErrorBodyResult.StatusCode;
            int? actualHttpStatusCode = null;

            this.mockHttpErrorBodyResultProvider
                .Setup(x => x.GetHttpErrorBodyResult(It.IsAny<HttpStatusCode>(), It.IsAny<int>(), It.IsAny<string[]>()))
                .Returns(httpErrorBodyResult);

            HttpRequest httpRequest = this.CreateHttpRequest();
            string name = "LearningProvider";
            string adapter = "some-adapter";
            CancellationToken cancellationToken = CancellationToken.None;

            IActionResult actionResult = null;

            // Act
            actionResult = await this.getEnumerationMappings.RunAsync(
                httpRequest,
                name,
                adapter,
                cancellationToken);

            // Assert
            Assert.IsInstanceOf<HttpErrorBodyResult>(actionResult);

            httpErrorBodyResult = actionResult as HttpErrorBodyResult;

            actualHttpStatusCode = httpErrorBodyResult.StatusCode;

            Assert.AreEqual(expectedHttpStatusCode, actualHttpStatusCode);
        }

        [Test]
        public async Task RunAsync_ResultsReturnedFromStorage_ReturnsOkAndResults()
        {
            // Arrange
            GetEnumerationMappingsResponse expectedGetEnumerationMappingsResponse =
                new GetEnumerationMappingsResponse()
                {
                    MappingsResult = new MappingsResult()
                    {
                        Mappings = new Dictionary<string, string[]>()
                        {
                            {
                                "example-mapping",
                                new string[]
                                {
                                    "1",
                                    "2",
                                }
                            }
                        },
                    },
                };
            GetEnumerationMappingsResponse actualGetEnumerationMappingsResponse = null;

            this.mockGetEnumerationMappingsProcessor
                .Setup(x => x.GetEnumerationMappingsAsync(It.IsAny<GetEnumerationMappingsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedGetEnumerationMappingsResponse);

            // Null means "OK" in ASP.NET Core/MVC land.
            int? expectedHttpStatusCode = null;
            int? actualHttpStatusCode = null;

            HttpRequest httpRequest = this.CreateHttpRequest();
            string name = "LearningProvider";
            string adapter = "some-adapter";
            CancellationToken cancellationToken = CancellationToken.None;

            IActionResult actionResult = null;

            JsonResult jsonResult = null;

            // Act
            actionResult = await this.getEnumerationMappings.RunAsync(
                httpRequest,
                name,
                adapter,
                cancellationToken);

            // Assert
            Assert.IsInstanceOf<JsonResult>(actionResult);

            jsonResult = actionResult as JsonResult;

            actualHttpStatusCode = jsonResult.StatusCode;

            Assert.AreEqual(expectedHttpStatusCode, actualHttpStatusCode);

            // Assert the contents too.
            actualGetEnumerationMappingsResponse =
                jsonResult.Value as GetEnumerationMappingsResponse;

            Assert.AreEqual(
                expectedGetEnumerationMappingsResponse,
                actualGetEnumerationMappingsResponse);
        }

        private HttpRequest CreateHttpRequest()
        {
            HttpRequest toReturn = new DefaultHttpRequest(
                new DefaultHttpContext());

            return toReturn;
        }
    }
}