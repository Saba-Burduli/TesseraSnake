# Feature: HUD Menu Bar

## What was added

- Added an in-game Tessera HUD menu bar.
- Shows current score, selected difficulty, and game status.
- Added `P` to pause and resume the run.
- Status updates live as Running, Paused, or Game Over.

## Why it was added

The player needs always-visible state while playing. The HUD also makes the selected difficulty visible during gameplay.

## Files changed

- `src/Game/GameStatus.cs`
- `src/Game/GameLoop.cs`
- `src/Game/InputHandler.cs`
- `src/UI/HUD/MenuBar.cs`
- `src/UI/SnakeTheme.cs`
- `src/UI/TerminalRenderer.cs`
- `docs/feature-hud-menu-bar.md`

## Future improvements

- Add elapsed time to the HUD.
- Add current snake length.
- Add a compact mobile-width HUD layout for narrow terminals.
