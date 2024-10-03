namespace IoTAgentSimulator.Models
{
    public class DeviceState
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }  // e.g., "active", "inactive", "error"
        public double Temperature { get; set; }  // Example sensor value
        public bool IsOnline { get; set; }
    }
}
