WIP: Note that this is a work in progress. For a production environment, optimize infrastructure specs,
API routes, data models, etc

# General

This is an application template for establishing an IoT application in Azure Cloud

## Requirements

- Docker + Docker Compose (^v3.8)
- Dotnet 8 SDK + Runtime (local build + test) 

## Usage

`docker compose up`

or to build fresh

`docker compose up --build`

## Components

- [IoT device](#iot-device)
- [Data store](#data-store)
- [Messaging service](#messaging-service)
- [Analytics engine](#analytics-engine)
- [IoT cloud controller](#iot-cloud-controller)
- [IoT agent simulator](#iot-agent-simulator)
- [Monitoring dashboard](#monitoring-dashboard)
- [Operator dashboard](#operator-dashboard)

### IoT Device

IoT device

- CSharp REST API server + MQTT client
- Device 1
    - Host: iot-device-1:8080, localhost:5002
- Device 2
    - Host: iot-device-2:8080, localhost:5003
- Device 1
    - Host: iot-device-3:8080, localhost:5004

### IoT Ingress Service

MQTT broker specs

- NodeJS MQTT server + Kafka broker
    - Host: iot-ingress-service:1883, localhost:1883
    - Same port for localhost

### Data Store

Data store specs

- PostgreSQL
    - Host: postgres-db:5432, localhost:5432
- MongoDB
    - Host: mongodb:27017, localhost 27017

### Messaging Service

Messaging service specs

- Apache Kafka
    - Host: kafka:9092, localhost:9092

### Analytics Engine

Analytics engine specs

- Python
- Constantly running + scheduled jobs
    - Host: analytics-engine:8000, localhost:7001

### IoT Cloud Controller

IoT cloud controller specs

- CSharp REST API server + Kafka consumer
    - Host: iot-cloud-controller:80, localhost:4001

### IoT Agent Simulator

IoT agent simulator specs

- CSharp REST API client

### Monitoring Dashboard

Monitoring dashboard specs

- React + Nivo charts
    - Host: localhost:3001
    - Recommend use ngrok or localtunnel

### Operator Dashboard

Operator dashboard specs
- React + Nivo charts
    - Host: localhost:3002
    - Recommend use ngrok or localtunnel

## Architecture

Architecture Diagram

![Azure IoT App Architecture](./media/Azure%20IoT%20App%20Architecture.jpg)

## Deployment

### Local Testing

`docker compose up`

### Azure Cloud

- Azure Resource Manager

## Gallery

![Monitoring Dashboard](./media/monitoring-dashboard.gif)
![Operator Dashboard](./media/operator-dashboard.gif)