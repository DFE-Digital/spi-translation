namespace Dfe.Spi.Translation.Application.Factories
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Caching;
    using Dfe.Spi.Common.Caching.Definitions.Managers;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Translation.Application.Definitions.Caches;
    using Dfe.Spi.Translation.Application.Definitions.Factories;
    using Dfe.Spi.Translation.Domain.Definitions;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Implements <see cref="IEnumerationValuesResultCacheManagerFactory" />.
    /// </summary>
    public class EnumerationValuesResultCacheManagerFactory
        : IEnumerationValuesResultCacheManagerFactory
    {
        private readonly IEnumerationValuesResultCache enumerationValuesResultCache;
        private readonly IEnumerationValuesResultStorageAdapter enumerationValuesResultStorageAdapter;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="EnumerationValuesResultCacheManagerFactory" /> class.
        /// </summary>
        /// <param name="enumerationValuesResultCache">
        /// An instance of type <see cref="IEnumerationValuesResultCache" />.
        /// </param>
        /// <param name="enumerationValuesResultStorageAdapter">
        /// An instance of type
        /// <see cref="IEnumerationValuesResultStorageAdapter" />.
        /// </param>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        public EnumerationValuesResultCacheManagerFactory(
            IEnumerationValuesResultCache enumerationValuesResultCache,
            IEnumerationValuesResultStorageAdapter enumerationValuesResultStorageAdapter,
            ILoggerWrapper loggerWrapper)
        {
            this.enumerationValuesResultCache = enumerationValuesResultCache;
            this.enumerationValuesResultStorageAdapter = enumerationValuesResultStorageAdapter;
            this.loggerWrapper = loggerWrapper;
        }

        /// <inheritdoc />
        public ICacheManager Create()
        {
            CacheManager toReturn = new CacheManager(
                this.enumerationValuesResultCache,
                this.loggerWrapper,
                this.InitialiseCacheItemAsync);

            return toReturn;
        }

        /// <inheritdoc />
        public async Task<object> InitialiseCacheItemAsync(
           string key,
           CancellationToken cancellationToken)
        {
            object toReturn = null;

            this.loggerWrapper.Debug(
                $"Looking up all enumeration values from storage with " +
                $"name \"{key}\"...");

            EnumerationValuesResult allEnumerationsResult =
                await this.enumerationValuesResultStorageAdapter.GetAllEnumerationValuesResultAsync(
                    key,
                    cancellationToken)
                    .ConfigureAwait(false);

            this.loggerWrapper.Info(
                $"Pulled {allEnumerationsResult} from storage for {key}.");

            toReturn = allEnumerationsResult;

            return toReturn;
        }
    }
}