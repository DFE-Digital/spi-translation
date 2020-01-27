namespace Dfe.Spi.Translation.Application.Processors
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Caching.Definitions.Managers;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Translation.Application.Definitions.Factories;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
    using Dfe.Spi.Translation.Application.Models.Processors;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Implements <see cref="IGetEnumerationValuesProcessor" />.
    /// </summary>
    public class GetEnumerationValuesProcessor : IGetEnumerationValuesProcessor
    {
        private readonly ICacheManager cacheManager;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="GetEnumerationValuesProcessor" /> class.
        /// </summary>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        /// <param name="enumerationValuesResultCacheManagerFactory">
        /// An instance of type
        /// <see cref="IEnumerationValuesResultCacheManagerFactory" />.
        /// </param>
        public GetEnumerationValuesProcessor(
            ILoggerWrapper loggerWrapper,
            IEnumerationValuesResultCacheManagerFactory enumerationValuesResultCacheManagerFactory)
        {
            if (enumerationValuesResultCacheManagerFactory == null)
            {
                throw new ArgumentNullException(
                    nameof(enumerationValuesResultCacheManagerFactory));
            }

            this.loggerWrapper = loggerWrapper;
            this.cacheManager =
                enumerationValuesResultCacheManagerFactory.Create();
        }

        /// <inheritdoc />
        public async Task<GetEnumerationValuesResponse> GetEnumerationValuesAsync(
            GetEnumerationValuesRequest getEnumerationValuesRequest,
            CancellationToken cancellationToken)
        {
            GetEnumerationValuesResponse toReturn = null;

            if (getEnumerationValuesRequest == null)
            {
                throw new ArgumentNullException(
                    nameof(getEnumerationValuesRequest));
            }

            string name = getEnumerationValuesRequest.Name;

            this.loggerWrapper.Debug(
                $"Pulling back {nameof(EnumerationValuesResult)} for " +
                $"{nameof(name)} \"{name}\" from the " +
                $"{nameof(ICacheManager)}...");

            object unboxedEnumerationValuesResult =
                await this.cacheManager.GetAsync(name, cancellationToken)
                    .ConfigureAwait(false);

            EnumerationValuesResult enumerationValuesResult =
                unboxedEnumerationValuesResult as EnumerationValuesResult;

            // Result will be non-null.
            this.loggerWrapper.Info(
                $"{nameof(EnumerationValuesResult)} pulled back from the " +
                $"{nameof(ICacheManager)}: {enumerationValuesResult}.");

            toReturn = new GetEnumerationValuesResponse()
            {
                EnumerationValuesResult = enumerationValuesResult,
            };

            return toReturn;
        }
    }
}