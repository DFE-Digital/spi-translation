namespace Dfe.Spi.Translation.Domain.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents the result of looking up mappings from the underlying
    /// storage.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage(
        "Microsoft.Usage",
        "CA2227",
        Justification = "This is a data transfer object.")]
    public class AdapterMappingsResult
    {
        /// <summary>
        /// Gets or sets all enumerations and their mappings for adapter.
        /// </summary>
        public Dictionary<string, MappingsResult> EnumerationMappings { get; set; }
    }
}