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
    /// Implements <see cref="IGetAdapterEnumerationMappingsProcessor" />.
    /// </summary>
    public class GetAdapterEnumerationMappingsProcessor
        : IGetAdapterEnumerationMappingsProcessor
    {
        private readonly ICacheManager cacheManager;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="GetAdapterEnumerationMappingsProcessor" /> class.
        /// </summary>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        /// <param name="adapterMappingsResultCacheManagerFactory">
        /// An instance of type
        /// <see cref="IAdapterMappingsResultCacheManagerFactory" />.
        /// </param>
        public GetAdapterEnumerationMappingsProcessor(
            ILoggerWrapper loggerWrapper,
            IAdapterMappingsResultCacheManagerFactory adapterMappingsResultCacheManagerFactory)
        {
            if (adapterMappingsResultCacheManagerFactory == null)
            {
                throw new ArgumentNullException(
                    nameof(adapterMappingsResultCacheManagerFactory));
            }

            this.cacheManager = adapterMappingsResultCacheManagerFactory.Create();
            this.loggerWrapper = loggerWrapper;
        }

        public async Task<GetAdapterEnumerationMappingsResponse> GetAdapterEnumerationMappingsAsync(
            GetAdapterEnumerationMappingsRequest getAdapterEnumerationMappingsRequest, 
            CancellationToken cancellationToken)
        {
            if (getAdapterEnumerationMappingsRequest == null)
            {
                throw new ArgumentNullException(nameof(getAdapterEnumerationMappingsRequest));
            }
            
            this.loggerWrapper.Debug(
                $"Pulling back {nameof(AdapterMappingsResult)} for " +
                $"adapter {getAdapterEnumerationMappingsRequest.AdapterName} " +
                $"from the {nameof(ICacheManager)}...");

            object unboxedMappingsResult = await this.cacheManager.GetAsync(
                    getAdapterEnumerationMappingsRequest.AdapterName,
                    cancellationToken)
                .ConfigureAwait(false);

            AdapterMappingsResult adapterMappingsResult =
                unboxedMappingsResult as AdapterMappingsResult;

            // Result will be non-null.
            this.loggerWrapper.Info(
                $"{nameof(MappingsResult)} pulled back from the " +
                $"{nameof(ICacheManager)}: {adapterMappingsResult}.");

            return new GetAdapterEnumerationMappingsResponse
            {
                AdapterMappingsResult = adapterMappingsResult,
            };
        }
    }
}