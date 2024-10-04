"use client";

import { useState, useEffect, useRef } from "react";
import DeviceReadingsChart from "@/components/DeviceReadingsChart";

interface Reading {
  id: number;
  sensor: string;
  sequence_number: number;
  device: string;
  sensor_value: number;
  timestamp: string;
}

interface DashboardProps {
  devices: string[];
  initialReadings: Reading[];
}

// Helper function to sort devices by the numeric suffix
const sortDevicesByNumberSuffix = (devices: string[]) => {
  return devices.sort((a, b) => {
    const numA = parseInt(a.match(/\d+$/)?.[0] || "0", 10); // Extract number from the end of device name
    const numB = parseInt(b.match(/\d+$/)?.[0] || "0", 10); // Extract number from the end of device name
    return numA - numB;
  });
};

// Client Component to display the dashboard
export default function Dashboard({ devices, initialReadings }: DashboardProps) {
  // Sort the devices by the numeric suffix at the end of their names
  const sortedDevices = sortDevicesByNumberSuffix(devices);

  const [selectedDevice, setSelectedDevice] = useState<string>(sortedDevices.length ? sortedDevices[0] : '');
  const [readings, setReadings] = useState<Reading[]>(initialReadings);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(false);
  const chartDataRef = useRef<Reading[]>([]);

  const appendNewReadings = (newReadings: Reading[]) => {
    const currentReadings = chartDataRef.current;

    const updatedReadings = [
      ...currentReadings,
      ...newReadings.filter(
        (newReading) => !currentReadings.some((currentReading) => currentReading.id === newReading.id)
      ),
    ];

    setReadings(updatedReadings);
    chartDataRef.current = updatedReadings;
  };

  useEffect(() => {
    const loadReadings = async () => {
      setLoading(true);
      setError(false);
      try {
        const response = await fetch(`/api/readings/${selectedDevice}`);
        const data = await response.json();
        appendNewReadings(data);
      } catch (err) {
        setError(true);
        console.error('Error fetching device readings:', err);
      } finally {
        setLoading(false);
      }
    };

    if (selectedDevice) {
      loadReadings();
    }
  }, [selectedDevice]);

  useEffect(() => {
    const intervalId = setInterval(() => {
      if (selectedDevice) {
        fetch(`/api/readings/${selectedDevice}`)
          .then((res) => res.json())
          .then((newData) => appendNewReadings(newData))
          .catch(() => setError(true));
      }
    }, 3000);
    return () => clearInterval(intervalId);
  }, [selectedDevice]);

  const readingsData = [
    {
      id: selectedDevice,
      data: readings.map((reading: Reading) => ({
        x: new Date(reading.timestamp).getTime(),
        y: reading.sensor_value,
      })),
    },
  ];

  return (
    <div className="min-h-screen flex flex-col">
      <header className="bg-gray-800 text-white py-4 px-8 text-center">
        <h1 className="text-4xl font-bold">Operator Dashboard</h1>
      </header>

      <main className="flex-grow p-8 pb-20 sm:p-20">
        <>
          <div className="mb-8">
            <h2 className="text-xl font-semibold mb-4 text-center">Select Device</h2>
            <div className="flex justify-center">
              <select
                value={selectedDevice}
                onChange={(e) => setSelectedDevice(e.target.value)}
                className="bg-gray-900 text-white p-3 rounded-md shadow-md border border-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                {sortedDevices.map((device) => (
                  <option key={device} value={device}>
                    {device}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div>
            <h2 className="text-xl font-semibold mb-4">Device Readings</h2>
            {loading ? (
              <div>Loading...</div>
            ) : error ? (
              <div className="error">Error fetching data</div>
            ) : readings.length > 0 ? (
              <DeviceReadingsChart readingsData={readingsData} />
            ) : (
              <p>No readings available for this device</p>
            )}
          </div>
        </>
      </main>

      <footer className="bg-gray-800 text-white py-4 px-8 text-center">
        <p className="flex items-center justify-center gap-2">Azure IoT App Template</p>
      </footer>
    </div>
  );
}
