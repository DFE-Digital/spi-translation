using System.Data.Common;

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

    /// <summary>
    /// Implements <see cref="IAllEnumerationValuesResultStorageAdapter" />.
    /// </summary>
    public class AllEnumerationValuesResultStorageAdapter
        : StorageAdapterBase, IAllEnumerationValuesResultStorageAdapter
    {
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="AllEnumerationValuesResultStorageAdapter" /> class.
        /// </summary>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        /// <param name="allEnumerationsResultStorageAdapterSettingsProvider">
        /// An instance of type
        /// <see cref="IAllEnumerationValuesResultStorageAdapterSettingsProvider" />.
        /// </param>
        public AllEnumerationValuesResultStorageAdapter(
            ILoggerWrapper loggerWrapper, 
            IAllEnumerationValuesResultStorageAdapterSettingsProvider allEnumerationsResultStorageAdapterSettingsProvider) 
            : base(loggerWrapper, allEnumerationsResultStorageAdapterSettingsProvider)
        {
            this.loggerWrapper = loggerWrapper;
        }

        public async Task<AllEnumerationValuesResult> GetAllEnumerationValuesResultAsync(CancellationToken cancellationToken)
        {
            // Read all rows from table
            var allMappings = await base.GetEnumerationMappingsAsync(null, cancellationToken);
            
            // Get distinct list of enumeration names
            var enumerationNames = allMappings
                .Select(row => row.PartitionKey.Split('.')[1])
                .Distinct()
                .ToArray();
            
            // Build mappings
            var enumerations = new Dictionary<string, EnumerationValuesResult>();
            foreach (var enumerationName in enumerationNames)
            {
                var values = allMappings
                    .Where(x => x.PartitionKey.EndsWith(enumerationName, StringComparison.InvariantCulture))
                    .Select(x => x.RowKey)
                    .Distinct()
                    .ToArray();
                enumerations.Add(enumerationName, new EnumerationValuesResult
                {
                    EnumerationValues = values,
                });
            }
            
            // Let them have it
            return new AllEnumerationValuesResult
            {
                Enumerations = enumerations,
            };
        }
    }
}