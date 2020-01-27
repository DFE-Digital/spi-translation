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
    public class MappingsResult : ModelsBase
    {
        /// <summary>
        /// Gets or sets the mappings as a
        /// <see cref="Dictionary{String, String}" />.
        /// </summary>
        public Dictionary<string, string> Mappings
        {
            get;
            set;
        }
    }
}
