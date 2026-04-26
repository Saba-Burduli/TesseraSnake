# Feature: GitHub Publish

## What was added

- Installed GitHub CLI with `winget`.
- Added the intended remote: `https://github.com/Saba-Burduli/tessera-snake.git`.
- Attempted a non-interactive remote check.

## Why it exists

The project workflow requires automatic GitHub publishing after local commits are verified.

## Files changed

- `docs/feature-github-publish.md`

## Next improvements

- Authenticate GitHub CLI with `gh auth login` or set `GH_TOKEN`.
- Create `Saba-Burduli/tessera-snake`.
- Push local `master` to the GitHub remote.

## Current blocker

GitHub CLI is installed but not authenticated, and no `GH_TOKEN` or `GITHUB_TOKEN` environment variable is available. The GitHub repository does not currently exist, so `git ls-remote origin` returns `Repository not found`.
