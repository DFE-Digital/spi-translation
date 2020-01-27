namespace Dfe.Spi.Translation.Application.Models.Processors
{
    using System.Threading;
    using Dfe.Spi.Translation.Application.Definitions.Processors;

    /// <summary>
    /// Request object for
    /// <see cref="IGetEnumerationValuesProcessor.GetEnumerationValuesAsync(GetEnumerationValuesRequest, CancellationToken)" />.
    /// </summary>
    public class GetEnumerationValuesRequest
    {
        /// <summary>
        /// Gets or sets the name of the enumeration.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}