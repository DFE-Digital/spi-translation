namespace Dfe.Spi.Translation.FunctionApp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Http.Server.Definitions;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
    using Dfe.Spi.Translation.Application.Models.Processors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;

    /// <summary>
    /// Entry class for the <c>get-enumeration-values</c> function.
    /// </summary>
    public class GetEnumerationValues
    {
        private readonly IGetEnumerationValuesProcessor getEnumerationsProcessor;
        private readonly IHttpErrorBodyResultProvider httpErrorBodyResultProvider;
        private readonly IHttpSpiExecutionContextManager httpSpiExecutionContextManager;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="GetEnumerationValues" />
        /// class.
        /// </summary>
        /// <param name="getEnumerationsProcessor">
        /// An instance of type <see cref="IGetEnumerationValuesProcessor" />.
        /// </param>
        /// <param name="httpErrorBodyResultProvider">
        /// An instance of type <see cref="IHttpErrorBodyResultProvider" />.
        /// </param>
        /// <param name="httpSpiExecutionContextManager">
        /// An instance of type <see cref="IHttpSpiExecutionContextManager" />.
        /// </param>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        public GetEnumerationValues(
            IGetEnumerationValuesProcessor getEnumerationsProcessor,
            IHttpErrorBodyResultProvider httpErrorBodyResultProvider,
            IHttpSpiExecutionContextManager httpSpiExecutionContextManager,
            ILoggerWrapper loggerWrapper)
        {
            this.getEnumerationsProcessor = getEnumerationsProcessor;
            this.httpErrorBodyResultProvider = httpErrorBodyResultProvider;
            this.httpSpiExecutionContextManager = httpSpiExecutionContextManager;
            this.loggerWrapper = loggerWrapper;
        }

        /// <summary>
        /// Entry method for the <c>get-enumeration-values</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <param name="name">
        /// The name of the enumeration.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        [FunctionName("get-enumerations")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "enumerations/{name}")]
            HttpRequest httpRequest,
            string name,
            CancellationToken cancellationToken)
        {
            IActionResult toReturn = null;

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;
            this.httpSpiExecutionContextManager.SetContext(headerDictionary);

            GetEnumerationValuesRequest getEnumerationValuesRequest =
                new GetEnumerationValuesRequest()
                {
                    Name = name,
                };

            toReturn = await this.ProcessWellFormedRequestAsync(
                getEnumerationValuesRequest,
                cancellationToken)
                .ConfigureAwait(false);

            return toReturn;
        }

        private async Task<IActionResult> ProcessWellFormedRequestAsync(
            GetEnumerationValuesRequest getEnumerationValuesRequest,
            CancellationToken cancellationToken)
        {
            IActionResult toReturn = null;

            this.loggerWrapper.Debug(
                $"Invoking {nameof(IGetEnumerationValuesProcessor)} with " +
                $"{getEnumerationValuesRequest}...");

            GetEnumerationValuesResponse getEnumerationValuesResponse =
                await this.getEnumerationsProcessor.GetEnumerationValuesAsync(
                    getEnumerationValuesRequest,
                    cancellationToken)
                    .ConfigureAwait(false);

            this.loggerWrapper.Info(
                $"{nameof(IGetEnumerationMappingsProcessor)} invoked with " +
                $"success: {getEnumerationValuesResponse}.");

            IEnumerable<string> enumerationValues = getEnumerationValuesResponse
                .EnumerationValuesResult
                .EnumerationValues;

            if (enumerationValues.Any())
            {
                this.loggerWrapper.Debug(
                    $"Looks like we got some results: " +
                    $"{getEnumerationValuesResponse}. Returning as JSON.");

                toReturn = new JsonResult(getEnumerationValuesResponse);
            }
            else
            {
                string name = getEnumerationValuesRequest.Name;

                this.loggerWrapper.Info(
                    $"No results found for enumeration {nameof(name)} " +
                    $"\"{name}\"!");

                toReturn = this.httpErrorBodyResultProvider.GetHttpErrorBodyResult(
                    HttpStatusCode.NotFound,
                    2,
                    name);
            }

            return toReturn;
        }
    }
}