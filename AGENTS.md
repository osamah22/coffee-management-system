# Repository Agent Instructions

These instructions apply to the entire repository.

## Scope

- Only write or modify frontend files.
- Backend modifications are prohibited. Do not edit anything under `src/Api`, `src/Application`, or `src/Contracts`.
- Treat `src/Contracts` and `src/Api` as read-only sources of truth for API routes, payloads, authentication, and response shapes.
- Do not change backend behavior to accommodate the frontend. Adapt the frontend to the existing API contract.

## Frontend conventions

- The frontend is the self-contained Vite application in `frontend/`; its source lives in `frontend/src/`.
- Run frontend package commands from `frontend/`.
- Keep API access in `frontend/src/lib/api.ts` and authentication concerns in `frontend/src/auth/`.
- Preserve the existing visual language and responsive behavior when extending the UI.
- Keep `README.md` and relevant frontend documentation current when architecture, configuration, or user flows change, so future conversations have enough context.

## Git operations

- Commit or push only when explicitly requested.
- Preserve unrelated user changes and never rewrite history or discard work without explicit instruction.
