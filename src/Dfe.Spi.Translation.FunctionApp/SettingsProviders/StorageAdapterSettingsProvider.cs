namespace Dfe.Spi.Translation.FunctionApp.SettingsProviders
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Dfe.Spi.Translation.Domain.Definitions.SettingsProviders;

    /// <summary>
    /// Implements <see cref="IStorageAdapterSettingsProvider" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class StorageAdapterSettingsProvider
        : IStorageAdapterSettingsProvider
    {
        /// <inheritdoc />
        public string EnumerationsStorageConnectionString
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable(
                    nameof(this.EnumerationsStorageConnectionString));

                return toReturn;
            }
        }

        /// <inheritdoc />
        public string EnumerationsStorageTableName
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable(
                    nameof(this.EnumerationsStorageTableName));

                return toReturn;
            }
        }
    }
}