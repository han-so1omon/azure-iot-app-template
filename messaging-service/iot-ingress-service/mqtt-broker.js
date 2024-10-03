// mqtt-broker.js
const aedes = require('aedes')();
const net = require('net');
const kafka = require('kafka-node');

// Define the port for the MQTT broker
const MQTT_PORT = 1883;

// Kafka configuration
const KAFKA_BROKER = 'kafka:9092'; // Replace with your Kafka broker address
const KAFKA_TEMPERATURE_TOPIC = 'temperature-readings'; // Kafka topic for temperature readings
const KAFKA_LOGS_TOPIC = 'system-logs'; // Kafka topic for system logs

// Retry logic variables
const MAX_RETRIES = 10;
let retryAttempts = 0;
let kafkaProducer;

// Function to initialize Kafka producer with retry logic
function initializeKafkaProducer() {
    return new Promise((resolve, reject) => {
        const kafkaClient = new kafka.KafkaClient({ kafkaHost: KAFKA_BROKER });
        kafkaProducer = new kafka.Producer(kafkaClient);

        kafkaProducer.on('ready', function () {
            console.log('Kafka producer is ready');
            retryAttempts = 0; // Reset the retry attempts after successful connection
            resolve();
        });

        kafkaProducer.on('error', function (err) {
            console.error('Error initializing Kafka producer:', err);

            if (retryAttempts < MAX_RETRIES) {
                retryAttempts++;
                console.log(`Retrying to connect to Kafka... Attempt ${retryAttempts}/${MAX_RETRIES}`);
                setTimeout(() => initializeKafkaProducer(), 5000); // Retry after 5 seconds
            } else {
                reject(`Failed to initialize Kafka producer after ${MAX_RETRIES} attempts.`);
            }
        });
    });
}

// Create an MQTT server
const server = net.createServer(aedes.handle);

// Start the MQTT broker
server.listen(MQTT_PORT, function () {
    console.log(`MQTT broker started and listening on port ${MQTT_PORT}`);
});

// Handle client connection
aedes.on('client', (client) => {
    console.log(`Client connected: ${client.id}`);
});

// Handle client disconnection
aedes.on('clientDisconnect', (client) => {
    console.log(`Client disconnected: ${client.id}`);
});

// Handle subscription
aedes.on('subscribe', (subscriptions, client) => {
    console.log(`Client ${client.id} subscribed to topics: ${subscriptions.map(sub => sub.topic).join(', ')}`);
});

// Handle messages (publishing) and forward to Kafka
aedes.on('publish', async (packet, client) => {
    const payload = packet.payload.toString();
    const topic = packet.topic;

    if (client) {
        console.log(`Message from client ${client.id}: ${payload} on topic ${topic}`);

        // Forward the message to Kafka
        const kafkaMessage = [
            {
                topic: topic === 'iot/temperature-readings' ? KAFKA_TEMPERATURE_TOPIC : KAFKA_LOGS_TOPIC,
                messages: payload,
                key: client.id, // Optional: Use client ID as key
            }
        ];

        kafkaProducer.send(kafkaMessage, (err, data) => {
            if (err) {
                console.error(`Error sending message to Kafka: ${err}`);
            } else {
                console.log(`Message sent to Kafka: ${JSON.stringify(data)}`);
            }
        });
    } else {
        console.log(`Message from broker on topic ${topic}: ${payload}`);
    }
});

// Initialize Kafka producer with retry logic
initializeKafkaProducer().catch((err) => {
    console.error('Failed to initialize Kafka producer:', err);
});
