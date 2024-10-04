import axios from "axios";
import DeviceReadingsChart from "@/components/DeviceReadingsChart";

// Server-side fetch function
async function fetchDevices() {
  const response = await axios.get("http://analytics-engine:8000/devices/");
  return response.data.devices || [];
}

// Server-side fetch function for device readings
async function fetchDeviceReadings(deviceId: string) {
  const response = await axios.get(`http://analytics-engine:8000/devices/${deviceId}/readings/`);
  return response.data || [];
}

export default async function Dashboard() {
  // Fetch devices on the server side
  const devices = await fetchDevices();
  const selectedDevice = devices.length ? devices[0] : "";
  const readings = selectedDevice ? await fetchDeviceReadings(selectedDevice) : [];

  // Prepare data for Nivo chart
  const readingsData = [
    {
      id: selectedDevice,
      data: readings.map((reading: any) => ({
        x: new Date(reading.timestamp).toLocaleString(),
        y: reading.sensor_value,
      })),
    },
  ];

  return (
    <div className="min-h-screen flex flex-col">
      {/* Header */}
      <header className="bg-gray-800 text-white py-4 px-8 text-center">
        <h1 className="text-4xl font-bold">Operator Dashboard</h1>
      </header>

      {/* Main Content */}
      <main className="flex-grow p-8 pb-20 sm:p-20">
        <>
          {/* Device Selector */}
          <div className="mb-8">
            <h2 className="text-xl font-semibold mb-4">Select Device</h2>
            <select className="bg-gray-200 p-2 rounded-md">
              {devices.map((device) => (
                <option key={device} value={device}>
                  {device}
                </option>
              ))}
            </select>
          </div>

          {/* Device Readings Section */}
          <div>
            <h2 className="text-xl font-semibold mb-4">Device Readings</h2>
            {readings.length > 0 ? (
              <DeviceReadingsChart readingsData={readingsData} />
            ) : (
              <p>No readings available for this device</p>
            )}
          </div>
        </>
      </main>

      {/* Footer */}
      <footer className="bg-gray-800 text-white py-4 px-8 text-center">
        <p className="flex items-center justify-center gap-2">
          Azure IoT App Template
          <a
            href="https://github.com/han-so1omon/azure-iot-app-template"
            target="_blank"
            rel="noopener noreferrer"
            className="flex items-center hover:text-blue-400"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
              className="w-5 h-5 inline-block"
            >
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 9V7a5 5 0 00-10 0v2a7 7 0 00-7 7v2a1 1 0 001 1h18a1 1 0 001-1v-2a7 7 0 00-7-7z" />
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M7 13l3 3 7-7" />
            </svg>
          </a>
        </p>
      </footer>
    </div>
  );
}
