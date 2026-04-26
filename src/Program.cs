using Tessera;
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
        runtime.Screen = new ScreenOptions
        {
            AltScreen = true,
            WindowTitle = "Tessera Snake",
            EnableFocusReporting = true
        };
    })
    .Build();

await app.RunAsync();
