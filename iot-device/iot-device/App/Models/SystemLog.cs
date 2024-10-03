using System;

namespace IoTDeviceApp.Models
{
    public class SystemLog
    {
        public string LogType { get; set; } = string.Empty;   // Initialized with default value
        public string Message { get; set; } = string.Empty;   // Initialized with default value
        public DateTime Timestamp { get; set; }
    }
}
