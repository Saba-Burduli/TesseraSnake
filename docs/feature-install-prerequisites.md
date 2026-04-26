# Feature: Install Prerequisites

## What was added

- Added project README guidance for installing and running with Tessera.
- Documented the .NET 10 SDK requirement.
- Documented recommended terminals with ANSI/CSI support.
- Recorded the default Tessera namespaces used by app code.

## Why it exists

The game depends on Tessera's terminal runtime and rendering model. Clear setup notes make the project reproducible on a compatible terminal.

## Files changed

- `README.md`
- `tessera-snake.csproj`
- `docs/feature-tessera-setup.md`
- `docs/feature-install-prerequisites.md`

## Next improvements

- Add screenshots or terminal capture after visual styling is added.
- Recheck the Tessera package version when a stable release is available.
