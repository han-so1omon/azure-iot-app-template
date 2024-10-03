from sqlalchemy.orm import Session
from sqlalchemy import func
from analytics_engine.models import DeviceReading

def get_average_sensor_value(db: Session, device: str, sensor: str):
    avg_value = db.query(func.avg(DeviceReading.sensor_value)) \
                  .filter(DeviceReading.device == device, DeviceReading.sensor == sensor) \
                  .scalar()
    return avg_value

def get_trends(db: Session, device: str, sensor: str):
    readings = db.query(DeviceReading) \
                 .filter(DeviceReading.device == device, DeviceReading.sensor == sensor) \
                 .order_by(DeviceReading.timestamp) \
                 .all()

    if len(readings) < 2:
        return "Insufficient data for trend analysis."

    trend = "increasing" if readings[-1].sensor_value > readings[0].sensor_value else "decreasing"
    return f"The sensor readings for {sensor} on device {device} are {trend}."
