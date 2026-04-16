## Deploy ProjectAllocation.API to Render (Docker)

### 1) Push your code
1. Commit and push the backend changes to your Git provider (GitHub/GitLab/etc.).
2. Ensure your repository root contains the `Backend/` folder with the `Dockerfile` inside it.

### 2) Create a Render Web Service
1. In the Render dashboard, click **New +** → **Web Service**.
2. Set **Root Directory** to `Backend`.
3. Set **Runtime** to `Docker`.
4. Set **Port** to `8080`.
5. Finish creating the service.

### 3) Configure environment variables
In the Render service settings → **Environment** (or **Environment Variables**), set:

| Variable | Value |
|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `Jwt__Secret` | `<strong secret>` |
| `Jwt__Issuer` | `ProjectAllocationAPI` |
| `Jwt__Audience` | `ProjectAllocationClient` |
| `FRONTEND_URL` | `<your deployed frontend URL>` |

Notes:
- `Jwt__Secret` corresponds to `Jwt:Secret` in `Program.cs` / `appsettings.json` (Render converts `:` to `__` automatically).
- `FRONTEND_URL` should include the full origin (scheme + host, e.g. `https://your-domain.com` or `https://your-domain.com:443` as applicable) because CORS `WithOrigins(...)` requires exact origins.

### 4) SQLite persistence warning
This project uses SQLite (`projectallocation.db`) and, on Render, the container filesystem is typically ephemeral. That means the SQLite data will reset on each deploy/rebuild.
