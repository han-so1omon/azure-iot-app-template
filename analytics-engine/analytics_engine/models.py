from sqlalchemy import Column, Integer, String, Float, DateTime
from sqlalchemy.sql import func
from analytics_engine.database import Base

class DeviceReading(Base):
    __tablename__ = "device_readings"

    id = Column(Integer, primary_key=True, index=True)
    timestamp = Column(DateTime(timezone=True), server_default=func.now())
    device = Column(String, index=True)
    sensor = Column(String, index=True)
    sensor_value = Column(Float)
    sequence_number = Column(Integer)
