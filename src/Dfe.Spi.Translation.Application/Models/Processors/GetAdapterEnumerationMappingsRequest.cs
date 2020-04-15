namespace Dfe.Spi.Translation.Application.Models.Processors
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using Dfe.Spi.Common.Models;
    using Dfe.Spi.Translation.Application.Definitions.Processors;

    /// <summary>
    /// Request object for
    /// <see cref="IGetEnumerationMappingsProcessor.GetEnumerationMappingsAsync(GetEnumerationMappingsRequest, CancellationToken)" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GetAdapterEnumerationMappingsRequest : RequestResponseBase
    {
        /// <summary>
        /// Gets or sets the adapter name
        /// </summary>
        public string AdapterName
        {
            get;
            set;
        }
    }
}