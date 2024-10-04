from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from analytics_engine.database import get_db
from analytics_engine import crud, analytics

router = APIRouter()

@router.get("/readings/")
def read_readings(skip: int = 0, limit: int = 10, db: Session = Depends(get_db)):
    readings = crud.get_readings(db, skip=skip, limit=limit)
    return readings

@router.post("/readings/")
def create_reading(device: str, sensor: str, sensor_value: float, sequence_number: int, db: Session = Depends(get_db)):
    return crud.create_reading(db, device=device, sensor=sensor, sensor_value=sensor_value, sequence_number=sequence_number)

@router.put("/readings/{reading_id}")
def update_reading(reading_id: int, sensor_value: float, db: Session = Depends(get_db)):
    return crud.update_reading(db, reading_id=reading_id, sensor_value=sensor_value)

@router.delete("/readings/{reading_id}")
def delete_reading(reading_id: int, db: Session = Depends(get_db)):
    return crud.delete_reading(db, reading_id=reading_id)

@router.get("/analytics/average/")
def average_sensor_value(device: str, sensor: str, db: Session = Depends(get_db)):
    avg_value = analytics.get_average_sensor_value(db, device, sensor)
    if avg_value is None:
        raise HTTPException(status_code=404, detail="No data found for this device and sensor.")
    return {"device": device, "sensor": sensor, "average_value": avg_value}

@router.get("/analytics/trends/")
def sensor_trends(device: str, sensor: str, db: Session = Depends(get_db)):
    return analytics.get_trends(db, device=device, sensor=sensor)

# Route to get all unique device IDs
@router.get("/devices/")
def get_all_devices(db: Session = Depends(get_db)):
    devices = crud.get_all_device_ids(db)
    return {"devices": devices}

# Route to get all readings for a specific device by ID
@router.get("/devices/{device_id}/readings/")
def get_readings_by_device(device_id: str, db: Session = Depends(get_db)):
    readings = crud.get_readings_by_device(db, device_id=device_id)
    if not readings:
        raise HTTPException(status_code=404, detail="No readings found for this device")
    return readings
