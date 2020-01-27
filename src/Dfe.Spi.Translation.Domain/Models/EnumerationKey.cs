namespace Dfe.Spi.Translation.Domain.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Represents a key to a set of enumerations.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EnumerationKey : ModelsBase
    {
        /// <summary>
        /// The delimiter character used in enumeration keys.
        /// </summary>
        public const char StringRepresentationDelimiter = '.';

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
        /// Parses an instance of <see cref="EnumerationKey" /> from its
        /// <see cref="string" /> representation.
        /// </summary>
        /// <param name="value">
        /// The <see cref="string" /> representation of an
        /// <see cref="EnumerationKey" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="EnumerationKey" />.
        /// </returns>
        public static EnumerationKey Parse(string value)
        {
            EnumerationKey toReturn = null;

            if (!string.IsNullOrEmpty(value))
            {
                string[] parts = value.Split(
                    new char[] { StringRepresentationDelimiter },
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    string adapter = parts.First();
                    string name = parts.Last();

                    toReturn = new EnumerationKey()
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
                    $"instance of {nameof(EnumerationKey)}.");
            }

            return toReturn;
        }

        /// <summary>
        /// Exports this instance to a <see cref="string" /> value. This
        /// <see cref="string" /> value can then be used to create an instance
        /// of <see cref="EnumerationKey" /> via <see cref="Parse(string)" />.
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