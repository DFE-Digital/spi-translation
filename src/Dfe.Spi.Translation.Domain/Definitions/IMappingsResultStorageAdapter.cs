namespace Dfe.Spi.Translation.Domain.Definitions
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Describes the operations of the <see cref="MappingsResult" /> storage
    /// provider.
    /// </summary>
    public interface IMappingsResultStorageAdapter
    {
        /// <summary>
        /// Gets from storage the specified <see cref="MappingsResult" />,
        /// using the supplied <paramref name="enumerationsKey" />.
        /// </summary>
        /// <param name="enumerationsKey">
        /// An instance of <see cref="EnumerationsKey" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="MappingsResult" />, unless not found,
        /// in which case null.
        /// </returns>
        Task<MappingsResult> GetMappingsResultAsync(
            EnumerationsKey enumerationsKey,
            CancellationToken cancellationToken);
    }
}