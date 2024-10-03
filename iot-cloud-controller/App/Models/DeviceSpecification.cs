using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace YourNamespace.Models
{
    public class DeviceSpecification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Make this nullable

        public string DeviceName { get; set; } = string.Empty; // Initialize as empty
        public string DeviceType { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
    }
}
