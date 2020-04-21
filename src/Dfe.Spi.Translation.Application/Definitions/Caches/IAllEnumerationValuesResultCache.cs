namespace Dfe.Spi.Translation.Application.Definitions.Caches
{
    using Dfe.Spi.Common.Caching.Definitions;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Describes the operations of the <see cref="AllEnumerationValuesResult" />
    /// cache.
    /// </summary>
    public interface IAllEnumerationValuesResultCache : ICacheProvider
    {
        // Nothing, it just inherits what it needs.
    }
}