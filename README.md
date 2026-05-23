# Lumina

**Lumina** is a full-stack **cinema / ticketing** application built as a portfolio project. It combines an **Angular** SPA, an **ASP.NET Core Web API**, and a separate **background worker** that communicates over **RabbitMQ** using **MassTransit**.

> **Status:** Work in progress — suitable for learning and junior portfolio review, not production-ready.

---

## Architecture overview

The solution uses **two runnable backends** plus a frontend. They share a message contract but use **different databases**.

```text
┌─────────────┐     HTTP/JWT      ┌──────────────────┐
│  Angular UI │ ◄──────────────► │  ApiLumina (API) │
│    (UI/)    │                   │  Main SQL DB     │
└─────────────┘                   │  (Identity,    │
                                  │   movies, etc.) │
                                  └────────┬─────────┘
                                           │ Publish
                                           │ UserRegisteredEvent
                                           ▼
                                  ┌──────────────────┐
                                  │    RabbitMQ      │
                                  │   (localhost)    │
                                  └────────┬─────────┘
                                           │ Consume
                                           ▼
                                  ┌──────────────────┐
                                  │  Lumina.Worker   │
                                  │  Worker SQL DB   │
                                  │  (email logs)    │
                                  └────────┬─────────┘
                                           │ SMTP
                                           ▼
                                  ┌──────────────────┐
                                  │    Mailtrap      │
                                  │  (welcome email) │
                                  └──────────────────┘
```

### Registration flow (async)

1. User registers via the **API** (`AuthService`).
2. User is stored in the **main** database (ASP.NET Core Identity).
3. API publishes **`UserRegisteredEvent`** to RabbitMQ (MassTransit).
4. **`Lumina.Worker`** consumes the message (`SendWelcomeEmailConsumer`).
5. Worker sends a **welcome email** via **Mailtrap** (SMTP).
6. Worker writes an **`EmailLog`** row to its **own** database.

Shared contract: `API/Lumina.Common/Shared/UserRegisteredEvent.cs`.

---

## Tech stack

| Area | Technologies |
|------|----------------|
| Frontend | Angular, RxJS, Angular Material |
| API | ASP.NET Core 8, EF Core, Identity, JWT, Swagger |
| Worker | .NET 8 Worker, MassTransit, MailKit |
| Messaging | RabbitMQ + MassTransit |
| Email (dev) | Mailtrap SMTP |
| Data | SQL Server LocalDB (main app DB + worker email log DB) |
| Real-time | SignalR (chat, presence, notifications) |

---

## Repository structure

```text
Lumina/
├── UI/                      # Angular frontend
├── API/
│   ├── API.sln
│   ├── ApiLumina/           # Web API (publishes events)
│   ├── Lumina.Worker/       # Consumer (email + email log DB)
│   ├── Lumina.Common/       # Shared messages (UserRegisteredEvent)
│   ├── Lumina.Data/         # Main EF Core context & migrations
│   ├── Lumina.Services.Core/
│   └── ...
└── README.md
```

---

## Features

- User **register / login** with **JWT**
- **Roles & permission policies** on API endpoints
- **Movies**, **screenings**, **tickets**, **watchlist**
- **Admin** user management
- **SignalR** for real-time features (chat, presence, notifications)
- **Async welcome flow:** register → message bus → worker → Mailtrap + email audit log

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (LTS) and npm
- **SQL Server LocalDB** (or update connection strings for your SQL instance)
- **RabbitMQ** running locally (`localhost`, default `guest` / `guest`)
- [Mailtrap](https://mailtrap.io/) account (SMTP credentials for the worker)

---

## Configuration & secrets

**Do not commit real secrets** to a public repository.

### API (`ApiLumina`)

`appsettings.json` contains structure and LocalDB connection strings. The JWT signing key uses a placeholder.

Set the JWT secret locally:

```bash
cd API/ApiLumina
dotnet user-secrets set "JwtSettings:Secret" "YOUR_LONG_RANDOM_SECRET_AT_LEAST_32_CHARS"
```

### Worker (`Lumina.Worker`)

`WorkerDbContextConnection` is in `appsettings.json` (LocalDB). **Mailtrap** credentials should be stored in user secrets:

```bash
cd API/Lumina.Worker
dotnet user-secrets set "MailtrapSettings:Username" "your-mailtrap-user"
dotnet user-secrets set "MailtrapSettings:Password" "your-mailtrap-password"
```

### RabbitMQ

Both **ApiLumina** and **Lumina.Worker** connect to RabbitMQ at `localhost` with `guest` / `guest`. Start RabbitMQ before testing the welcome-email flow.

---

## Database setup

### Main application database

Migrations live in `Lumina.Data` (`CinemaAppDBContext`):

```bash
cd API/ApiLumina
dotnet ef database update --project ../Lumina.Data --startup-project .
```

### Worker database (email logs)

Migrations live in `Lumina.Worker` (`WorkerDbContext`):

```bash
cd API/Lumina.Worker
dotnet ef database update
```

---

## Run locally

Use **four** steps (RabbitMQ + Worker + API + UI).

### 1. RabbitMQ

Ensure RabbitMQ is running on your machine.

### 2. Worker (consumer)

```bash
cd API/Lumina.Worker
dotnet run
```

Keep this running so it can consume `UserRegisteredEvent` messages.

### 3. API

```bash
cd API/ApiLumina
dotnet run
```

### 4. Angular UI

```bash
cd UI
npm install
npm start
```

Open the UI (usually `http://localhost:4200`). Register a new user, then verify:

- **Mailtrap inbox** — welcome email
- **Worker database** — `EmailLogs` table

---

## Screenshots

_Add screenshots here (home, movies, admin, Mailtrap welcome email)._

---

## Roadmap

- [ ] Retry / dead-letter handling for failed emails
- [ ] Align MassTransit package versions across API and Worker
- [ ] CI: build API, Worker, and Angular
- [ ] Production secrets via environment variables or a secret manager

---

## License

MIT
