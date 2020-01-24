namespace Dfe.Spi.Translation.Domain.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Represents a key to a set of enumerations.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EnumerationsKey : ModelsBase
    {
        private const char StringRepresentationDelimiter = '.';

        /// <summary>
        /// Gets or sets the adapter in parcitular to get mappings for.
        /// </summary>
        public string Adapter
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

        /// <summary>
        /// Parses an instance of <see cref="EnumerationsKey" /> from its
        /// <see cref="string" /> representation.
        /// </summary>
        /// <param name="value">
        /// The <see cref="string" /> representation of an
        /// <see cref="EnumerationsKey" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="EnumerationsKey" />.
        /// </returns>
        public static EnumerationsKey Parse(string value)
        {
            EnumerationsKey toReturn = null;

            if (!string.IsNullOrEmpty(value))
            {
                string[] parts = value.Split(
                    new char[] { StringRepresentationDelimiter },
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    string adapter = parts.First();
                    string name = parts.Last();

                    toReturn = new EnumerationsKey()
                    {
                        Adapter = adapter,
                        Name = name,
                    };
                }
            }

            if (toReturn == null)
            {
                throw new FormatException(
                    $"Could not parse the string \"{value}\" into an " +
                    $"instance of {nameof(EnumerationsKey)}.");
            }

            return toReturn;
        }

        /// <summary>
        /// Exports this instance to a <see cref="string" /> value. This
        /// <see cref="string" /> value can then be used to create an instance
        /// of <see cref="EnumerationsKey" /> via <see cref="Parse(string)" />.
        /// </summary>
        /// <returns>
        /// The exported <see cref="string" /> representation of this instance.
        /// </returns>
        public string ExportToString()
        {
            string toReturn =
                this.Adapter + StringRepresentationDelimiter + this.Name;

            return toReturn;
        }
    }
}