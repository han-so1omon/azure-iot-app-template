from sqlalchemy.orm import Session
from analytics_engine.models import DeviceReading

def get_readings(db: Session, skip: int = 0, limit: int = 10):
    return db.query(DeviceReading).offset(skip).limit(limit).all()

def get_reading_by_id(db: Session, reading_id: int):
    return db.query(DeviceReading).filter(DeviceReading.id == reading_id).first()

def create_reading(db: Session, device: str, sensor: str, sensor_value: float, sequence_number: int):
    reading = DeviceReading(device=device, sensor=sensor, sensor_value=sensor_value, sequence_number=sequence_number)
    db.add(reading)
    db.commit()
    db.refresh(reading)
    return reading

def update_reading(db: Session, reading_id: int, sensor_value: float):
    reading = db.query(DeviceReading).filter(DeviceReading.id == reading_id).first()
    if reading:
        reading.sensor_value = sensor_value
        db.commit()
        db.refresh(reading)
    return reading

def delete_reading(db: Session, reading_id: int):
    reading = db.query(DeviceReading).filter(DeviceReading.id == reading_id).first()
    if reading:
        db.delete(reading)
        db.commit()
    return reading
