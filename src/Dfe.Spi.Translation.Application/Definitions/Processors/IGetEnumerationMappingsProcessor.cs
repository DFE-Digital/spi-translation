namespace Dfe.Spi.Translation.Application.Definitions.Processors
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Translation.Application.Models.Processors;

    /// <summary>
    /// Describes the operations of the get enumeration mappings processor.
    /// </summary>
    public interface IGetEnumerationMappingsProcessor
    {
        /// <summary>
        /// The get enumeration mappings entry method.
        /// </summary>
        /// <param name="getEnumerationMappingsRequest">
        /// An instance of <see cref="GetEnumerationMappingsRequest" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="GetEnumerationMappingsResponse" />.
        /// </returns>
        Task<GetEnumerationMappingsResponse> GetEnumerationMappingsAsync(
            GetEnumerationMappingsRequest getEnumerationMappingsRequest,
            CancellationToken cancellationToken);
    }
}