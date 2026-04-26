# Feature: Local Deploy

## What was added

- Documented the local Release publish command.
- Ignored the generated `/deploy` output folder.
- Verified the published executable with `--self-test`.

## Why it was added

The project needs a clear local deployment path that runs outside `dotnet run`.

## Files changed

- `.gitignore`
- `README.md`
- `docs/feature-local-deploy.md`

## Future improvements

- Add a self-contained Windows publish profile.
- Add zipped release packaging.
