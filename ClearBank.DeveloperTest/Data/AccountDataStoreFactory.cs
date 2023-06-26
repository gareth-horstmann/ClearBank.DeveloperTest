using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ClearBank.DeveloperTest.Data;

/// <summary>
/// Factory class to construct IAccountDataStore instances
/// </summary>
/// <remarks>
///     The data store type is defined based on a configuration setting, which is read at system startup.
///     On a large majority of database SDK's it is recommended to reuse the database instances
///     to improve performance (for example, EF DataContext, MongoDB IMongoClient).  These would normally
///     be stored in the IoC container as a single instance so that they can be injected and reused through 
///     the application's lifetime.
///     
///     As this project does not have an IoC container, this class is constructing the correct IAccountDataStore
///     based on the configuration setting, and storing the instance for reuse later.
/// </remarks>
public class AccountDataStoreFactory : IAccountDataStoreFactory
{
    private readonly DatabaseConfiguration _databaseConfiguration;
    private readonly IMemoryCache _cache;

    public AccountDataStoreFactory(IOptions<DatabaseConfiguration> databaseConfiguration, IMemoryCache memoryCache)
    {
        _databaseConfiguration = databaseConfiguration.Value;
        _cache = memoryCache;
    }

    public IAccountDataStore Build()
    {
        if (!_cache.TryGetValue(nameof(IAccountDataStore), out IAccountDataStore dataStoreInstance))
        {
            // Key not in cache, so get data.
            dataStoreInstance = _databaseConfiguration.DataStoreType == DataStoreType.Backup
                ? new BackupAccountDataStore()
                : new AccountDataStore();

            var cacheEntryOptions = new MemoryCacheEntryOptions();

            _cache.Set(nameof(IAccountDataStore), dataStoreInstance, cacheEntryOptions);
        }

        return dataStoreInstance;
    }
}
