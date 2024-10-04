"use client";

import { useEffect, useState } from "react";
import { fetchSystemLogs } from "@/lib/api";
import { ArrowTopRightOnSquareIcon } from "@heroicons/react/24/outline";

type SystemLog = {
  id: string;
  logType: string;
  message: string;
  timestamp: string;
};

export default function MonitoringDashboard() {
  const [logs, setLogs] = useState<SystemLog[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);

  useEffect(() => {
    const loadSystemLogs = async () => {
      try {
        const logsData = await fetchSystemLogs();
        setLogs(logsData);
      } catch (err) {
        console.error("Error fetching system logs", err);
        setError(true);
      } finally {
        setLoading(false);
      }
    };

    loadSystemLogs();
  }, []);

  return (
    <div className="min-h-screen flex flex-col">
      {/* Header */}
      <header className="bg-gray-800 text-white py-4 px-8 text-center">
        <h1 className="text-4xl font-bold">Monitoring Dashboard</h1>
      </header>

      {/* Main Content */}
      <main className="flex-grow p-8 pb-20 sm:p-20">
        {loading ? (
          <div className="flex items-center justify-center min-h-[60vh]">
            <div className="w-16 h-16 border-4 border-dashed rounded-full animate-spin border-blue-500"></div>
          </div>
        ) : error ? (
          <div className="flex items-center justify-center min-h-[60vh]">
            <p className="text-red-500">Error loading system logs. Please try again later.</p>
          </div>
        ) : (
          <>
            {/* System Logs Section */}
            <div className="mb-8">
              <h2 className="text-xl font-semibold mb-4">System Logs</h2>
              {logs.length > 0 ? (
                <ul className="bg-gray-100 p-4 rounded-md space-y-2">
                  {logs.map((log) => (
                    <li key={log.id} className="border-b border-gray-200 pb-2">
                      <p><strong>{log.logType}</strong>: {log.message}</p>
                      <p className="text-sm text-gray-500">
                        {new Date(log.timestamp).toLocaleString()}
                      </p>
                    </li>
                  ))}
                </ul>
              ) : (
                <p>No logs found.</p>
              )}
            </div>
          </>
        )}
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
            <ArrowTopRightOnSquareIcon className="w-5 h-5 inline-block" />
          </a>
        </p>
      </footer>
    </div>
  );
}
