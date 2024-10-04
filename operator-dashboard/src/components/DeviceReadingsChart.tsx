"use client"; // Ensures this component is client-side

import { ResponsiveScatterPlot } from "@nivo/scatterplot";

type DeviceReading = {
  x: number; // Since we are using timestamps in milliseconds, the x-value should be a number
  y: number;
};

interface DeviceReadingsChartProps {
  readingsData: {
    id: string;
    data: DeviceReading[];
  }[];
}

export default function DeviceReadingsChart({ readingsData }: DeviceReadingsChartProps) {
  return (
    <div style={{ height: 400, backgroundColor: "white", padding: "20px", borderRadius: "8px" }}>
      <ResponsiveScatterPlot
        data={readingsData}
        margin={{ top: 50, right: 110, bottom: 50, left: 60 }}
        xScale={{ type: "linear", min: "auto", max: "auto" }}
        yScale={{ type: "linear", min: "auto", max: "auto" }}
        axisBottom={{
          tickSize: 5,
          tickPadding: 5,
          tickRotation: 0,
          legend: "Time (Unix Timestamp)", // Label for x-axis
          legendPosition: "middle",
          legendOffset: 46,
          format: (value) => new Date(value).toLocaleTimeString(), // Convert Unix timestamp to readable time
        }}
        axisLeft={{
          tickSize: 5,
          tickPadding: 5,
          tickRotation: 0,
          legend: "Temperature",
          legendPosition: "middle",
          legendOffset: -50,
        }}
        colors={{ scheme: "category10" }}  // High-contrast colors
        blendMode="multiply"
        nodeSize={10}
        theme={{
          axis: {
            ticks: {
              line: {
                stroke: "#555555", // Dark tick lines
              },
              text: {
                fill: "#333333", // Dark tick text
              },
            },
            legend: {
              text: {
                fill: "#333333", // Dark legend text
              },
            },
          },
          grid: {
            line: {
              stroke: "#dddddd", // Light grid lines for better contrast
              strokeWidth: 1,
            },
          },
        }}
        useMesh={true}
      />
    </div>
  );
}
