namespace Dfe.Spi.Translation.Application.Caches
{
    using Dfe.Spi.Common.Caching;
    using Dfe.Spi.Translation.Application.Definitions.Caches;

    /// <summary>
    /// Implements <see cref="IMappingsResultCache" />.
    /// </summary>
    public class MappingsResultCache
        : CacheProvider, IMappingsResultCache
    {
        // Nothing - inherits all it needs for now.
    }
}