# JobTracker backend

The backend for JobTracker, a personal job-application tracker. It provides a
.NET 10 user API plus a worker-only email service. It does not publish jobs,
manage candidates, or expose an email HTTP API.

## Architecture

- `UserService.API` provides registration, email verification, login, password recovery, profile management, job-application CRUD, SignalR updates, and health checks.
- `UserService.Application` contains MediatR commands, queries, validators, and contracts.
- `UserService.Domain` contains the user and job-application domain models.
- `UserService.Infrastructure` provides MongoDB persistence, JWT creation, Kafka publishing, indexes, and the transactional email outbox.
- `EmailService.Worker` consumes `email-notifications` and sends branded email through SMTP.
- `SharedKernel` contains validation, authentication, CORS, health, and exception handling.
- `Tests` covers domain behavior, validation, Mongo mappings, outbox encryption, and email templates.

The previous Ocelot gateway, Job Service, and Notification Service were removed.
The root workspace Nginx proxy is now the browser-facing API entry point.

## Reliability and security

- JWT issuer, audience, signature, and lifetime validation
- PBKDF2-SHA256 password hashing with migration from legacy hashes
- Hashed email-verification and password-reset secrets in user records
- Five-attempt reset-code lockout and expiring, single-use codes
- Per-client rate limits on authentication and recovery endpoints
- Generic authentication/recovery errors that avoid account and token leakage
- MongoDB unique and query indexes
- Transactional MongoDB email outbox with AES-GCM encrypted token payloads
- Kafka idempotent producer, manual commits, exponential retry, and dead-letter delivery
- Authenticated user-scoped job-application endpoints and SignalR hub

MongoDB transactions require a replica set or sharded cluster.

## Configuration

Copy the template to the workspace root, then replace every placeholder:

```powershell
Copy-Item .\example.env ..\.env
```

Configure MongoDB, JWT, Kafka, SMTP, frontend origin, email topic, dead-letter
topic, and optionally `OUTBOX_ENCRYPTION_KEY`. Use a dedicated outbox key of at
least 32 characters in production. When omitted, the service falls back to
`JWT_SECRET_KEY`; drain the outbox before rotating the active encryption key.

Never commit `.env`, credentials, private keys, or production certificates.

## Build and test

Requires the .NET 10 SDK.

```powershell
dotnet build JobTrackerApp.sln
dotnet test --solution JobTrackerApp.sln
```

Run the full stack from the parent workspace, where `docker-compose.yml` lives:

```powershell
Set-Location ..
docker compose up -d --build
docker compose ps -a
```

The frontend proxy publishes the health endpoint at
`http://localhost:4200/health` during local Compose development.

## Email delivery flow

1. Signup or password recovery updates the user and inserts an encrypted outbox record in one MongoDB transaction.
2. The hosted outbox publisher leases pending records and publishes them to Kafka.
3. The email worker renders and sends the message through SMTP.
4. Successful SMTP delivery commits the Kafka offset.
5. Invalid messages are discarded; exhausted delivery failures go to the configured DLQ.
