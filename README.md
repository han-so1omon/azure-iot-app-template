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

IoT device specs

- CSharp

### IoT Ingress Service

MQTT broker

- NodeJS

### Data Store

Data store specs

- Azure Database for PostgreSQL

### Messaging Service

Messaging service specs

- Apache Kafka

### Analytics Engine

Analytics engine specs

- Python
- Constantly running + scheduled jobs

### IoT Cloud Controller

IoT cloud controller specs

- CSharp

### IoT Agent Simulator

IoT agent simulator specs

- CSharp

### Monitoring Dashboard

Monitoring dashboard specs

- React

### Operator Dashboard

Operator dashboard specs

## Architecture

Architecture Diagram

![Azure IoT App Architecture](./Azure%20IoT%20App%20Architecture.jpg)

## Deployment

### Local Testing

`docker compose up`

### Azure Cloud

- Azure Resource Manager