namespace Dfe.Spi.Translation.Domain.Models
{
    /// <summary>
    /// Represents a reference to an enumeration.
    /// </summary>
    public class EnumerationsReference : ModelsBase
    {
        /// <summary>
        /// Gets or sets the adapter name to get mappings for.
        /// </summary>
        public string AdapterName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the enumeration to return.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}