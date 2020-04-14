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
    
    public class AllEnumerationValuesResultCacheManagerFactory
        : IAllEnumerationValuesResultCacheManagerFactory
    {
        private readonly IAllEnumerationValuesResultCache allEnumerationValuesResultCache;
        private readonly IAllEnumerationValuesResultStorageAdapter allEnumerationValuesResultStorageAdapter;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="EnumerationValuesResultCacheManagerFactory" /> class.
        /// </summary>
        /// <param name="allEnumerationValuesResultCache">
        /// An instance of type <see cref="IEnumerationValuesResultCache" />.
        /// </param>
        /// <param name="allEnumerationValuesResultStorageAdapter">
        /// An instance of type
        /// <see cref="IEnumerationValuesResultStorageAdapter" />.
        /// </param>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        public AllEnumerationValuesResultCacheManagerFactory(
            IAllEnumerationValuesResultCache allEnumerationValuesResultCache,
            IAllEnumerationValuesResultStorageAdapter allEnumerationValuesResultStorageAdapter,
            ILoggerWrapper loggerWrapper)
        {
            this.allEnumerationValuesResultCache = allEnumerationValuesResultCache;
            this.allEnumerationValuesResultStorageAdapter = allEnumerationValuesResultStorageAdapter;
            this.loggerWrapper = loggerWrapper;
        }
        
        public ICacheManager Create()
        {
            CacheManager toReturn = new CacheManager(
                this.allEnumerationValuesResultCache,
                this.loggerWrapper,
                this.InitialiseCacheItemAsync);

            return toReturn;
        }

        public async Task<object> InitialiseCacheItemAsync(string key, CancellationToken cancellationToken)
        {
            object toReturn = null;

            this.loggerWrapper.Debug(
                $"Looking up all enumeration values from storage with " +
                $"name \"{key}\"...");

            AllEnumerationValuesResult allEnumerationsResult =
                await this.allEnumerationValuesResultStorageAdapter.GetAllEnumerationValuesResultAsync(
                        cancellationToken)
                    .ConfigureAwait(false);

            this.loggerWrapper.Info(
                $"Pulled {allEnumerationsResult} from storage for {key}.");

            toReturn = allEnumerationsResult;

            return toReturn;
        }
    }
}