# Feature: Tessera Setup

## What was added

- Added the Tessera NuGet package.
- Verified the project builds after dependency restore.
- Documented the framework prerequisites: .NET 10 SDK and an ANSI/CSI-capable terminal.
- Aligned the project root namespace with the `TesseraSnake` source namespace.

## Why it exists

Tessera provides the terminal UI app model, rendering, input messages, and periodic effects used by the game.

## Files changed

- `tessera-snake.csproj`
- `README.md`
- `docs/feature-tessera-setup.md`

## Next improvements

- Add color styling after terminal capability behavior is verified.
- Add higher-level gameplay documentation as new features land.
