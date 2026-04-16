# Project Allocation Dashboard Backend

This solution contains a Clean Architecture `.NET 8` backend for the Project Allocation Dashboard.

## Projects

- `ProjectAllocation.API` - Web API entry point (controllers, middleware, Program setup).
- `ProjectAllocation.Application` - DTOs, interfaces, services, validators, AutoMapper profile.
- `ProjectAllocation.Domain` - Entities and enums.
- `ProjectAllocation.Infrastructure` - EF Core DbContext, repositories, seed data, migrations.
- `ProjectAllocation.Tests` - xUnit unit tests with Moq.

## Prerequisites

- .NET SDK 8.x
- EF Core tools: `dotnet tool install --global dotnet-ef` (if not already installed)

## Run Locally

1. Restore dependencies:
   - `dotnet restore ProjectAllocation.sln`
2. Create database migration:
   - `dotnet ef migrations add InitialCreate --project ProjectAllocation.Infrastructure --startup-project ProjectAllocation.API`
3. Update database:
   - `dotnet ef database update --project ProjectAllocation.Infrastructure --startup-project ProjectAllocation.API`
4. Start API:
   - `dotnet run --project ProjectAllocation.API`
5. Open Swagger:
   - `http://localhost:5000/swagger` or `https://localhost:5001/swagger`

## Default Seeded Users

- Admin: `alice@app.com` / `Admin@123`
- User: `bob@app.com` / `User@123`
- User: `carol@app.com` / `User@123`
- User: `david@app.com` / `User@123`
