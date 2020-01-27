namespace Dfe.Spi.Translation.Application.Definitions.Caches
{
    using Dfe.Spi.Common.Caching.Definitions;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Describes the operations of the <see cref="MappingsResult" /> cache.
    /// </summary>
    public interface IMappingsResultCache : ICacheProvider
    {
        // Nothing, just inherits what it needs.
    }
}