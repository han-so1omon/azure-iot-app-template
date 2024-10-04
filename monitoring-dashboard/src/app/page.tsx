// app/page.tsx (Server Component)
import MonitoringDashboard from "@/components/MonitoringDashboard";

// Server-side entry point for the Monitoring Dashboard
export default function Page() {
  return (
    <>
      {/* Monitoring Dashboard Client-Side Component */}
      <MonitoringDashboard />
    </>
  );
}
