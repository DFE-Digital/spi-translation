namespace Dfe.Spi.Translation.FunctionApp.Functions
{
    using System;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
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
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="GetEnumerationMappings" />
        /// class.
        /// </summary>
        /// <param name="getEnumerationMappingsProcessor">
        /// An instance of type
        /// <see cref="IGetEnumerationMappingsProcessor" />.
        /// </param>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        public GetEnumerationMappings(
            IGetEnumerationMappingsProcessor getEnumerationMappingsProcessor,
            ILoggerWrapper loggerWrapper)
        {
            this.getEnumerationMappingsProcessor = getEnumerationMappingsProcessor;
            this.loggerWrapper = loggerWrapper;
        }

        /// <summary>
        /// Entry method for the <c>get-enumeration-mappings</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpContext" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        [FunctionName("get-enumeration-mappings")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = null)]
            HttpRequest httpRequest)
        {
            IActionResult toReturn = null;

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headers = httpRequest.Headers;

            this.loggerWrapper.SetContext(headers);

            // TODO: Schema validation.
            await this.getEnumerationMappingsProcessor.GetEnumerationMappingsAsync()
                .ConfigureAwait(false);

            // TODO: Map up the result of the processor here.
            toReturn = new StatusCodeResult(200);

            return toReturn;
        }
    }
}