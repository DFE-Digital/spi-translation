namespace Dfe.Spi.Translation.FunctionApp.UnitTests.Functions
{
    using Dfe.Spi.Common.Http.Server.Definitions;
    using Dfe.Spi.Common.UnitTesting.Infrastructure;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
    using Dfe.Spi.Translation.FunctionApp.Functions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;

    [TestFixture]
    public class GetEnumerationMappingsTests
    {
        private GetEnumerationMappings getEnumerationMappings;
        private LoggerWrapper loggerWrapper;

        [SetUp]
        public void Arrange()
        {
            Mock<IGetEnumerationMappingsProcessor> mockGetEnumerationMappingsProcessor =
                new Mock<IGetEnumerationMappingsProcessor>();
            Mock<IHttpErrorBodyResultProvider> mockHttpErrorBodyResultProvider =
                new Mock<IHttpErrorBodyResultProvider>();
            Mock<IHttpSpiExecutionContextManager> mockHttpSpiExecutionContextManager =
                new Mock<IHttpSpiExecutionContextManager>();

            IGetEnumerationMappingsProcessor getEnumerationMappingsProcessor =
                mockGetEnumerationMappingsProcessor.Object;
            IHttpErrorBodyResultProvider httpErrorBodyResultProvider =
                mockHttpErrorBodyResultProvider.Object;
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

        private HttpRequest CreateHttpRequest(string bodyStr = null)
        {
            HttpRequest toReturn = new DefaultHttpRequest(
                new DefaultHttpContext());

            if (!string.IsNullOrEmpty(bodyStr))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(bodyStr);

                MemoryStream memoryStream = new MemoryStream(buffer);

                toReturn.Body = memoryStream;
            }

            return toReturn;
        }
    }
}