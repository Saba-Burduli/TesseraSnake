# Feature: Difficulty System

## What was added

- Added Easy, Medium, and Hard difficulty levels.
- Added difficulty selection to the Options menu.
- Stored the selected difficulty in memory for the session.
- Applied difficulty to game movement speed when a run starts.
- Displayed the active difficulty in the in-game stats and status text.

## Why it was added

Difficulty gives the game replay value and makes the Options menu functional. It also establishes a session-level setting that later UI features can display.

## Files changed

- `src/Game/DifficultyLevel.cs`
- `src/Game/DifficultySettings.cs`
- `src/Game/GameLoop.cs`
- `src/UI/TerminalRenderer.cs`
- `src/UI/Menu/OptionsMenu.cs`
- `docs/feature-difficulty-system.md`

## Future improvements

- Add obstacle rules tied to difficulty.
- Persist the preferred difficulty between sessions.
- Add a visual confirmation when a difficulty is applied.
