# Tessera Snake

A real-time terminal Snake game built in C# with Tessera.

## Prerequisites

- .NET 10 SDK
- Terminal with reliable ANSI/CSI support, such as Windows Terminal, Ghostty, iTerm2, or macOS Terminal

## Tessera

This project uses the primary Tessera app namespaces:

```csharp
using Tessera;
using Tessera.Controls;
using Tessera.Layout;
```

The project is pinned to the currently available NuGet package version:

```xml
<PackageReference Include="Tessera" Version="1.0.0-alpha.1" />
```

For a new project, Tessera's documented install path is:

```powershell
dotnet new console -n MyApp -f net10.0
cd MyApp
dotnet add package Tessera
```

## Build

```powershell
dotnet build
```

## Run

```powershell
dotnet run
```

## Verify

```powershell
dotnet run -- --self-test
```

## Controls

- Arrow keys or WASD: move
- Space: restart after game over
- Ctrl+Q: quit
