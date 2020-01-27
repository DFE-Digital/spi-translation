namespace Dfe.Spi.Translation.Infrastructure.AzureStorage
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Translation.Domain.Definitions.SettingsProviders;
    using Dfe.Spi.Translation.Domain.Models;
    using Dfe.Spi.Translation.Infrastructure.AzureStorage.Models;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Base class for all storage adapters.
    /// </summary>
    public abstract class StorageAdapterBase
    {
        private readonly ILoggerWrapper loggerWrapper;

        private readonly CloudTable cloudTable;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="StorageAdapterBase" /> class.
        /// </summary>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        /// <param name="storageAdapterSettingsProvider">
        /// An instance of type <see cref="IStorageAdapterSettingsProvider" />.
        /// </param>
        public StorageAdapterBase(
            ILoggerWrapper loggerWrapper,
            IStorageAdapterSettingsProvider storageAdapterSettingsProvider)
        {
            if (storageAdapterSettingsProvider == null)
            {
                throw new ArgumentNullException(
                    nameof(storageAdapterSettingsProvider));
            }

            this.loggerWrapper = loggerWrapper;

            string enumerationsStorageConnectionString =
                storageAdapterSettingsProvider.EnumerationsStorageConnectionString;

            CloudStorageAccount cloudStorageAccount =
                CloudStorageAccount.Parse(enumerationsStorageConnectionString);

            CloudTableClient cloudTableClient =
                cloudStorageAccount.CreateCloudTableClient();

            string enumerationsStorageTableName =
                storageAdapterSettingsProvider.EnumerationsStorageTableName;

            this.cloudTable = cloudTableClient.GetTableReference(
                enumerationsStorageTableName);
        }

        /// <summary>
        /// Gets <see cref="EnumerationMapping" />s from the storage table.
        /// </summary>
        /// <param name="enumerationsKey">
        /// An optional <see cref="EnumerationKey" />. If null, all rows
        /// are returned.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IList{EnumerationMapping}" />.
        /// </returns>
        protected async Task<IList<EnumerationMapping>> GetEnumerationMappingsAsync(
            EnumerationKey enumerationsKey,
            CancellationToken cancellationToken)
        {
            IList<EnumerationMapping> toReturn = null;

            TableQuery<EnumerationMapping> tableQuery =
                    new TableQuery<EnumerationMapping>();

            if (enumerationsKey != null)
            {
                string enumerationsKeyStr = enumerationsKey.ExportToString();

                string filter = TableQuery.GenerateFilterCondition(
                    "PartitionKey",
                    QueryComparisons.Equal,
                    enumerationsKeyStr);

                this.loggerWrapper.Debug(
                    $"The following filter will be used to get " +
                    $"{nameof(EnumerationMapping)} instance(s) from " +
                    $"table storage: \"{filter}\".");

                tableQuery.Where(filter);
            }

            this.loggerWrapper.Debug(
                $"Getting {nameof(EnumerationMapping)} instance(s) from " +
                $"table storage...");

            toReturn =
                await this.cloudTable.ExecuteQueryAsync(
                    tableQuery,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

            this.loggerWrapper.Info(
                $"{toReturn.Count} {nameof(EnumerationMapping)} instance(s) " +
                $"returned.");

            return toReturn;
        }
    }
}