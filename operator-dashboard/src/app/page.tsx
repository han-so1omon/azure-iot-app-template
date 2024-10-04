import axios from 'axios';
import Dashboard from '@/components/Dashboard';

// Fetch devices (Server-Side)
async function fetchDevices() {
  try {
    const response = await axios.get('http://analytics-engine:8000/devices/');
    return response.data.devices || [];
  } catch (error) {
    console.error('Error fetching devices:', error);
    return [];
  }
}

// Fetch readings for a specific device (Server-Side)
async function fetchDeviceReadings(deviceId: string) {
  try {
    const response = await axios.get(`http://analytics-engine:8000/devices/${deviceId}/readings/`);
    return response.data || [];
  } catch (error) {
    console.error('Error fetching device readings:', error);
    return [];
  }
}

export default async function Page() {
  // Fetch devices and initial readings server-side
  const devices = await fetchDevices();
  const selectedDevice = devices.length ? devices[0] : '';
  const initialReadings = selectedDevice ? await fetchDeviceReadings(selectedDevice) : [];

  // Pass data as props to the client-side Dashboard
  return <Dashboard devices={devices} initialReadings={initialReadings} />;
}
