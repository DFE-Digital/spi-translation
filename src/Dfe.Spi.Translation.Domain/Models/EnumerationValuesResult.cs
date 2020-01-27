namespace Dfe.Spi.Translation.Domain.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents the result of looking up all distinct enumeration values
    /// for a single enumeration from the underlying storage.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EnumerationValuesResult : ModelsBase
    {
        /// <summary>
        /// Gets or sets a set of <see cref="string" /> values.
        /// </summary>
        public IEnumerable<string> EnumerationValues
        {
            get;
            set;
        }
    }
}