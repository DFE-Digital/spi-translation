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
    public class GetAllEnumerationValuesProcessor : IGetAllEnumerationValuesProcessor
    {
        private readonly ICacheManager cacheManager;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="GetAllEnumerationValuesProcessor" /> class.
        /// </summary>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        /// <param name="allEnumerationValuesResultCacheManagerFactory">
        /// An instance of type
        /// <see cref="IEnumerationValuesResultCacheManagerFactory" />.
        /// </param>
        public GetAllEnumerationValuesProcessor(
            ILoggerWrapper loggerWrapper,
            IAllEnumerationValuesResultCacheManagerFactory allEnumerationValuesResultCacheManagerFactory)
        {
            if (allEnumerationValuesResultCacheManagerFactory == null)
            {
                throw new ArgumentNullException(
                    nameof(allEnumerationValuesResultCacheManagerFactory));
            }

            this.loggerWrapper = loggerWrapper;
            this.cacheManager =
                allEnumerationValuesResultCacheManagerFactory.Create();
        }

        public async Task<GetAllEnumerationValuesResponse> GetAllEnumerationValuesAsync(CancellationToken cancellationToken)
        {
            const string cacheKeyName = "__AllEnumerations";
            GetAllEnumerationValuesResponse toReturn = null;

            this.loggerWrapper.Debug(
                $"Pulling back {nameof(AllEnumerationValuesResult)} from the " +
                $"{nameof(ICacheManager)}...");

            object unboxedEnumerationValuesResult =
                await this.cacheManager.GetAsync(cacheKeyName, cancellationToken)
                    .ConfigureAwait(false);

            AllEnumerationValuesResult enumerationValuesResult =
                unboxedEnumerationValuesResult as AllEnumerationValuesResult;

            // Result will be non-null.
            this.loggerWrapper.Info(
                $"{nameof(AllEnumerationValuesResult)} pulled back from the " +
                $"{nameof(ICacheManager)}: {enumerationValuesResult}.");

            toReturn = new GetAllEnumerationValuesResponse()
            {
                Enumerations = enumerationValuesResult.Enumerations,
            };

            return toReturn;
        }
    }
}