namespace Dfe.Spi.Translation.FunctionApp.Functions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Entry class for the <c>get-translation</c> function.
    /// </summary>
    public class GetTranslation
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="GetTranslation" />
        /// class.
        /// </summary>
        public GetTranslation()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Entry method for the <c>get-translation</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpContext" />.
        /// </param>
        /// <param name="logger">
        /// An instance of type <see cref="ILogger" />.
        /// </param>
        [FunctionName("get-translation")]
        public static void Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = null)]
            HttpRequest httpRequest,
            ILogger logger)
        {
            logger.LogDebug(
                "TEST DEBUG MESSAGE, to see if the debug messages are " +
                "logging to app insights.");

            logger.LogInformation(
                "TEST INFO MESSAGE, to see if the info messages are " +
                "logging to app insights.");
        }
    }
}