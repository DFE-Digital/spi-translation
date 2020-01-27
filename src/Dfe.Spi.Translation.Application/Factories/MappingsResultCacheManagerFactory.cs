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
    /// Implements <see cref="IMappingsResultCacheManagerFactory" />.
    /// </summary>
    public class MappingsResultCacheManagerFactory
        : IMappingsResultCacheManagerFactory
    {
        private readonly ILoggerWrapper loggerWrapper;
        private readonly IMappingsResultCache mappingsResultCache;
        private readonly IMappingsResultStorageAdapter mappingsResultStorageAdapter;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="MappingsResultCacheManagerFactory" /> class.
        /// </summary>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        /// <param name="mappingsResultCache">
        /// An instance of type <see cref="IMappingsResultCache" />.
        /// </param>
        /// <param name="mappingsResultStorageAdapter">
        /// An instance of type <see cref="IMappingsResultStorageAdapter" />.
        /// </param>
        public MappingsResultCacheManagerFactory(
            ILoggerWrapper loggerWrapper,
            IMappingsResultCache mappingsResultCache,
            IMappingsResultStorageAdapter mappingsResultStorageAdapter)
        {
            this.loggerWrapper = loggerWrapper;
            this.mappingsResultCache = mappingsResultCache;
            this.mappingsResultStorageAdapter = mappingsResultStorageAdapter;
        }

        /// <inheritdoc />
        public ICacheManager Create()
        {
            CacheManager toReturn = new CacheManager(
                this.mappingsResultCache,
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
                $"Parsing \"{key}\" as an instance of " +
                $"{nameof(EnumerationKey)}...");

            EnumerationKey enumerationsKey = EnumerationKey.Parse(key);

            this.loggerWrapper.Info(
                $"Parsed {enumerationsKey} from \"{key}\".");

            this.loggerWrapper.Debug(
                $"Fetching {enumerationsKey} from storage...");

            MappingsResult mappingsResult =
                await this.mappingsResultStorageAdapter.GetMappingsResultAsync(
                    enumerationsKey,
                    cancellationToken)
                   .ConfigureAwait(false);

            this.loggerWrapper.Info(
                $"Pulled {mappingsResult} from storage for " +
                $"{enumerationsKey}.");

            toReturn = mappingsResult;

            return toReturn;
        }
    }
}