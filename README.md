# Job Tracker Web Application

## Overview

The **Job Tracker Web Application** is designed to help users manage job listings, applications, and status updates. This project is built by a solo developer to learn about **microservices**, **Domain-Driven Design (DDD)**, **CQRS (Command Query Responsibility Segregation)**, and the full **software development lifecycle**. The application is built using **.NET**, incorporating best practices for **scalability**, **clean code**, and **maintainability**.

## Key Features

- **Job Listings Management**: Create, read, update, and delete job listings.
- **Job Applications**: Users can apply for jobs, and track the status of their applications.
- **CQRS**: Clear separation of commands (write operations) and queries (read operations) for better scalability.
- **Email Notifications**: Automatically send emails for job application updates (e.g., when a job status changes).
- **Microservices Architecture**: Decomposed into microservices to enhance modularity, scalability, and maintainability.
- **Clean Code Practices**: Follows DDD principles for maintainable, scalable software architecture.

## Tech Stack

- **Backend**: .NET 6+ (C#)
- **Architecture**: Microservices, CQRS, DDD
- **Frameworks & Tools**:
  - **MediatR**: For handling CQRS commands and queries
  - **SendGrid / SMTP**: Email service integration
  - **Serilog**: Structured logging
  - **Moq / NUnit**: Unit testing framework
  - **Docker**: Containerization (optional)
  - **CI/CD**: GitHub Actions / GitLab CI for continuous integration and deployment
  - **EF Core**: ORM for database interactions

## Setup

### Prerequisites

Ensure you have the following installed on your machine:
- **.NET 6+ SDK**: [Download .NET](https://dotnet.microsoft.com/download)
- **Docker** (optional, for containerization): [Install Docker](https://www.docker.com/get-started)
- **SMTP/SendGrid** account for email functionality.

### Installation Steps

#### 1. Clone the Repository
Start by cloning the repository to your local machine:
```bash
git clone [https://github.com/your-username/job-tracker.git](https://github.com/Kheang1409/job-tracker-api)
cd job-tracker
