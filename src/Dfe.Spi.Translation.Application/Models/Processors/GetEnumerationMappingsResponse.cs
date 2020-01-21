namespace Dfe.Spi.Translation.Application.Models.Processors
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Dfe.Spi.Common.Models;
    using Dfe.Spi.Translation.Application.Definitions.Processors;

    /// <summary>
    /// Response object for
    /// <see cref="IGetEnumerationMappingsProcessor.GetEnumerationMappingsAsync(GetEnumerationMappingsRequest)" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage(
        "Microsoft.Usage",
        "CA2227",
        Justification = "This is a data transfer object.")]
    public class GetEnumerationMappingsResponse : RequestResponseBase
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