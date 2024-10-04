import axios from 'axios';
import { NextResponse } from 'next/server';

export async function GET(req: Request, { params }: { params: { deviceId: string } }) {
  const { deviceId } = params;
  try {
    const response = await axios.get(`http://analytics-engine:8000/devices/${deviceId}/readings/`);
    return NextResponse.json(response.data);
  } catch (error) {
    console.error(`Error fetching readings for device ${deviceId}:`, error);
    return NextResponse.json([]);
  }
}
