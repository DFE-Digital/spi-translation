namespace Dfe.Spi.Translation.Infrastructure.AzureStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Translation.Domain.Definitions;
    using Dfe.Spi.Translation.Domain.Definitions.SettingsProviders;
    using Dfe.Spi.Translation.Domain.Models;
    using Dfe.Spi.Translation.Infrastructure.AzureStorage.Models;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Implements <see cref="IMappingsResultStorageAdapter" />.
    /// </summary>
    public class MappingsResultStorageAdapter : IMappingsResultStorageAdapter
    {
        private readonly ILoggerWrapper loggerWrapper;

        private readonly CloudTable cloudTable;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="MappingsResultStorageAdapter" /> class.
        /// </summary>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        /// <param name="mappingsResultStorageAdapterSettingsProvider">
        /// An instance of type
        /// <see cref="IMappingsResultStorageAdapterSettingsProvider" />.
        /// </param>
        public MappingsResultStorageAdapter(
            ILoggerWrapper loggerWrapper,
            IMappingsResultStorageAdapterSettingsProvider mappingsResultStorageAdapterSettingsProvider)
        {
            if (mappingsResultStorageAdapterSettingsProvider == null)
            {
                throw new ArgumentNullException(
                    nameof(mappingsResultStorageAdapterSettingsProvider));
            }

            this.loggerWrapper = loggerWrapper;

            string enumerationsStorageConnectionString =
                mappingsResultStorageAdapterSettingsProvider.EnumerationsStorageConnectionString;

            CloudStorageAccount cloudStorageAccount =
                CloudStorageAccount.Parse(enumerationsStorageConnectionString);

            CloudTableClient cloudTableClient =
                cloudStorageAccount.CreateCloudTableClient();

            string enumerationsStorageTableName =
                mappingsResultStorageAdapterSettingsProvider.EnumerationsStorageTableName;

            this.cloudTable = cloudTableClient.GetTableReference(
                enumerationsStorageTableName);
        }

        /// <inheritdoc />
        public async Task<MappingsResult> GetMappingsResultAsync(
            EnumerationsKey enumerationsKey,
            CancellationToken cancellationToken)
        {
            MappingsResult toReturn = null;

            if (enumerationsKey == null)
            {
                throw new ArgumentNullException(nameof(enumerationsKey));
            }

            string enumerationsKeyStr = enumerationsKey.ExportToString();

            TableQuery<EnumerationMapping> tableQuery =
                new TableQuery<EnumerationMapping>();

            string filter = TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                enumerationsKeyStr);

            tableQuery.Where(filter);

            IList<EnumerationMapping> enumerationMappings =
                await this.cloudTable.ExecuteQueryAsync(
                    tableQuery,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

            Dictionary<string, string> mappings = enumerationMappings
                .ToDictionary(x => x.RowKey, x => x.Mapping);

            toReturn = new MappingsResult()
            {
                Mappings = mappings,
            };

            return toReturn;
        }
    }
}