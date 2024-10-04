"use client";

import { useEffect, useState } from "react";
import { ResponsiveBar } from '@nivo/bar';
import { ArrowTopRightOnSquareIcon } from "@heroicons/react/24/outline";

type SystemLog = {
  id: string;
  logType: string;
  message: string;
  timestamp: string;
};

type LogCounts = {
  Info: number;
  Warning: number;
  Error: number;
};

export default function MonitoringDashboard() {
  const [logs, setLogs] = useState<SystemLog[]>([]);
  const [logCounts, setLogCounts] = useState<LogCounts>({ Info: 0, Warning: 0, Error: 0 });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);

  // Fetch system logs and log counts every 5 seconds
  useEffect(() => {
    const fetchLogsAndCounts = async () => {
      try {
        const logsResponse = await fetch('/api/systemlogs');
        const logsData = await logsResponse.json();
        
        // Only keep the most recent 100 logs
        setLogs(logsData.slice(0, 100)); 

        const countsResponse = await fetch('/api/logcounts');
        const countsData = await countsResponse.json();
        setLogCounts(countsData);
      } catch (err) {
        console.error('Error fetching data:', err);
        setError(true);
      } finally {
        setLoading(false);
      }
    };

    const intervalId = setInterval(fetchLogsAndCounts, 3000); // Fetch every 5 seconds

    return () => clearInterval(intervalId); // Cleanup on component unmount
  }, []);

  const barChartData = [
    { logType: 'Info', count: logCounts.Info },
    { logType: 'Warning', count: logCounts.Warning },
    { logType: 'Error', count: logCounts.Error },
  ];

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
            <p className="text-red-500">Error loading data. Please try again later.</p>
          </div>
        ) : (
          <>
            {/* Log Counts Bar Chart Section */}
            <div className="mb-8">
              <h2 className="text-xl font-semibold mb-4">Recent Log Types</h2>
              <div style={{ height: 400 }}>
                <ResponsiveBar
                  data={barChartData}
                  keys={['count']}
                  indexBy="logType"
                  margin={{ top: 50, right: 130, bottom: 50, left: 60 }}
                  padding={0.3}
                  valueScale={{ type: 'linear' }}
                  indexScale={{ type: 'band', round: true }}
                  colors={({ indexValue }) => {
                    if (indexValue === 'Info') return '#1E3A8A';  // Blue for Info
                    if (indexValue === 'Warning') return '#ffbb00'; // Yellow for Warning
                    if (indexValue === 'Error') return '#d62728';   // Red for Error
                    return '#00ff00';  // Green color as fallback
                  }}
                  borderColor={{ from: 'color', modifiers: [['darker', 1.6]] }}
                  axisTop={null}
                  axisRight={null}
                  axisBottom={{
                    tickSize: 5,
                    tickPadding: 5,
                    tickRotation: 0,
                    legend: 'Log Type',
                    legendPosition: 'middle',
                    legendOffset: 32,
                    tickTextColor: '#ffffff',
                    style: {
                      fontSize: '16px', // Increase font size for better visibility
                    },
                  }}
                  axisLeft={{
                    tickSize: 5,
                    tickPadding: 5,
                    tickRotation: 0,
                    legend: 'Count',
                    legendPosition: 'middle',
                    legendOffset: -40,
                    tickTextColor: '#ffffff',
                    style: {
                      fontSize: '16px', // Increase font size for better visibility
                    },
                  }}
                  theme={{
                    labels: {
                      text: {
                        fontSize: '14px',
                        fill: '#ffffff',
                      },
                    },
                    axis: {
                      ticks: {
                        text: {
                          fontSize: '16px',
                          fill: '#ffffff',
                        },
                      },
                      legend: {
                        text: {
                          fontSize: '18px',
                          fill: '#ffffff',
                        },
                      },
                    },
                    tooltip: {
                      container: {
                        color: '#000000', // Set hover over text color to black
                      },
                    },
                  }}
                />
              </div>
            </div>

            {/* Console Section with Styled Logs */}
            <div className="mb-8">
              <h2 className="text-xl font-semibold mb-4">System Logs Console</h2>
              <div className="bg-indigo-950 text-white p-4 rounded-md space-y-2" style={{ maxHeight: '400px', overflowY: 'auto' }}>
                {logs.length > 0 ? (
                  <ul>
                    {logs.map((log) => (
                      <li key={log.id} className="border-b border-gray-600 pb-2">
                        <p style={{ color: log.message.includes("Error") ? 'yellow' : 'white' }}>
                          <strong>{log.logType}</strong>: {log.message}
                        </p>
                        <p className="text-sm text-gray-400">
                          {new Date(log.timestamp).toLocaleString()}
                        </p>
                      </li>
                    ))}
                  </ul>
                ) : (
                  <p className="text-white">No logs found.</p>
                )}
              </div>
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
