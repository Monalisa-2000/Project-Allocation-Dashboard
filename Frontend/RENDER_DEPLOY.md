## Deploy Frontend to Render (Static Site)

### Render Static Site Settings
1. Root Directory: `Frontend`
2. Build Command: `npm install && npm run build`
3. Publish Directory: `dist`

### Environment Variables (Render Dashboard)
| Key | Value |
|---|---|
| `VITE_API_BASE_URL` | `https://your-actual-backend.onrender.com/api` |

### Important Notes
- The `_redirects` file in `public/` handles client-side SPA routing (do not remove it).
- `VITE_API_BASE_URL` must be set in the Render dashboard (at build time for static sites), not just in `.env.production`.
- After deploying the backend first, copy its Render URL and set it in `VITE_API_BASE_URL` here before deploying the frontend.
- Also update the backend and set `FRONTEND_URL = https://your-frontend.onrender.com` so CORS allows the frontend.
