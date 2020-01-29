namespace Dfe.Spi.Translation.FunctionApp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Http.Server.Definitions;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
    using Dfe.Spi.Translation.Application.Models.Processors;
    using Dfe.Spi.Translation.Domain.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;

    /// <summary>
    /// Entry class for the <c>get-enumeration-mappings</c> function.
    /// </summary>
    public class GetEnumerationMappings
    {
        private readonly IGetEnumerationMappingsProcessor getEnumerationMappingsProcessor;
        private readonly IHttpErrorBodyResultProvider httpErrorBodyResultProvider;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="GetEnumerationMappings" /> class.
        /// </summary>
        /// <param name="getEnumerationMappingsProcessor">
        /// An instance of type
        /// <see cref="IGetEnumerationMappingsProcessor" />.
        /// </param>
        /// <param name="httpErrorBodyResultProvider">
        /// An instance of type <see cref="IHttpErrorBodyResultProvider" />.
        /// </param>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        public GetEnumerationMappings(
            IGetEnumerationMappingsProcessor getEnumerationMappingsProcessor,
            IHttpErrorBodyResultProvider httpErrorBodyResultProvider,
            ILoggerWrapper loggerWrapper)
        {
            this.getEnumerationMappingsProcessor = getEnumerationMappingsProcessor;
            this.httpErrorBodyResultProvider = httpErrorBodyResultProvider;
            this.loggerWrapper = loggerWrapper;
        }

        /// <summary>
        /// Entry method for the <c>get-enumeration-mappings</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <param name="name">
        /// The name of the enumeration.
        /// </param>
        /// <param name="adapter">
        /// The name of the adapter.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        [FunctionName("get-enumeration-mappings")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "{name}/{adapter}")]
            HttpRequest httpRequest,
            string name,
            string adapter,
            CancellationToken cancellationToken)
        {
            IActionResult toReturn = null;

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;
            this.loggerWrapper.SetContext(headerDictionary);

            GetEnumerationMappingsRequest getEnumerationMappingsRequest =
                new GetEnumerationMappingsRequest()
                {
                    EnumerationsKey = new EnumerationKey()
                    {
                        Adapter = adapter,
                        Name = name,
                    },
                };

            toReturn = await this.ProcessWellFormedRequestAsync(
                getEnumerationMappingsRequest,
                cancellationToken)
                .ConfigureAwait(false);

            return toReturn;
        }

        private async Task<IActionResult> ProcessWellFormedRequestAsync(
            GetEnumerationMappingsRequest getEnumerationMappingsRequest,
            CancellationToken cancellationToken)
        {
            IActionResult toReturn = null;

            this.loggerWrapper.Debug(
                $"Invoking {nameof(IGetEnumerationMappingsProcessor)} with " +
                $"{getEnumerationMappingsRequest}...");

            GetEnumerationMappingsResponse getEnumerationMappingsResponse =
                await this.getEnumerationMappingsProcessor.GetEnumerationMappingsAsync(
                    getEnumerationMappingsRequest,
                    cancellationToken)
                    .ConfigureAwait(false);

            this.loggerWrapper.Info(
                $"{nameof(IGetEnumerationMappingsProcessor)} invoked with " +
                $"success: {getEnumerationMappingsResponse}.");

            Dictionary<string, string[]> mappings =
                getEnumerationMappingsResponse.MappingsResult.Mappings;

            if (mappings.Any())
            {
                this.loggerWrapper.Debug(
                    $"Looks like we got some results: " +
                    $"{getEnumerationMappingsResponse}. Returning as JSON.");

                toReturn = new JsonResult(getEnumerationMappingsResponse);
            }
            else
            {
                EnumerationKey enumerationsKey =
                    getEnumerationMappingsRequest.EnumerationsKey;

                this.loggerWrapper.Info(
                    $"No results found for {enumerationsKey}!");

                string name = enumerationsKey.Name;
                string adapter = enumerationsKey.Adapter;

                toReturn = this.httpErrorBodyResultProvider.GetHttpErrorBodyResult(
                    HttpStatusCode.NotFound,
                    1,
                    name,
                    adapter);
            }

            return toReturn;
        }
    }
}