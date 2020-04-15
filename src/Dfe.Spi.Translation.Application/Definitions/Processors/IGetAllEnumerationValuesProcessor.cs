namespace Dfe.Spi.Translation.Application.Definitions.Processors
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Translation.Application.Models.Processors;

    /// <summary>
    /// Describes the operations of the get enumerations processor.
    /// </summary>
    public interface IGetAllEnumerationValuesProcessor
    {
        /// <summary>
        /// The get enumeration mappings entry method.
        /// </summary>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="GetEnumerationValuesResponse" />.
        /// </returns>
        Task<GetAllEnumerationValuesResponse> GetAllEnumerationValuesAsync(
            CancellationToken cancellationToken);
    }
}