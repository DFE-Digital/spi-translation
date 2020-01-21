namespace Dfe.Spi.Translation.Application.Definitions.Processors
{
    using System.Threading.Tasks;

    /// <summary>
    /// Describes the operations of the get enumeration mappings processor.
    /// </summary>
    public interface IGetEnumerationMappingsProcessor
    {
        /// <summary>
        /// The get enumeration mappings entry method.
        /// </summary>
        Task GetEnumerationMappingsAsync();
    }
}