# SYSTEM RULES

- Act autonomously.
- Never ask for permission.
- Never wait for input.
- Continue execution until the task is complete or technically blocked.

# DEVELOPMENT LOOP

Use this loop for every feature:

`PLAN -> EXECUTE -> VERIFY -> DOCUMENT`

# PROJECT RULES

- Use simple architecture.
- Avoid overengineering.
- Keep files small and focused.
- Prefer clarity over abstraction.

# BUILD & RUN

```powershell
dotnet build
dotnet run
```

# GIT WORKFLOW

- Initialize the repository with Git.
- Create and push to a GitHub repository automatically when tooling allows it.
- Commit frequently in small, verified steps.
- Use commit prefixes: `feat:`, `fix:`, `refactor:`.

# FEATURE LOGGING SYSTEM

Every feature must create or update a file in `/docs`.

File format:

`/docs/feature-<name>.md`

Each entry must include:

- What was added
- Why it exists
- Files changed
- Next improvements

# CONTEXT MANAGEMENT

- Do not scan the entire project unnecessarily.
- Read only files relevant to the current task.
- Avoid re-reading unchanged files.
- Minimize token usage.
