using Tessera;
using Tessera.Styles;
using TesseraSnake.Game;

if (args.Contains("--self-test", StringComparer.OrdinalIgnoreCase))
{
    GameLoop.RunSelfTest();
    Console.WriteLine("Self-test OK");
    return;
}

var app = TesseraApplication.CreateBuilder()
    .UseApp<GameLoop>()
    .ConfigureRuntime(static runtime =>
    {
        runtime.MaxFps = 60;
        runtime.AdaptiveFramePacing = true;
        runtime.Theme = TesseraThemes.Catppuccin(CatppuccinVariant.Mocha);
        runtime.Screen = new ScreenOptions
        {
            AltScreen = true,
            WindowTitle = "Tessera Snake",
            EnableFocusReporting = true,
            EnableSynchronizedUpdates = true,
            ForegroundColor = "#CDD6F4",
            BackgroundColor = "#11111B",
            CursorColor = "#A6E3A1"
        };
    })
    .Build();

await app.RunAsync();
