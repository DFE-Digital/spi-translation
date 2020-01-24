namespace Dfe.Spi.Translation.Application.Factories
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Caching;
    using Dfe.Spi.Common.Caching.Definitions.Managers;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Translation.Application.Definitions.Caches;
    using Dfe.Spi.Translation.Application.Definitions.Factories;

    /// <summary>
    /// Implements <see cref="IMappingsResultCacheManagerFactory" />.
    /// </summary>
    public class MappingsResultCacheManagerFactory
        : IMappingsResultCacheManagerFactory
    {
        private readonly ILoggerWrapper loggerWrapper;
        private readonly IMappingsResultCache mappingsResultCache;

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
        public MappingsResultCacheManagerFactory(
            ILoggerWrapper loggerWrapper,
            IMappingsResultCache mappingsResultCache)
        {
            this.loggerWrapper = loggerWrapper;
            this.mappingsResultCache = mappingsResultCache;
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
        public Task<object> InitialiseCacheItemAsync(
            string key,
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}