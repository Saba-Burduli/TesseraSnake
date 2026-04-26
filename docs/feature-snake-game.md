# Feature: Snake Game

## What was added

- Added a Tessera application loop with periodic game ticks.
- Added keyboard input for arrows, WASD, restart, and quit.
- Added Snake state, movement, food spawning, collision, scoring, and win/game-over handling.
- Added terminal rendering through Tessera controls.
- Added a non-interactive `--self-test` mode for verification.

## Why it exists

This is the first playable version of the terminal game. It proves Tessera can host a real-time interactive loop and render live game state in the terminal.

## Files changed

- `src/Program.cs`
- `src/Game/GameLoop.cs`
- `src/Game/InputHandler.cs`
- `src/Game/Renderer.cs`
- `src/Core/Entities/Direction.cs`
- `src/Core/Entities/GridPoint.cs`
- `src/Core/Entities/SnakeGameState.cs`
- `src/UI/TerminalRenderer.cs`
- `docs/feature-snake-game.md`

## Next improvements

- Add difficulty selection.
- Add persisted high score.
- Add color styling once terminal capability behavior is verified.
