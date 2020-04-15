namespace Dfe.Spi.Translation.Infrastructure.AzureStorage.Models
{
    using System.Diagnostics.CodeAnalysis;
    
    /// <summary>
    /// Represents an table enumeration mapping entry.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EnumerationMapping : ModelsBase
    {
        /// <summary>
        /// Gets or sets the mapping, as a <see cref="string" />.
        /// </summary>
        public string Mapping
        {
            get;
            set;
        }
    }
}