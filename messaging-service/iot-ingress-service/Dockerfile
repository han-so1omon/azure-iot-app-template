# Use the official Node.js image
FROM node:18

# Set the working directory
WORKDIR /app

# Copy the package.json and install dependencies
COPY package*.json ./
RUN npm install

# Copy the remaining application files
COPY . .

# Expose the MQTT port
EXPOSE 1883

# Start the MQTT broker
CMD ["node", "mqtt-broker.js"]
