# Parking Management API

## Introduction
The Parking Management API backend provides endpoints for managing parking lots, parking spots, tickets, payments, tariffs, and reporting.  
It is designed to support a web frontend with JWT authentication, role-based authorization, observability features (health endpoints and correlation IDs), and API documentation.  
The main objective of this project is to simplify and automate parking management operations while maintaining auditability and traceability of critical actions.

---

## Prerequisites / Software Dependencies
The following software is required to run the backend:

- .NET 8 SDK  
- SQL Server  
- Node.js & Angular (for frontend client)

---

## Setup / Installation
To set up the project locally:

1. Clone the repository.  
2. Update the database connection string in `appsettings.json` to point to your SQL Server instance.  
3. Apply database migrations and seed initial data to ensure the database is ready.  

---

## Run Commands
To start the backend API, run the application. The API will start on a local HTTPS port (check console output for the port). Once running, it is ready to serve requests.

---

## API References
The backend publishes an API reference:

- **Swagger UI (human-readable):** Allows exploring and testing all API endpoints.  
- **OpenAPI JSON (machine-readable):** Can be used to generate client code or documentation.

---

## Health Endpoints
The API provides minimal observability through health endpoints:

- **Liveness:** Checks if the application is running.  
- **Readiness:** Checks if the application is ready to handle requests (e.g., database connectivity).

---

## Observability
- All API responses include a **correlation ID** in headers and error responses.  
- This allows tracing requests for debugging and monitoring purposes.  
- Example of an error response with correlation ID:
{
"code": "notfound",
"message": "Parking lot not found.",
"correlationId": "0HNH8S328MQ4B:00000009"
}

---

## Authentication
- JWT Bearer tokens are used for authentication.  
- Login endpoint returns a token and role information.  
- Role-based authorization is enforced on endpoints where necessary.  

---

## Audit Logging
- Core actions, such as creating or closing tickets and modifying tariffs, are logged automatically.  
- Each record stores:  
  - **Who** performed the action (`CreatedBy` / `ModifiedBy`)  
  - **When** the action occurred (`CreatedAt` / `ModifiedAt`)  

---

## Changelog
A minimal changelog of API contract changes is maintained in `docs/CHANGELOG.md`. This file includes additions, changes, or deprecations of endpoints, request/response models, and headers.

---

## Contributing
- Follow existing patterns for controllers and services.  
- Update Swagger documentation if new endpoints are added.  
- Update the changelog if request/response models or headers change.

