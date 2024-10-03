# Stage 1: Build the Next.js application
FROM node:20-alpine AS builder

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package.json package-lock.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the Next.js application
RUN npm run build

# Stage 2: Create a lightweight production image
FROM node:20-alpine AS runner

# Set the working directory
WORKDIR /app

# Copy the built application from the builder stage
COPY --from=builder /app/.next ./.next
COPY --from=builder /app/public ./public
COPY --from=builder /app/package.json ./

# Install only production dependencies
RUN npm install --only=production

# Expose the port that Next.js will run on
EXPOSE 3000

# Set the environment variable to production
ENV NODE_ENV=production

# Start the Next.js application
CMD ["npm", "start"]
