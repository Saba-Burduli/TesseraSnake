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
- Added keyboard focus management across the widget page.
- Added responsive widget layouts so borders and panels do not collide on narrower terminals.
- Wired widget actions:
  - `Tab` moves focus forward.
  - `[` moves focus backward.
  - arrows/WASD update focused tabs and lists.
  - `Enter`/`Space` activates the focused button.

## Why it exists

The game should demonstrate Tessera as a broader TUI framework, not only a custom-rendered Snake board.
The widget page also needs to behave like a real terminal screen, not a static screenshot.

## Files changed

- `src/Game/AppScreen.cs`
- `src/Game/GameLoop.cs`
- `src/UI/Menu/MainMenu.cs`
- `src/UI/Menu/WidgetShowcasePage.cs`
- `src/UI/TerminalRenderer.cs`
- `docs/feature-widget-showcase.md`

## Next improvements

- Add more specialized widgets when they map naturally to gameplay features.
- Add mouse/pointer validation if the runtime exposes pointer input consistently across terminals.
