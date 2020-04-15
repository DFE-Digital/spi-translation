namespace Dfe.Spi.Translation.Domain.Definitions
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Describes the operations of the
    /// <see cref="EnumerationValuesResult" /> storage adapter.
    /// </summary>
    public interface IEnumerationValuesResultStorageAdapter
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
        
        /// <summary>
        /// Gets from storage all distinct enumeration values for a given
        /// enumeration <paramref name="name" />.
        /// </summary>
        /// <param name="name">
        /// The name of the enumeration.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="EnumerationValuesResult" />.
        /// </returns>
        Task<EnumerationValuesResult> GetEnumerationValuesResultAsync(
            string name,
            CancellationToken cancellationToken);
    }
}