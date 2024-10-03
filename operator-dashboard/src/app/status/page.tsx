"use client";

import React, { useEffect, useState } from 'react';
import { fetchDeviceStatus } from '../../lib/analyticsApi';

type DeviceStatus = {
  device: string;
  status: string;
  lastUpdated: string;
};

const StatusPage = () => {
  const [deviceId] = useState('device1');
  const [deviceStatus, setDeviceStatus] = useState<DeviceStatus | null>(null);

  useEffect(() => {
    const loadStatus = async () => {
      const status = await fetchDeviceStatus(deviceId);
      setDeviceStatus(status);
    };
    loadStatus();
  }, [deviceId]);

  return (
    <div>
      <h1>Device Status</h1>
      {deviceStatus ? (
        <div>
          <p>Device: {deviceStatus.device}</p>
          <p>Status: {deviceStatus.status}</p>
          <p>Last Updated: {new Date(deviceStatus.lastUpdated).toLocaleString()}</p>
        </div>
      ) : (
        <p>Loading...</p>
      )}
    </div>
  );
};

export default StatusPage;
