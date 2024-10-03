import axios from 'axios';

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_ANALYTICS_API_BASE_URL,
});

export const fetchDeviceStatus = async (deviceId: string) => {
  const response = await api.get(`/analytics/status/${deviceId}`);
  return response.data;
};

export const fetchDeviceTrends = async (deviceId: string, sensor: string) => {
  const response = await api.get(`/analytics/trends?device=${deviceId}&sensor=${sensor}`);
  return response.data;
};
