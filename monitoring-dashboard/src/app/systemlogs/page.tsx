"use client";

import React, { useEffect, useState } from 'react';
import { fetchSystemLogs } from '../../lib/api';

type SystemLog = {
  id: string;
  logType: string;
  message: string;
  timestamp: string;
};

const SystemLogsPage = () => {
  const [logs, setLogs] = useState<SystemLog[]>([]);

  useEffect(() => {
    const loadLogs = async () => {
      const logsData = await fetchSystemLogs();
      setLogs(logsData);
    };
    loadLogs();
  }, []);

  return (
    <div>
      <h1>System Logs</h1>
      <ul>
        {logs.map(log => (
          <li key={log.id}>
            <p><strong>{log.logType}</strong>: {log.message} at {new Date(log.timestamp).toLocaleString()}</p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default SystemLogsPage;
