using MongoDB.Driver;
using YourNamespace.Models;
using Microsoft.Extensions.Options;

namespace YourNamespace.Services
{
    public class SystemLogsService
    {
        private readonly IMongoCollection<SystemLog> _systemLogsCollection;

        public SystemLogsService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _systemLogsCollection = mongoDatabase.GetCollection<SystemLog>("SystemLogs");
        }

        public async Task<List<SystemLog>> GetAsync() => 
            await _systemLogsCollection.Find(_ => true).ToListAsync();

        public async Task<SystemLog?> GetAsync(string id) => 
            await _systemLogsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(SystemLog newSystemLog) =>
            await _systemLogsCollection.InsertOneAsync(newSystemLog);

        public async Task UpdateAsync(string id, SystemLog updatedSystemLog) =>
            await _systemLogsCollection.ReplaceOneAsync(x => x.Id == id, updatedSystemLog);

        public async Task RemoveAsync(string id) =>
            await _systemLogsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
