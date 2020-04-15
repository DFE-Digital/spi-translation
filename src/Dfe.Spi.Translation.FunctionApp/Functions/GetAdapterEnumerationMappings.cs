using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Dfe.Spi.Common.Http.Server.Definitions;
using Dfe.Spi.Common.Logging.Definitions;
using Dfe.Spi.Translation.Application.Definitions.Processors;
using Dfe.Spi.Translation.Application.Models.Processors;
using Dfe.Spi.Translation.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dfe.Spi.Translation.FunctionApp.Functions
{
    /// <summary>
    /// Entry class for the <c>get-adapter-enumeration-mappings</c> function.
    /// </summary>
    public class GetAdapterEnumerationMappings
    {
        private const string FunctionName = nameof(GetAdapterEnumerationMappings);
        
        private readonly IGetAdapterEnumerationMappingsProcessor getAdapterEnumerationMappingsProcessor;
        private readonly IHttpErrorBodyResultProvider httpErrorBodyResultProvider;
        private readonly IHttpSpiExecutionContextManager httpSpiExecutionContextManager;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="GetEnumerationMappings" /> class.
        /// </summary>
        /// <param name="getAdapterEnumerationMappingsProcessor">
        /// An instance of type
        /// <see cref="IGetEnumerationMappingsProcessor" />.
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
        public GetAdapterEnumerationMappings(
            IGetAdapterEnumerationMappingsProcessor getAdapterEnumerationMappingsProcessor,
            IHttpErrorBodyResultProvider httpErrorBodyResultProvider,
            IHttpSpiExecutionContextManager httpSpiExecutionContextManager,
            ILoggerWrapper loggerWrapper)
        {
            this.getAdapterEnumerationMappingsProcessor = getAdapterEnumerationMappingsProcessor;
            this.httpErrorBodyResultProvider = httpErrorBodyResultProvider;
            this.httpSpiExecutionContextManager = httpSpiExecutionContextManager;
            this.loggerWrapper = loggerWrapper;
        }
        
        /// <summary>
        /// Entry method for the <c>get-adapter-enumeration-mappings</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
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
        [FunctionName(FunctionName)]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "adapters/{adapter}/mappings")]
            HttpRequest httpRequest,
            string adapter,
            CancellationToken cancellationToken)
        {
            IActionResult toReturn = null;

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;
            this.httpSpiExecutionContextManager.SetContext(headerDictionary);

            GetAdapterEnumerationMappingsRequest getAdapterEnumerationMappingsRequest =
                new GetAdapterEnumerationMappingsRequest()
                {
                    AdapterName = adapter,
                };

            toReturn = await this.ProcessWellFormedRequestAsync(
                    getAdapterEnumerationMappingsRequest,
                    cancellationToken)
                .ConfigureAwait(false);

            return toReturn;
        }

        private async Task<IActionResult> ProcessWellFormedRequestAsync(
            GetAdapterEnumerationMappingsRequest getAdapterEnumerationMappingsRequest,
            CancellationToken cancellationToken)
        {
            IActionResult toReturn = null;

            this.loggerWrapper.Debug(
                $"Invoking {nameof(IGetAdapterEnumerationMappingsProcessor)} with " +
                $"{getAdapterEnumerationMappingsRequest}...");

            GetAdapterEnumerationMappingsResponse getAdapterEnumerationMappingsResponse =
                await this.getAdapterEnumerationMappingsProcessor.GetAdapterEnumerationMappingsAsync(
                        getAdapterEnumerationMappingsRequest,
                        cancellationToken)
                    .ConfigureAwait(false);

            this.loggerWrapper.Info(
                $"{nameof(IGetAdapterEnumerationMappingsProcessor)} invoked with " +
                $"success: {getAdapterEnumerationMappingsResponse}.");

            Dictionary<string, MappingsResult> mappings = getAdapterEnumerationMappingsResponse.AdapterMappingsResult?.EnumerationMappings;

            if (mappings != null && mappings.Any())
            {
                this.loggerWrapper.Debug(
                    $"Looks like we got some results: " +
                    $"{getAdapterEnumerationMappingsResponse}. Returning as JSON.");

                toReturn = new JsonResult(mappings);
            }
            else
            {
                this.loggerWrapper.Info(
                    $"No mappings found for adapter {getAdapterEnumerationMappingsRequest.AdapterName}!");

                toReturn = this.httpErrorBodyResultProvider.GetHttpErrorBodyResult(
                    HttpStatusCode.NotFound,
                    1,
                    "all",
                    getAdapterEnumerationMappingsRequest.AdapterName);
            }

            return toReturn;
        }
    }
}