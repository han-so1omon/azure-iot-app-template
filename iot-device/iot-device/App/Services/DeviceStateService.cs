using IoTDeviceApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace IoTDeviceApp.Services
{
    public class DeviceStateService
    {
        private readonly List<DeviceState> _devices = new List<DeviceState>();

        public DeviceStateService()
        {

        }

        public IEnumerable<DeviceState> GetAllDevices() => _devices;

        public DeviceState? GetDevice(string deviceId) => _devices.FirstOrDefault(d => d.DeviceId == deviceId);

        public void UpdateDeviceState(string deviceId, DeviceState updatedState)
        {
            var device = _devices.FirstOrDefault(d => d.DeviceId == deviceId);
            if (device != null)
            {
                device.Name = updatedState.Name;
                device.Status = updatedState.Status;
                device.Temperature = updatedState.Temperature;
                device.IsOnline = updatedState.IsOnline;
            }
        }

        public void AddDevice(DeviceState newDevice) => _devices.Add(newDevice);

        public void DeleteDevice(string deviceId)
        {
            var device = _devices.FirstOrDefault(d => d.DeviceId == deviceId);
            if (device != null)
            {
                _devices.Remove(device);
            }
        }
    }
}
