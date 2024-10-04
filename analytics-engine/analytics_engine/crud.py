from sqlalchemy.orm import Session
from analytics_engine.models import DeviceReading

# Get all readings with pagination
def get_readings(db: Session, skip: int = 0, limit: int = 10):
    return db.query(DeviceReading).offset(skip).limit(limit).all()

# Get a reading by ID
def get_reading_by_id(db: Session, reading_id: int):
    return db.query(DeviceReading).filter(DeviceReading.id == reading_id).first()

# Create a new reading
def create_reading(db: Session, device: str, sensor: str, sensor_value: float, sequence_number: int):
    reading = DeviceReading(device=device, sensor=sensor, sensor_value=sensor_value, sequence_number=sequence_number)
    db.add(reading)
    db.commit()
    db.refresh(reading)
    return reading

# Update an existing reading
def update_reading(db: Session, reading_id: int, sensor_value: float):
    reading = db.query(DeviceReading).filter(DeviceReading.id == reading_id).first()
    if reading:
        reading.sensor_value = sensor_value
        db.commit()
        db.refresh(reading)
    return reading

# Delete a reading by ID
def delete_reading(db: Session, reading_id: int):
    reading = db.query(DeviceReading).filter(DeviceReading.id == reading_id).first()
    if reading:
        db.delete(reading)
        db.commit()
    return reading

# Get all unique device IDs, returning a list of strings instead of tuples
def get_all_device_ids(db: Session):
    device_ids = db.query(DeviceReading.device).distinct().all()
    # Flatten the list of tuples into a list of device IDs
    return [device_id[0] for device_id in device_ids]

# Get all readings for a specific device
def get_readings_by_device(db: Session, device_id: str):
    return db.query(DeviceReading).filter(DeviceReading.device == device_id).all()
