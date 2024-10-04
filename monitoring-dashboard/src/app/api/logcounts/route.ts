// app/api/logcounts/route.ts
import { NextResponse } from 'next/server';
import axios from 'axios';

const API_BASE_URL = 'http://iot-cloud-controller:8080/api';

export async function GET() {
  try {
    const response = await axios.get(`${API_BASE_URL}/systemlogs/log-count`);
    return NextResponse.json(response.data);
  } catch (error) {
    console.error('Error fetching log counts:', error);
    return NextResponse.error();
  }
}
