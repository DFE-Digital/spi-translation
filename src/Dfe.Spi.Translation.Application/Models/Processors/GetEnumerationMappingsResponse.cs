namespace Dfe.Spi.Translation.Application.Models.Processors
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using Dfe.Spi.Common.Models;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Response object for
    /// <see cref="IGetEnumerationMappingsProcessor.GetEnumerationMappingsAsync(GetEnumerationMappingsRequest, CancellationToken)" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GetEnumerationMappingsResponse : RequestResponseBase
    {
        /// <summary>
        /// Gets or sets an instance of <see cref="MappingsResult" />.
        /// </summary>
        public MappingsResult MappingsResult
        {
            get;
            set;
        }
    }
}