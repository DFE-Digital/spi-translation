﻿namespace Dfe.Spi.Translation.FunctionApp.Functions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;

    /// <summary>
    /// Entry class for the <c>HeartBeat</c> function.
    /// Note: EAPIM's health check is *not* looking for kebab-case.
    ///       So, this is a one-off.
    /// </summary>
    public static class HeartBeat
    {
        /// <summary>
        /// Entry method for the <c>HeartBeat</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        [FunctionName(nameof(HeartBeat))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "HeartBeat")]
            HttpRequest httpRequest)
        {
            OkResult toReturn = new OkResult();

            // Just needs to return 200/OK.
            return toReturn;
        }
    }
}