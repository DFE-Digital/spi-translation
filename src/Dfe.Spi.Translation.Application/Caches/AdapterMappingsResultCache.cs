namespace Dfe.Spi.Translation.Application.Caches
{
    using Dfe.Spi.Common.Caching;
    using Dfe.Spi.Translation.Application.Definitions.Caches;

    /// <summary>
    /// Implements <see cref="IAdapterMappingsResultCache" />.
    /// </summary>
    public class AdapterMappingsResultCache
        : CacheProvider, IAdapterMappingsResultCache
    {
        // Nothing - inherits all it needs for now.
    }
}