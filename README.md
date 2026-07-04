# Event Manager

A web application for managing events, built as part of a technical assessment.

## Tech Stack

**Backend:** ASP.NET Core Web API (.NET 10), Entity Framework Core, SQL Server (LocalDB), ASP.NET Identity, JWT authentication

**Frontend:** Angular (latest), TypeScript in an another repository

## Prerequisites

- Visual Studio 2026 / .NET 10 SDK
- SQL Server LocalDB
- Postman (or any other HTTP client) for testing the API

## Getting Started (Backend)

1. Clone the repository
2. Apply migrations to create the database:
   ```
   dotnet ef database update
   ```
3. Run the project (F5). Two test users are seeded automatically on startup.
4. Use Postman (or any HTTP client) to test the endpoints listed below.

> **Note:** The JWT secret key is stored in `appsettings.json` for the purposes of this assessment. See the **Notes** section below for how this would be handled in a real production setup.

## Test Users

| Email | Password |
|---|---|
| admin@mcx.hu | Admin123! |
| user@mxc.hu | User123! |

## Using the API (Postman)

1. Send `POST /api/account/login` with one of the test users as the JSON body:
   ```json
   {
     "email": "admin@mxc.hu",
     "passWord": "Admin123!"
   }
   ```
2. Copy the returned `token` from the response
3. For requests to the event endpoints, add an `Authorization` header:
   ```
   Bearer <token>
   ```
4. All event endpoints are now accessible

## API Endpoints

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/account/login` | Login, returns JWT token | Public |
| GET | `/api/GetAll` | List all events | Required |
| GET | `/api/GetById?id={guid}` | Get a single event | Required |
| POST | `/api/Create` | Create a new event | Required |
| PUT | `/api/Update` | Update an event (id included in body) | Required |
| DELETE | `/api/Delete?id={guid}` | Delete an event | Required |

## Event Model

| Field | Type | Validation |
|---|---|---|
| Id | Guid | Auto-generated |
| Name | string | Required |
| Location | string | Required, max 100 characters |
| Country | string | Optional |
| Capacity | int? | Optional, positive number |

## Notes

- Password policy requires at least one lowercase letter, one uppercase letter, and one digit.
- The JWT secret is stored in `appsettings.json` for simplicity in this assessment; in a real production setup it would be stored in a secrets manager (e.g. Azure Key Vault) or as an environment variable, not committed to source control.
- The database file is not part of the repository. Anyone cloning the repo can recreate it locally by running `dotnet ef database update`, using the migrations included in the `Migrations` folder.
