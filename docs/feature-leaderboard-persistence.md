# Feature: Leaderboard Persistence

## What was added

- Added file-based JSON leaderboard persistence.
- Stores and loads the top 10 scores.
- Records score automatically when a run ends.
- Added a Leaderboard view accessible from Options.
- Displays rank, score, and date/time.

## Why it was added

The game needs durable progress feedback across sessions. A local JSON leaderboard keeps the implementation simple and easy to inspect.

## Files changed

- `src/Core/Leaderboard/ScoreEntry.cs`
- `src/Core/Leaderboard/LeaderboardService.cs`
- `src/Game/AppScreen.cs`
- `src/Game/GameLoop.cs`
- `src/UI/Menu/OptionsMenu.cs`
- `src/UI/Menu/LeaderboardPage.cs`
- `src/UI/TerminalRenderer.cs`
- `docs/feature-leaderboard-persistence.md`

## Future improvements

- Add initials/name entry for each score.
- Store difficulty alongside each score.
- Allow clearing leaderboard data from Options.
