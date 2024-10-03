using Confluent.Kafka;
using MongoDB.Driver;
using YourNamespace.Models;
using YourNamespace.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace YourNamespace.Services
{
    public class KafkaConsumerService : IHostedService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly SystemLogsService _systemLogsService;

        public KafkaConsumerService(SystemLogsService systemLogsService)
        {
            _systemLogsService = systemLogsService;
            _consumer = CreateKafkaConsumer();  // Initialize the Kafka consumer
        }

        // Creates and configures a Kafka consumer using plaintext communication
        private IConsumer<Ignore, string> CreateKafkaConsumer()
        {
            var config = new ConsumerConfig
            {
                GroupId = $"iot-cloud-controller-group-{Guid.NewGuid().ToString().Substring(0, 8)}", // Random GroupId
                BootstrapServers = "kafka:9092",  // Kafka broker address (PLAINTEXT)
                AutoOffsetReset = AutoOffsetReset.Earliest, // Start consuming from the earliest message
                EnableAutoCommit = true, // Ensure offset auto-commit is enabled
                SecurityProtocol = SecurityProtocol.Plaintext, // Use plaintext communication
                SaslMechanism = SaslMechanism.Plain, // Use plain authentication
            };

            // Return a new Kafka consumer instance
            return new ConsumerBuilder<Ignore, string>(config).Build();
        }

        // Starts the Kafka consumer service
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => StartConsuming(cancellationToken), cancellationToken); // Run the consumer in the background
            return Task.CompletedTask;
        }

        // Consumes messages from Kafka
        private void StartConsuming(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("system-logs");  // Subscribe to the Kafka topic

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(cancellationToken); // Consume messages
                        if (consumeResult != null)
                        {
                            ProcessMessage(consumeResult.Message.Value);  // Process each message
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error consuming Kafka message: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();  // Close the consumer when cancelled
            }
        }

        // Processes the consumed Kafka message and stores it in MongoDB
        private void ProcessMessage(string message)
        {
            try
            {
                var log = new SystemLog
                {
                    LogType = "KafkaLog",  // Custom log type
                    Message = message,
                    Timestamp = DateTime.UtcNow
                };

                // Store the log in MongoDB
                _systemLogsService.CreateAsync(log).GetAwaiter().GetResult();
                Console.WriteLine($"Processed message: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing Kafka message: {ex.Message}");
            }
        }

        // Stops the Kafka consumer service
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Close(); // Gracefully stop the consumer
            return Task.CompletedTask;
        }
    }
}
