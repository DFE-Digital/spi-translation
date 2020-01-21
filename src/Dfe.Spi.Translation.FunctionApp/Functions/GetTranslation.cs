namespace Dfe.Spi.Translation.FunctionApp.Functions
{
    using System;
    using Dfe.Spi.Common.Logging.Definitions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;

    /// <summary>
    /// Entry class for the <c>get-translation</c> function.
    /// </summary>
    public class GetTranslation
    {
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="GetTranslation" />
        /// class.
        /// </summary>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        public GetTranslation(
            ILoggerWrapper loggerWrapper)
        {
            this.loggerWrapper = loggerWrapper;
        }

        /// <summary>
        /// Entry method for the <c>get-translation</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpContext" />.
        /// </param>
        [FunctionName("get-translation")]
        public void Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = null)]
            HttpRequest httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headers = httpRequest.Headers;

            this.loggerWrapper.SetContext(headers);

            this.loggerWrapper.Debug(
                "TEST DEBUG MESSAGE, to see if the debug messages are " +
                "logging to app insights.");

            this.loggerWrapper.Info(
                "TEST INFO MESSAGE, to see if the info messages are " +
                "logging to app insights.");
        }
    }
}