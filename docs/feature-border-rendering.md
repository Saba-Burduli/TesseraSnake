# Feature: Border Rendering

## What was added

- Switched UI borders from rounded glyphs to ASCII borders.
- Kept existing Tessera colors, layout, and styling.

## Why it was added

ASCII borders render consistently across Windows Terminal, PowerShell, and terminals with inconsistent box-drawing glyph support.

## Files changed

- `src/UI/SnakeBoardControl.cs`
- `src/UI/TerminalRenderer.cs`
- `src/UI/Menu/MainMenu.cs`
- `src/UI/Menu/OptionsMenu.cs`
- `src/UI/Menu/AboutPage.cs`
- `src/UI/Menu/LeaderboardPage.cs`
- `docs/feature-border-rendering.md`

## Future improvements

- Add a runtime border style option when terminal font capability detection is available.
