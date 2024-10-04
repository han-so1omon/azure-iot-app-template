import asyncio
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from analytics_engine.database import engine, Base
from analytics_engine.routes import router
from analytics_engine.kafka_consumer import consume_temperature_readings

app = FastAPI()

# Initialize the database tables
Base.metadata.create_all(bind=engine)

# Include routes
app.include_router(router)

# Enable CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"], 
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Kafka consumer task
async def kafka_consumer_task():
    while True:
        await asyncio.to_thread(consume_temperature_readings)
        await asyncio.sleep(1)

@app.on_event("startup")
async def startup_event():
    loop = asyncio.get_event_loop()
    loop.create_task(kafka_consumer_task())
