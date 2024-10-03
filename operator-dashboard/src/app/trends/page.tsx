"use client";

import React, { useEffect, useState } from 'react';
import { fetchDeviceTrends } from '../../lib/analyticsApi';

type Trend = {
  device: string;
  name: string;
  timestamp: string;
  value: number;
};

const TrendsPage = () => {
  const [deviceId] = useState('device1');
  const [sensor] = useState('temperature');
  const [trends, setTrends] = useState<Trend[]>([]);

  useEffect(() => {
    const loadTrends = async () => {
      const trendData = await fetchDeviceTrends(deviceId, sensor);
      setTrends(trendData);
    };
    loadTrends();
  }, [deviceId, sensor]);

  return (
    <div>
      <h1>Device Trends</h1>
      <ul>
        {trends.map((trend, index) => (
          <li key={index}>
            <p>Time: {new Date(trend.timestamp).toLocaleString()}</p>
            <p>Value: {trend.value}</p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TrendsPage;
