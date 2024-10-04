using MongoDB.Driver;
using YourNamespace.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNamespace.Services
{
    public class SystemLogsService
    {
        private readonly IMongoCollection<SystemLog> _systemLogsCollection;

        public SystemLogsService(IOptions<DatabaseSettings> databaseSettings)
        {
            // Create a MongoClient with proper credentials
            var settings = MongoClientSettings.FromConnectionString(databaseSettings.Value.ConnectionString);

            var mongoClient = new MongoClient(settings);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _systemLogsCollection = mongoDatabase.GetCollection<SystemLog>("SystemLogs");
        }

        // Get logs from the past 10 minutes and limit to 100 logs
        public async Task<List<SystemLog>> GetRecentLogsAsync()
        {
            // Calculate the time 10 minutes ago
            var tenMinutesAgo = DateTime.UtcNow.AddMinutes(-10);

            // Build the filter to retrieve logs from the last 10 minutes
            var filter = Builders<SystemLog>.Filter.Gte(log => log.Timestamp, tenMinutesAgo);

            // Retrieve logs with the filter and limit the result to 100 entries
            return await _systemLogsCollection.Find(filter)
                                              .SortByDescending(log => log.Timestamp)  // Sort by timestamp in descending order
                                              .Limit(100)  // Limit the result to 100 logs
                                              .ToListAsync();
        }

        // Get the count of logs with types INFO, WARNING, or ERROR from the past 10 minutes
        public async Task<Dictionary<string, int>> GetLogCountByTypeAsync()
        {
            var recentLogs = await GetRecentLogsAsync();

            // Initialize counters for each log type
            int infoCount = 0;
            int warningCount = 0;
            int errorCount = 0;

            foreach (var log in recentLogs)
            {
                try
                {
                    var parsedMessage = JObject.Parse(log.Message);
                    var logType = parsedMessage["LogType"]?.ToString();

                    // Increment counters based on log type
                    if (logType == "Info")
                    {
                        infoCount++;
                    }
                    else if (logType == "Warning")
                    {
                        warningCount++;
                    }
                    else if (logType == "Error")
                    {
                        errorCount++;
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle any parsing errors
                    Console.WriteLine($"Error parsing log message: {ex.Message}");
                }
            }

            // Return a dictionary with the counts for each log type
            var result = new Dictionary<string, int>
            {
                { "Info", infoCount },
                { "Warning", warningCount },
                { "Error", errorCount }
            };

            Console.WriteLine($"Getting log count by type: {result}");

            return result;
        }

        // Get a specific log by its ID
        public async Task<SystemLog?> GetAsync(string id) =>
            await _systemLogsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Create a new log
        public async Task CreateAsync(SystemLog newSystemLog) =>
            await _systemLogsCollection.InsertOneAsync(newSystemLog);

        // Update an existing log by its ID
        public async Task UpdateAsync(string id, SystemLog updatedSystemLog) =>
            await _systemLogsCollection.ReplaceOneAsync(x => x.Id == id, updatedSystemLog);

        // Remove a log by its ID
        public async Task RemoveAsync(string id) =>
            await _systemLogsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
