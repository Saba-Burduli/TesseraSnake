# Feature: Widget Showcase

## What was added

- Added a `Tessera Widgets` main-menu page.
- Integrated representative built-in Tessera controls:
  - `Button`
  - `Badge`
  - `Tabs`
  - `ListView`
  - `KeyValueList`
  - `StatsCard`
  - `ProgressBar`
  - `Gauge`
  - `Sparkline`
  - `Label`
  - `StatusBar`
- Connected widget values to current game state, difficulty, and leaderboard data.

## Why it exists

The game should demonstrate Tessera as a broader TUI framework, not only a custom-rendered Snake board.

## Files changed

- `src/Game/AppScreen.cs`
- `src/Game/GameLoop.cs`
- `src/UI/Menu/MainMenu.cs`
- `src/UI/Menu/WidgetShowcasePage.cs`
- `src/UI/TerminalRenderer.cs`
- `docs/feature-widget-showcase.md`

## Next improvements

- Add more specialized widgets when they map naturally to gameplay features.
- Add keyboard focus management for interactive controls inside the showcase.
