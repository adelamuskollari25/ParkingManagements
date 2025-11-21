# API Contract Changelog

This file documents changes to request/response models, routes, or authentication.

## [1.1.0] - 2025-11-21
### Added
- `/health/live` and `/health/ready` endpoints for liveness/readiness.
- `X-Correlation-ID` header included in all API responses.
- Swagger documentation (`/swagger` + `/swagger/v1/swagger.json`).

### Changed
- Error responses now include `correlationId` in JSON.
- ParkingLot GET `/api/ParkingLot/{id}` returns structured error format when not found.

## [1.0.0] - 2025-11-18
- Initial API release with authentication, parking management, tickets, payments, and reporting endpoints.
