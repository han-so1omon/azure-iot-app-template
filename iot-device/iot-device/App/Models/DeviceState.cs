namespace IoTDeviceApp.Models
{
    public class DeviceState
    {
        public string DeviceId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public bool IsOnline { get; set; }
    }
}
