"use client";

import React, { useEffect, useState } from 'react';
import { fetchDevices, fetchDeviceStatus } from '../../lib/api';

type Device = {
  id: string;
  name: string;
  type: string;
};

type DeviceStatus = {
  device: string;
  status: string;
  lastUpdated: string;
};

const DevicesPage = () => {
  const [devices, setDevices] = useState<Device[]>([]);
  const [selectedDevice, setSelectedDevice] = useState<Device | null>(null);
  const [deviceStatus, setDeviceStatus] = useState<DeviceStatus | null>(null);

  useEffect(() => {
    const loadDevices = async () => {
      const devicesData = await fetchDevices();
      setDevices(devicesData);
    };
    loadDevices();
  }, []);

  useEffect(() => {
    if (selectedDevice) {
      const loadDeviceStatus = async () => {
        const status = await fetchDeviceStatus(selectedDevice.id);
        setDeviceStatus(status);
      };
      loadDeviceStatus();
    }
  }, [selectedDevice]);

  return (
    <div>
      <h1>Devices</h1>
      <ul>
        {devices.map(device => (
          <li key={device.id}>
            <button onClick={() => setSelectedDevice(device)}>{device.name}</button>
          </li>
        ))}
      </ul>

      {selectedDevice && deviceStatus && (
        <div>
          <h2>Device Status for {selectedDevice.name}</h2>
          <p>Status: {deviceStatus.status}</p>
          <p>Last Updated: {deviceStatus.lastUpdated}</p>
        </div>
      )}
    </div>
  );
};

export default DevicesPage;
