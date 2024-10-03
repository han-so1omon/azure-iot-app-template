namespace IoTAgentSimulator.Models
{
    public class SystemLog
    {
        public string LogType { get; set; }  // e.g., "INFO", "WARNING", "ERROR"
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
