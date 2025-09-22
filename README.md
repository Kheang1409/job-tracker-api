# Job Tracker Application API

## Overview

The **Job Tracker** is a microservice-based application designed to manage job postings, user accounts, and notifications. It supports a clean separation of concerns by organizing functionality into three main services:

- **User Service**
- **Job Service**
- **Notification Service**

An **API Gateway** serves as a unified entry point for all HTTP requests. Kafka is used for inter-service communication (e.g., sending notifications for password resets).

---

## 🧱 Architecture Overview

```
Client → API Gateway → [User Service | Job Service]
                              ↓
                      Kafka Topic (job-tracker-topic)
                              ↓
                    Notification Service (email OTP)
```

---

## 📦 Services and Responsibilities

### 1. **User Service**

Handles user-related functionality such as:

- Login
- Forgot/Reset password (via email OTP)
- Register / Create user
- Update/delete profile
- Fetch user info

### 2. **Job Service**

Manages:

- Creating, updating, deleting job posts
- Assigning skills to jobs
- Changing job status/location
- Searching jobs

### 3. **Notification Service**

- Subscribes to Kafka topic `job-tracker-topic`
- Sends email OTPs for password reset

### 4. **API Gateway**

Routes all incoming API requests to the appropriate backend service via predefined routes.

---

## 🔁 Kafka Integration

- **Kafka + Zookeeper** is used as the messaging layer between services.
- Topic used: `job-tracker-topic`
- **Notification Service** consumes from Kafka to process email OTP notifications.

Kafka is initialized by the `init-kafka` container which waits for Kafka to be ready and then creates the topic.

---

## 🐳 Dockerized Microservices

Your application runs as a set of Docker containers defined in `docker-compose.yml`. Below are the key services:

| Service              | Port Exposed | Internal Port | Description                     |
| -------------------- | ------------ | ------------- | ------------------------------- |
| API Gateway          | 5000         | 8080          | Entry point for client requests |
| User Service         | 5001         | 8080          | Auth & user profile handling    |
| Job Service          | 5002         | 8080          | Job post management             |
| Notification Service | N/A          | N/A           | Sends emails, listens to Kafka  |
| Kafka                | 9092         | 9092          | Message queue                   |
| Zookeeper            | 2181         | 2181          | Kafka dependency                |

---

## ⚙️ Environment Variables

Make sure to create a `.env` file in the root with the following (sample):

```env
# MongoDB
CONNECTION_STRINGS=mongodb://host.docker.internal:27017
MONGODB_DATABASE_NAME=JobTrackerDB

# JWT
JWT_SECRET_KEY=your_jwt_secret
JWT_ISSUER=jobtracker.io
JWT_AUDIENCE=jobtracker.io
JWT_EXPIRY_MINUTES=60

# Kafka
KAFKA_BOOTSTRAP_SERVERS=kafka:9092
KAFKA_GROUP_ID=notification-service-group

# Email (SMTP)
SMTP_SERVER=smtp.gmail.com
SMTP_PORT=587
SMTP_SENDER_EMAIL=youremail@gmail.com
SMTP_SENDER_PASSWORD=yourpassword
```

---

## 🚀 Running the Application

### ✅ Prerequisites

- Docker
- Docker Compose

### 📦 Start Services

```bash
docker-compose up --build
```

This will:

- Start **Zookeeper** and **Kafka**
- Create the `job-tracker-topic`
- Build and run all services
- Run the **API Gateway** on `http://localhost:5000`

---

## 📚 Available Endpoints

Here are the main HTTP endpoints (via API Gateway):

### 🔐 Auth

- `POST /api/auth/login`
- `POST /api/auth/forgot-password`
- `POST /api/auth/reset-password`

### 👤 User Management

- `POST /api/users` — create user
- `GET /api/users/{userId}`
- `PUT /api/users/{userId}/profile`
- `GET /api/users/me`
- `PUT /api/users/me`
- `DELETE /api/users/me`

### 💼 Job Management

- `POST /api/jobs` — create job
- `GET /api/jobs` — search jobs
- `GET /api/jobs/{id}`
- `PUT /api/jobs/{id}`
- `DELETE /api/jobs/{id}`
- `PUT /api/jobs/{id}/status`
- `PUT /api/jobs/{id}/location`
- `POST /api/jobs/{jobId}/skills`
- `DELETE /api/jobs/{jobId}/skills/{skillId}`

---

## 🧪 Health & Startup Order

1. **Zookeeper** → 2. **Kafka** → 3. **init-kafka** → 4. **User / Job Services** → 5. **Notification Service** → 6. **API Gateway**

Startup is automatically managed by `depends_on` in `docker-compose.yml`.

---

## 🤝 Contributing

We welcome contributions and improvements. Please:

1. Fork the repository
2. Create a feature branch
3. Submit a pull request

---

## 📄 License

This project is licensed under the MIT License.
