namespace Dfe.Spi.Translation.FunctionApp.Functions
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Http.Server;
    using Dfe.Spi.Common.Http.Server.Definitions;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
    using Dfe.Spi.Translation.Application.Models.Processors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;

    /// <summary>
    /// Entry class for the <c>get-enumeration-mappings</c> function.
    /// </summary>
    public class GetEnumerationMappings
        : FunctionsBase<GetEnumerationMappingsRequest>
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
            : base(loggerWrapper)
        {
            this.getEnumerationMappingsProcessor = getEnumerationMappingsProcessor;
            this.httpErrorBodyResultProvider = httpErrorBodyResultProvider;
            this.loggerWrapper = loggerWrapper;
        }

        /// <summary>
        /// Entry method for the <c>get-enumeration-mappings</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpContext" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        [FunctionName("get-enumeration-mappings")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = null)]
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            IActionResult toReturn = await this.ValidateAndRunAsync(
                httpRequest,
                cancellationToken)
                .ConfigureAwait(false);

            return toReturn;
        }

        /// <inheritdoc />
        protected override HttpErrorBodyResult GetMalformedErrorResponse()
        {
            HttpErrorBodyResult toReturn =
                this.httpErrorBodyResultProvider.GetHttpErrorBodyResult(
                    HttpStatusCode.BadRequest,
                    1);

            return toReturn;
        }

        /// <inheritdoc />
        protected override HttpErrorBodyResult GetSchemaValidationResponse(
            string message)
        {
            HttpErrorBodyResult toReturn =
                this.httpErrorBodyResultProvider.GetHttpErrorBodyResult(
                    HttpStatusCode.BadRequest,
                    2,
                    message);

            return toReturn;
        }

        /// <inheritdoc />
        protected async override Task<IActionResult> ProcessWellFormedRequestAsync(
            GetEnumerationMappingsRequest getEnumerationMappingsRequest,
            CancellationToken cancellationToken)
        {
            IActionResult toReturn = null;

            if (getEnumerationMappingsRequest == null)
            {
                throw new ArgumentNullException(
                    nameof(getEnumerationMappingsRequest));
            }

            GetEnumerationMappingsResponse getEnumerationMappingsResponse =
                await this.getEnumerationMappingsProcessor.GetEnumerationMappingsAsync(
                    getEnumerationMappingsRequest,
                    cancellationToken)
                    .ConfigureAwait(false);

            return toReturn;
        }
    }
}