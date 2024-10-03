import axios from 'axios';

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_BASE_URL,
});

export const fetchDevices = async () => {
  const response = await api.get('/devices');
  return response.data;
};

export const fetchDeviceStatus = async (deviceId: string) => {
  const response = await api.get(`/devices/${deviceId}/status`);
  return response.data;
};

export const fetchSystemLogs = async () => {
  const response = await api.get('/systemlogs');
  return response.data;
};
