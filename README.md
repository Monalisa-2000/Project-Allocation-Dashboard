# Project Allocation Dashboard

A role-based project management application where admins can create and assign projects to users, and users can track and complete their assigned work.

**Frontend:** https://project-allocation-dashboard.vercel.app/login  
**Backend (Swagger):** https://project-allocation-dashboard.onrender.com/swagger/index.html

> The backend runs on Render's free tier and may take up to 60 seconds to respond after a period of inactivity.

---

## Tech Stack

- **Frontend:** React 19, TypeScript, TanStack Router, TanStack Query, Axios, shadcn/ui, Tailwind CSS
- **Backend:** ASP.NET Core 8, Clean Architecture, ASP.NET Identity, JWT, EF Core, SQLite, FluentValidation

---

## Prerequisites

- Node.js 18+
- .NET SDK 8.x

---

## Running Locally

### Backend

```bash
cd Backend
dotnet restore ProjectAllocation.slnx
dotnet run --project ProjectAllocation.API
```

Swagger: `http://localhost:5000/swagger`

### Frontend

```bash
cd Frontend
npm install
cp .env.example .env
npm run dev
```

App: `http://localhost:5173`

---

## Demo Accounts

| Role  | Email               | Password   |
|-------|---------------------|------------|
| Admin | monalisa@app.com    | Admin@123  |
| User  | sooraj@app.com      | User@123   |
| User  | priya@app.com       | User@123   |
| User  | arjun@app.com       | User@123   |

---

## Project Structure

```
Frontend/                          # React + Vite
Backend/
  ProjectAllocation.API/           # Controllers, middleware, Program.cs
  ProjectAllocation.Application/   # Services, DTOs, interfaces, validators
  ProjectAllocation.Domain/        # Entities, enums
  ProjectAllocation.Infrastructure/# Repositories, EF Core, migrations
  ProjectAllocation.Tests/         # Unit tests
```



## Assumptions

- Admin accounts are seeded — public registration creates User accounts only
- A user can be assigned multiple projects at the same time
- Marking a project complete is a toggle — it can be undone
- All timestamps are in UTC
