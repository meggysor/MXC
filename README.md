# Event Manager

A web application for managing events, built as part of a technical assessment.

## Tech Stack

**Backend:** ASP.NET Core Web API (.NET 10), Entity Framework Core, SQL Server (LocalDB), ASP.NET Identity, JWT authentication

**Frontend:** Angular (latest), TypeScript

## Repository Structure

Both the backend and frontend live in this repository, in separate folders:
- Backend — ASP.NET Core Web API project
- `event-manager-client/` — Angular client (frontend)

## Prerequisites

**Backend:**
- Visual Studio 2026 / .NET 10 SDK
- SQL Server LocalDB
- Postman (or any other HTTP client) for testing the API

**Frontend:**
- Node.js
- Angular CLI (`npm install -g @angular/cli`)

## Getting Started (Backend)

1. Clone the repository
2. Set the Angular client URL for CORS in `appsettings.json`:
   ```json
   "AngularUIURL": "http://localhost:4200"
   ```
   This must exactly match the URL the Angular app runs on (protocol, host, port — no trailing slash).
3. Apply migrations to create the database:
   ```
   dotnet ef database update
   ```
4. Run the project (F5). Two test users are seeded automatically on startup.
5. Use Postman (or any HTTP client) to test the endpoints listed below.

> **Note:** The JWT secret key is stored in `appsettings.json` for the purposes of this assessment. See the **Notes** section below for how this would be handled in a real production setup.

## Getting Started (Frontend)

1. Navigate to the `event-manager-client` folder
2. Install dependencies:
   ```
   npm install
   ```
3. Set the backend API URL in `src/environments/environment.ts`:
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'https://localhost:7035/api'
   };
   ```
   (adjust the port to match your backend's actual HTTPS port)
4. Run the dev server:
   ```
   ng serve
   ```
5. Open `http://localhost:4200` in the browser. You'll be redirected to the login page.

> **Note:** The backend must be running for login and data to work, and its `AngularUIURL` setting must match the URL shown above.

## Test Users

| Email | Password |
|---|---|
| admin@mxc.hu | Admin123! |
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
- The frontend and backend live in the same repository but are fully decoupled, communicating over REST/JSON, with CORS restricted to the configured Angular client origin.
