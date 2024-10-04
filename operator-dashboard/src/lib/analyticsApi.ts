import axios from 'axios';

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_ANALYTICS_API_BASE_URL,
});

// Fetch all device IDs
export const fetchAllDevices = async () => {
  const response = await api.get("/devices/");
  return response.data;
};

// Fetch all readings by device ID
export const fetchDeviceReadings = async (deviceId: string) => {
  const response = await api.get(`/devices/${deviceId}/readings/`);
  return response.data;
};