# Feature: Main Menu

## What was added

- Added a Tessera-rendered main menu with Start Game, Options, About Developer, and Exit.
- Added keyboard navigation with arrows/WASD.
- Added Enter/Space activation.
- Added simple Options and About pages with Escape/B navigation back to the main menu.
- Changed Ctrl+Q while playing to return to the main menu.

## Why it exists

The game needs a structured entry point before starting gameplay and a navigation model for future options, leaderboard, and about pages.

## Files changed

- `src/Game/AppScreen.cs`
- `src/Game/GameLoop.cs`
- `src/UI/TerminalRenderer.cs`
- `src/UI/SnakeTheme.cs`
- `src/UI/Menu/MainMenu.cs`
- `src/UI/Menu/OptionsMenu.cs`
- `src/UI/Menu/AboutPage.cs`
- `docs/feature-main-menu.md`

## Future improvements

- Replace the placeholder Options content with difficulty selection.
- Add a leaderboard screen.
- Expand the About Developer page with final developer details.
