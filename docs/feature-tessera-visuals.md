# Feature: Tessera Visuals

## What was added

- Replaced the plain text board with a custom Tessera control.
- Added styled board cells, snake head/body, food, border, status bar, and game-over overlay.
- Enabled Tessera theme configuration and synchronized terminal updates.
- Kept the real-time game loop on `TesseraEffects.Periodic`.

## Why it exists

The game should use Tessera as the primary terminal UI framework instead of only displaying a plain string inside a label. A custom control lets the board render directly through Tessera's canvas and theme system.

## Files changed

- `src/Program.cs`
- `src/Game/GameLoop.cs`
- `src/Game/Renderer.cs`
- `src/UI/TerminalRenderer.cs`
- `src/UI/SnakeBoardControl.cs`
- `src/UI/SnakeTheme.cs`
- `docs/feature-tessera-visuals.md`

## Next improvements

- Add a high-score panel.
- Add difficulty controls.
- Add terminal screenshots after a visual capture path is available.
