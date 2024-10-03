import asyncio
from fastapi import FastAPI
from analytics_engine.database import engine, Base
from analytics_engine.routes import router
from analytics_engine.kafka_consumer import consume_temperature_readings

app = FastAPI()

# Initialize the database tables
Base.metadata.create_all(bind=engine)

# Include routes
app.include_router(router)

# Kafka consumer task
async def kafka_consumer_task():
    while True:
        consume_temperature_readings()
        await asyncio.sleep(1)  # Small delay to allow other coroutines to run

@app.on_event("startup")
async def startup_event():
    loop = asyncio.get_event_loop()
    loop.create_task(kafka_consumer_task())
