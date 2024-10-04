"use client"; // Ensures this component is client-side

import { ResponsiveLine } from "@nivo/line";

type DeviceReading = {
  x: string;
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
    <div style={{ height: 400 }}>
      <ResponsiveLine
        data={readingsData}
        margin={{ top: 50, right: 110, bottom: 50, left: 60 }}
        xScale={{ type: "point" }}
        yScale={{ type: "linear", min: "auto", max: "auto", stacked: true, reverse: false }}
        axisLeft={{
          legend: "Value",
          legendOffset: -40,
          legendPosition: "middle",
        }}
        axisBottom={{
          legend: "Time",
          legendOffset: 36,
          legendPosition: "middle",
        }}
        colors={{ scheme: "nivo" }}
        lineWidth={3}
        pointSize={10}
        pointBorderWidth={2}
        pointBorderColor={{ from: "serieColor" }}
        pointLabelYOffset={-12}
        useMesh={true}
        enableSlices="x"
      />
    </div>
  );
}
