# Use the official Python 3.11 image as the base image
FROM python:3.11-slim

# Set environment variables
ENV POETRY_VERSION=1.6.1
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1

# Install system utilities (curl, net-tools, procps)
RUN apt-get update && apt-get install -y curl net-tools procps

# Install Poetry
RUN pip install "poetry==$POETRY_VERSION"

# Set the working directory
WORKDIR /app

# Copy pyproject.toml and poetry.lock (if available)
COPY pyproject.toml poetry.lock* /app/

# Install dependencies with Poetry, including venv creation
RUN poetry config virtualenvs.create false && poetry install --no-interaction --no-ansi

# Copy the entire application code into the container
COPY . /app

# Expose the port FastAPI will run on
EXPOSE 8000

# Command to run the FastAPI server using Uvicorn
CMD ["poetry", "run", "uvicorn", "analytics_engine.main:app", "--host", "0.0.0.0", "--port", "8000"]
