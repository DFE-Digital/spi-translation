namespace Dfe.Spi.Translation.Infrastructure.AzureStorage
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Translation.Domain.Definitions;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Implements <see cref="IMappingsResultStorageAdapter" />.
    /// </summary>
    public class MappingsResultStorageAdapter : IMappingsResultStorageAdapter
    {
        /// <inheritdoc />
        public Task<MappingsResult> GetMappingsResultAsync(
            EnumerationsKey enumerationsKey,
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}