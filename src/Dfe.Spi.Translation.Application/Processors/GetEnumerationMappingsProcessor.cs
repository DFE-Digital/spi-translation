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
    /// Implements <see cref="IGetEnumerationMappingsProcessor" />.
    /// </summary>
    public class GetEnumerationMappingsProcessor
        : IGetEnumerationMappingsProcessor
    {
        private readonly ICacheManager cacheManager;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="GetEnumerationMappingsProcessor" /> class.
        /// </summary>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        /// <param name="mappingsResultCacheManagerFactory">
        /// An instance of type
        /// <see cref="IMappingsResultCacheManagerFactory" />.
        /// </param>
        public GetEnumerationMappingsProcessor(
            ILoggerWrapper loggerWrapper,
            IMappingsResultCacheManagerFactory mappingsResultCacheManagerFactory)
        {
            if (mappingsResultCacheManagerFactory == null)
            {
                throw new ArgumentNullException(
                    nameof(mappingsResultCacheManagerFactory));
            }

            this.cacheManager = mappingsResultCacheManagerFactory.Create();
            this.loggerWrapper = loggerWrapper;
        }

        /// <inheritdoc />
        public async Task<GetEnumerationMappingsResponse> GetEnumerationMappingsAsync(
            GetEnumerationMappingsRequest getEnumerationMappingsRequest,
            CancellationToken cancellationToken)
        {
            GetEnumerationMappingsResponse toReturn = null;

            if (getEnumerationMappingsRequest == null)
            {
                throw new ArgumentNullException(
                    nameof(getEnumerationMappingsRequest));
            }

            EnumerationsKey enumerationsKey =
                getEnumerationMappingsRequest.EnumerationsKey;

            string enumerationsKeyStr = enumerationsKey.ExportToString();

            this.loggerWrapper.Debug(
                $"Pulling back {nameof(MappingsResult)} for " +
                $"{nameof(enumerationsKey)} {enumerationsKey} " +
                $"(\"{enumerationsKeyStr}\") from the " +
                $"{nameof(ICacheManager)}...");

            object unboxedMappingsResult = await this.cacheManager.GetAsync(
                enumerationsKeyStr,
                cancellationToken)
                .ConfigureAwait(false);

            MappingsResult mappingsResult =
                unboxedMappingsResult as MappingsResult;

            // Result will be non-null.
            this.loggerWrapper.Info(
                $"{nameof(MappingsResult)} pulled back from the " +
                $"{nameof(ICacheManager)}: {mappingsResult}");

            // If the below is null, then it indicates that mappings couldn't
            // be found for the given enumerationsKey.
            // This needs to be inspected by the caller and dealt with as
            // such.
            toReturn = new GetEnumerationMappingsResponse()
            {
                MappingsResult = mappingsResult,
            };

            return toReturn;
        }
    }
}