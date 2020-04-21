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
    using Newtonsoft.Json;

    /// <summary>
    /// Implements <see cref="IMappingsResultStorageAdapter" />.
    /// </summary>
    public class MappingsResultStorageAdapter
        : StorageAdapterBase, IMappingsResultStorageAdapter
    {
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
            : base(
                  loggerWrapper,
                  mappingsResultStorageAdapterSettingsProvider)
        {
            // Nothing.
        }

        
        /// <inheritdoc />
        public async Task<AdapterMappingsResult> GetAdapterMappingsResultAsync(
            string adapterName, 
            CancellationToken cancellationToken)
        {
            // Check the request
            if (adapterName == null)
            {
                throw new ArgumentNullException(nameof(adapterName));
            }
            
            // Read all rows from table
            var allMappings = await base.GetEnumerationMappingsAsync(null, cancellationToken);
            
            // Get mappings for specified adapter
            var adapterMappings = allMappings
                .Where(row => row.PartitionKey.StartsWith(adapterName, StringComparison.InvariantCultureIgnoreCase))
                .Select(row =>
                    new
                    {
                        EnumerationName = row.PartitionKey.Substring(adapterName.Length + 1),
                        EnumerationValue = row.RowKey,
                        Mappings = MappingToArray(row.Mapping),
                    })
                .GroupBy(row => row.EnumerationName)
                .ToDictionary(
                    projectedRow => projectedRow.Key,
                    projectedRow =>
                        new MappingsResult
                        {
                            Mappings = projectedRow.ToDictionary(
                                value => value.EnumerationValue,
                                value => value.Mappings),
                        });

            // Let them have it
            return new AdapterMappingsResult
            {
                EnumerationMappings = adapterMappings,
            };
        }

        /// <inheritdoc />
        public async Task<MappingsResult> GetMappingsResultAsync(
            EnumerationKey enumerationsKey,
            CancellationToken cancellationToken)
        {
            MappingsResult toReturn = null;

            if (enumerationsKey == null)
            {
                throw new ArgumentNullException(nameof(enumerationsKey));
            }

            IEnumerable<EnumerationMapping> enumerationMappings =
                await this.GetEnumerationMappingsAsync(
                    enumerationsKey,
                    cancellationToken)
                    .ConfigureAwait(false);

            Dictionary<string, string[]> mappings = enumerationMappings
                .ToDictionary(
                x => x.RowKey,
                x => MappingToArray(x.Mapping));

            toReturn = new MappingsResult()
            {
                Mappings = mappings,
            };

            return toReturn;
        }

        private static string[] MappingToArray(string mappingStr)
        {
            string[] toReturn = null;

            toReturn = JsonConvert.DeserializeObject<string[]>(mappingStr);

            return toReturn;
        }
    }
}