namespace Dfe.Spi.Translation.Application.Definitions.Caches
{
    using Dfe.Spi.Common.Caching.Definitions;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Describes the operations of the <see cref="EnumerationValuesResult" />
    /// cache.
    /// </summary>
    public interface IEnumerationValuesResultCache : ICacheProvider
    {
        // Nothing, it just inherits what it needs.
    }
}