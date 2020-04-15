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
    /// Implements <see cref="IAdapterMappingsResultCacheManagerFactory" />.
    /// </summary>
    public class AdapterMappingsResultCacheManagerFactory
        : IAdapterMappingsResultCacheManagerFactory
    {
        private readonly ILoggerWrapper loggerWrapper;
        private readonly IAdapterMappingsResultCache adapterMappingsResultCache;
        private readonly IMappingsResultStorageAdapter mappingsResultStorageAdapter;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="MappingsResultCacheManagerFactory" /> class.
        /// </summary>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        /// <param name="adapterMappingsResultCache">
        /// An instance of type <see cref="IMappingsResultCache" />.
        /// </param>
        /// <param name="mappingsResultStorageAdapter">
        /// An instance of type <see cref="IMappingsResultStorageAdapter" />.
        /// </param>
        public AdapterMappingsResultCacheManagerFactory(
            ILoggerWrapper loggerWrapper,
            IAdapterMappingsResultCache adapterMappingsResultCache,
            IMappingsResultStorageAdapter mappingsResultStorageAdapter)
        {
            this.loggerWrapper = loggerWrapper;
            this.adapterMappingsResultCache = adapterMappingsResultCache;
            this.mappingsResultStorageAdapter = mappingsResultStorageAdapter;
        }
        
        public ICacheManager Create()
        {
            CacheManager toReturn = new CacheManager(
                this.adapterMappingsResultCache,
                this.loggerWrapper,
                this.InitialiseCacheItemAsync);

            return toReturn;
        }

        public async Task<object> InitialiseCacheItemAsync(string key, CancellationToken cancellationToken)
        {
            object toReturn = null;

            this.loggerWrapper.Debug(
                $"Fetching {key} from storage...");

            AdapterMappingsResult adapterMappingsResult =
                await this.mappingsResultStorageAdapter.GetAdapterMappingsResultAsync(
                        key,
                        cancellationToken)
                    .ConfigureAwait(false);

            this.loggerWrapper.Info(
                $"Pulled {adapterMappingsResult} from storage for " +
                $"{key}.");

            toReturn = adapterMappingsResult;

            return toReturn;
        }
    }
}