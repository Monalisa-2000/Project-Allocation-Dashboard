# Allocation Dashboard

This repository contains a full-stack Project Allocation Dashboard with:

- `Frontend`: React + Vite application
- `Backend`: .NET 8 Clean Architecture Web API

## Prerequisites

- Node.js 18+ and npm
- .NET SDK 8.x
- (Optional) EF Core tools:
  - `dotnet tool install --global dotnet-ef`

## Project Structure

- `Frontend/` - UI application
- `Backend/` - API, application, domain, infrastructure, and tests
- `Backend/ProjectAllocation.slnx` - backend solution file

## Backend Setup and Run

1. Go to backend folder:
   - `cd Backend`
2. Restore dependencies:
   - `dotnet restore ProjectAllocation.slnx`
3. Apply database migrations:
   - `dotnet ef database update --project ProjectAllocation.Infrastructure --startup-project ProjectAllocation.API`
4. Run the API:
   - `dotnet run --project ProjectAllocation.API`
5. Open Swagger:
   - `http://localhost:5000/swagger` or `https://localhost:5001/swagger`

### Default Seeded Users

- Admin: `alice@app.com` / `Admin@123`
- User: `bob@app.com` / `User@123`
- User: `carol@app.com` / `User@123`
- User: `david@app.com` / `User@123`

## Frontend Setup and Run

1. Open a new terminal and go to frontend folder:
   - `cd Frontend`
2. Install dependencies:
   - `npm install`
3. Create env file from example:
   - Windows (PowerShell): `Copy-Item .env.example .env`
   - macOS/Linux: `cp .env.example .env`
4. Start development server:
   - `npm run dev`

By default, frontend uses:

- `VITE_API_BASE_URL=http://localhost:5001/api`

## Useful Commands

### Frontend

- `npm run dev` - start dev server
- `npm run build` - production build
- `npm run lint` - run lint checks
- `npm run format` - format project

### Backend

- `dotnet test` - run unit tests

