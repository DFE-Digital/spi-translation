namespace Dfe.Spi.Translation.Domain.Definitions.SettingsProviders
{
    /// <summary>
    /// Describes the operations of the storage adapter settings provider.
    /// </summary>
    public interface IStorageAdapterSettingsProvider
    {
        /// <summary>
        /// Gets the connection string to the stroage account hosting the table
        /// storage which details the enumerations.
        /// </summary>
        string EnumerationsStorageConnectionString
        {
            get;
        }

        /// <summary>
        /// Gets the table name holding the details of enumerations.
        /// </summary>
        string EnumerationsStorageTableName
        {
            get;
        }
    }
}