namespace Dfe.Spi.Translation.Application.Processors
{
    using System.Threading.Tasks;
    using Dfe.Spi.Translation.Application.Definitions.Processors;

    /// <summary>
    /// Implements <see cref="IGetEnumerationMappingsProcessor" />.
    /// </summary>
    public class GetEnumerationMappingsProcessor
        : IGetEnumerationMappingsProcessor
    {
        /// <inheritdoc />
        public Task GetEnumerationMappingsAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}