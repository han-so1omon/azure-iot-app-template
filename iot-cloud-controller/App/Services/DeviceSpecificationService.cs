using MongoDB.Driver;
using YourNamespace.Models;
using Microsoft.Extensions.Options;

namespace YourNamespace.Services
{
    public class DeviceSpecificationsService
    {
        private readonly IMongoCollection<DeviceSpecification> _deviceSpecificationsCollection;

        public DeviceSpecificationsService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _deviceSpecificationsCollection = mongoDatabase.GetCollection<DeviceSpecification>("DeviceSpecifications");
        }

        public async Task<List<DeviceSpecification>> GetAsync() => 
            await _deviceSpecificationsCollection.Find(_ => true).ToListAsync();

        public async Task<DeviceSpecification?> GetAsync(string id) => 
            await _deviceSpecificationsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(DeviceSpecification newDeviceSpecification) =>
            await _deviceSpecificationsCollection.InsertOneAsync(newDeviceSpecification);

        public async Task UpdateAsync(string id, DeviceSpecification updatedDeviceSpecification) =>
            await _deviceSpecificationsCollection.ReplaceOneAsync(x => x.Id == id, updatedDeviceSpecification);

        public async Task RemoveAsync(string id) =>
            await _deviceSpecificationsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
