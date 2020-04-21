namespace Dfe.Spi.Translation.Application.Models.Processors
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Response object for
    /// <see cref="IGetAllEnumerationValuesProcessor.GetAllEnumerationValuesAsync" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GetAllEnumerationValuesResponse
    {
        /// <summary>
        /// Gets or sets an instance of all enumerations and their values.
        /// </summary>
        public Dictionary<string, EnumerationValuesResult> Enumerations { get; set; }
    }
}