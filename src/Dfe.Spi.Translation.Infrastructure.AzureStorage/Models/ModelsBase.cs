namespace Dfe.Spi.Translation.Infrastructure.AzureStorage.Models
{
    using Meridian.MeaningfulToString;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Abstract base class for all models in the
    /// <see cref="Dfe.Spi.Translation.Infrastructure.AzureStorage.Models" />
    /// namespace.
    /// </summary>
    public abstract class ModelsBase : TableEntity
    {
        /// <inheritdoc />
        public override string ToString()
        {
            return this.MeaningfulToString();
        }
    }
}