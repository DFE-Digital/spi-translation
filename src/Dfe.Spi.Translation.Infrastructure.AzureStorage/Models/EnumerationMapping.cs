namespace Dfe.Spi.Translation.Infrastructure.AzureStorage.Models
{
    /// <summary>
    /// Represents an table enumeration mapping entry.
    /// </summary>
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