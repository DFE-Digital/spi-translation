namespace Dfe.Spi.Translation.Application.Models.Processors
{
    using System.Threading;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
    using Dfe.Spi.Translation.Domain.Models;

    /// <summary>
    /// Response object for
    /// <see cref="IGetEnumerationValuesProcessor.GetEnumerationValuesAsync(GetEnumerationValuesRequest, CancellationToken)" />.
    /// </summary>
    public class GetEnumerationValuesResponse
    {
        /// <summary>
        /// Gets or sets an instance of
        /// <see cref="Domain.Models.EnumerationValuesResult" />.
        /// </summary>
        public EnumerationValuesResult EnumerationValuesResult
        {
            get;
            set;
        }
    }
}