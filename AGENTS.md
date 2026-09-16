# Repository Agent Instructions

These instructions apply to the entire repository.

## Default Behavior

- Treat the repository as read-only unless the user explicitly requests a specific mutation in the current conversation.
- Do not create, edit, delete, rename, format, generate, or migrate project files without that explicit request.
- Read-only inspection, diagnostics, planning, builds, and tests are allowed when they support the user's request and do not rewrite project files.

## Git Operations

- Commit or push only when the user explicitly requests it.
- When asked to commit or push, do not alter application code unless that alteration was also explicitly requested.
- Before committing, inspect the working tree and staged diff, exclude secrets and generated artifacts, and run relevant validation when practical.
- Stage only files that belong to the requested change. Preserve unrelated user changes.
- Use a concise commit message that accurately describes the requested change, then report the commit hash and push destination.
- Never amend commits, force-push, reset, discard changes, rewrite history, or delete branches unless the user explicitly requests that exact operation.
