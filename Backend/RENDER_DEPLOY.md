# Deploy Backend to Render (Docker)

## 1. Prepare the Dockerfile
This project already includes `Backend/Dockerfile` configured to:
- Build and publish `ProjectAllocation.API` in Release mode (multi-stage build)
- Run on port `8080`
- Bind ASP.NET Core to `http://+:8080`

## 2. Create a Render Web Service
1. Log in to Render.
2. Go to **New +** -> **Web Service**.
3. Choose **Blueprint: Docker** (Runtime = Docker).
4. Set the following:
   - **Root Directory**: `Backend`
   - **Runtime**: `Docker`
   - **Port**: `8080`
5. Use the default build settings (Render will use the `Backend/Dockerfile`).
6. Click **Create Web Service**.

## 3. Set Environment Variables in Render
After creating the service (or before starting the first deploy), set these environment variables in the Render dashboard:

| Environment Variable | Value |
|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `Jwt__Secret` | `<strong secret>` |
| `Jwt__Issuer` | `ProjectAllocationAPI` |
| `Jwt__Audience` | `ProjectAllocationClient` |
| `FRONTEND_URL` | `<your deployed frontend URL>` |

## 4. Deploy and Verify
1. Trigger a new deploy (or redeploy) from the service page.
2. Check the service logs in Render to confirm the app starts successfully.
3. Validate endpoints from your frontend domain listed in `FRONTEND_URL`.

## 5. Important Note about SQLite
This backend uses SQLite (`projectallocation.db`) and the container filesystem is ephemeral. That means:
- Data (including seeded/created data) can reset on each deploy/restart.
- You will lose changes if Render redeploys the service.

