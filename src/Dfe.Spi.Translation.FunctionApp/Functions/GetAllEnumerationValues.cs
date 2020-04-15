using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
    /// Entry class for the <c>get-all-enumeration-values</c> function.
    /// </summary>
    public class GetAllEnumerationValues
    {
        private const string FunctionName = nameof(GetAllEnumerationValues);
        
        private readonly IGetAllEnumerationValuesProcessor getAllEnumerationsProcessor;
        private readonly IHttpSpiExecutionContextManager httpSpiExecutionContextManager;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="GetEnumerationValues" />
        /// class.
        /// </summary>
        /// <param name="getAllEnumerationsProcessor">
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
        public GetAllEnumerationValues(
            IGetAllEnumerationValuesProcessor getAllEnumerationsProcessor,
            IHttpSpiExecutionContextManager httpSpiExecutionContextManager,
            ILoggerWrapper loggerWrapper)
        {
            this.getAllEnumerationsProcessor = getAllEnumerationsProcessor;
            this.httpSpiExecutionContextManager = httpSpiExecutionContextManager;
            this.loggerWrapper = loggerWrapper;
        }
        
        /// <summary>
        /// Entry method for the <c>get-all-enumeration-values</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        [FunctionName(FunctionName)]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "enumerations")]
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            this.httpSpiExecutionContextManager.SetContext(httpRequest.Headers);

            this.loggerWrapper.Debug(
                $"Invoking {nameof(IGetAllEnumerationValuesProcessor)}");
            
            GetAllEnumerationValuesResponse getEnumerationValuesResponse =
                await this.getAllEnumerationsProcessor.GetAllEnumerationValuesAsync(
                        cancellationToken)
                    .ConfigureAwait(false);

            this.loggerWrapper.Info(
                $"{nameof(IGetAllEnumerationValuesProcessor)} invoked with " +
                $"success: {getEnumerationValuesResponse}.");
            
            return new JsonResult(getEnumerationValuesResponse);
        }
    }
}