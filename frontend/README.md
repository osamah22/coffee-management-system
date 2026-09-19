# Roast & Ritual frontend

A Vite + React + TypeScript frontend for the Coffee Management System. It provides a public coffee collection and a Keycloak-protected manager studio for creating, editing, and deleting coffees and their serving options.

## Run locally

1. Start the existing API on `http://localhost:5231` and Keycloak on `http://localhost:8080`.
2. In Keycloak realm `coffee_management_system`, configure the public OIDC client named `public`:
   - Client authentication: off (public client)
   - Standard flow: on
   - Valid redirect URI: `http://localhost:5173/auth/callback`
   - Valid post logout redirect URI: `http://localhost:5173/*`
   - Web origin: `http://localhost:5173`
3. Give management users the `manager` realm role (or a `manager` client role on `public`).
4. From the `frontend` directory, install and run the application:

```bash
npm install
npm run dev
```

Vite proxies `/api` to the API during development. Copy `.env.example` to `.env` only when different endpoints or Keycloak settings are required.

During development, OIDC token and user-info requests are proxied through `/keycloak`. This avoids browser CORS failures while keeping the authorization and logout redirects on Keycloak. Production deployments should configure the frontend origin in the `public` client's **Web origins** and serve Keycloak directly over HTTPS.

## Structure

- `src/auth/` — Keycloak/OIDC configuration and auth state
- `src/components/` — reusable presentation, dialogs, and forms
- `src/hooks/` — data-loading hooks
- `src/lib/api.ts` — API transport matching the backend contracts and routes
- `src/pages/` — storefront, manager studio, and auth callback

The backend directories are read-only for frontend work. API changes must be handled separately.
