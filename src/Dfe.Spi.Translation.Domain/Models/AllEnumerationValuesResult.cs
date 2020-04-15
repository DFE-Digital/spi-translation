namespace Dfe.Spi.Translation.Domain.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents the result of looking up all distinct enumerations
    /// and their values from the underlying storage.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AllEnumerationValuesResult
    {
        /// <summary>
        /// Gets or sets a dictionary of enumerations and their values.
        /// </summary>
        public Dictionary<string, EnumerationValuesResult> Enumerations { get; set; }
    }
}