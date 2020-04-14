namespace Dfe.Spi.Translation.Domain.Definitions
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Describes the operations of the
    /// <see cref="AllEnumerationValuesResult" /> storage adapter.
    /// </summary>
    public interface IAllEnumerationValuesResultStorageAdapter
    {
        /// <summary>
        /// Gets from storage all distinct enumeration values for each
        /// enumeration.
        /// </summary>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="AllEnumerationValuesResult" />.
        /// </returns>
        Task<AllEnumerationValuesResult> GetAllEnumerationValuesResultAsync(
            CancellationToken cancellationToken);
    }
}