using System.Collections.Generic;

namespace Dfe.Spi.Translation.Domain.Models
{
    public class AllEnumerationValuesResult
    {
        /// <summary>
        /// Gets or sets a dictionary of enumerations and their values
        /// </summary>
        public Dictionary<string, EnumerationValuesResult> Enumerations { get; set; }
    }
}