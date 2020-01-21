namespace Dfe.Spi.Translation.Application.Processors
{
    using System.Threading.Tasks;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
    using Dfe.Spi.Translation.Application.Models.Processors;

    /// <summary>
    /// Implements <see cref="IGetEnumerationMappingsProcessor" />.
    /// </summary>
    public class GetEnumerationMappingsProcessor
        : IGetEnumerationMappingsProcessor
    {
        /// <inheritdoc />
        public Task<GetEnumerationMappingsResponse> GetEnumerationMappingsAsync(
            GetEnumerationMappingsRequest getEnumerationMappingsRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}