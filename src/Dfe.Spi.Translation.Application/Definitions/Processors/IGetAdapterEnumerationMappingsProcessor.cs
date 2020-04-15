namespace Dfe.Spi.Translation.Application.Definitions.Processors
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Translation.Application.Models.Processors;

    /// <summary>
    /// Describes the operations of the get adapter enumeration mappings processor.
    /// </summary>
    public interface IGetAdapterEnumerationMappingsProcessor
    {
        /// <summary>
        /// The get adapter enumeration mappings entry method.
        /// </summary>
        /// <param name="getAdapterEnumerationMappingsRequest">
        /// An instance of <see cref="GetAdapterEnumerationMappingsRequest" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="GetAdapterEnumerationMappingsResponse" />.
        /// </returns>
        Task<GetAdapterEnumerationMappingsResponse> GetAdapterEnumerationMappingsAsync(
            GetAdapterEnumerationMappingsRequest getAdapterEnumerationMappingsRequest,
            CancellationToken cancellationToken);
    }
}