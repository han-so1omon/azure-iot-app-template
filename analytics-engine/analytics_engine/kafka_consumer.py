import json
import time
from kafka import KafkaConsumer
from kafka.errors import NoBrokersAvailable
from sqlalchemy.orm import Session
from analytics_engine.database import SessionLocal
from analytics_engine.crud import create_reading

# Kafka configuration
KAFKA_BROKER = 'kafka:9092'
KAFKA_TEMPERATURE_TOPIC = 'temperature-readings'

def get_kafka_consumer():
    """Tries to create a Kafka consumer, waits and retries if the broker is not available."""
    max_retries = 10
    retry_delay = 5  # Start with a 5 second delay between retries

    for attempt in range(max_retries):
        try:
            # Initialize the Kafka consumer
            consumer = KafkaConsumer(
                KAFKA_TEMPERATURE_TOPIC,
                bootstrap_servers=KAFKA_BROKER,
                group_id=f'analytics-engine-group-{time.time()}',
                auto_offset_reset='earliest',  # Adjust based on your preference
                enable_auto_commit=True,
                value_deserializer=lambda x: json.loads(x.decode('utf-8'))
            )
            print("Kafka consumer created successfully.")
            return consumer
        except NoBrokersAvailable:
            print(f"Kafka broker not available. Retry {attempt + 1}/{max_retries} in {retry_delay} seconds...")
            time.sleep(retry_delay)
            retry_delay *= 2  # Exponential backoff
        except Exception as e:
            print(f"Error creating Kafka consumer: {e}")
            time.sleep(retry_delay)
            retry_delay *= 2

    raise Exception("Could not connect to Kafka after several retries.")

def consume_temperature_readings():
    # Get a Kafka consumer (retry logic is handled inside this function)
    consumer = get_kafka_consumer()

    print("Listening for messages...")
    # Continuously listen to Kafka messages
    for message in consumer:
        print(f"Received message: {message.value}")
        process_message(message.value)

def process_message(message):
    try:
        device = message['DeviceId']
        sensor = message['Name']
        status = message['Status']
        sensor_value = message['Temperature']
        is_online = message['IsOnline']
        sequence_number = message.get('sequence_number', 0)

        # Insert into the database
        with SessionLocal() as db:
            create_reading(db, device, sensor, sensor_value, sequence_number)

        print(f"Processed message from device: {device}, sensor: {sensor}, value: {sensor_value}")
    except Exception as e:
        print(f"Error processing message: {message}, {str(e)}")
