using Tessera;
using TesseraSnake.Core.Entities;
using TesseraSnake.UI;
using TesseraSnake.UI.Menu;

namespace TesseraSnake.Game;

internal sealed class GameLoop : TesseraApp
{
    private static readonly TimeSpan TickInterval = TimeSpan.FromMilliseconds(110);

    private readonly SnakeGameState _state = new();
    private readonly MainMenu _mainMenu = new();
    private readonly TerminalRenderer _renderer;
    private AppScreen _screen = AppScreen.MainMenu;

    public GameLoop()
    {
        _renderer = new TerminalRenderer(_mainMenu);
    }

    public override TesseraEffect? Initialize()
    {
        return TesseraEffects.Periodic(TickInterval, _ => new GameTick());
    }

    public override TesseraEffect? Update(Message message)
    {
        return message switch
        {
            GameTick => UpdateGame(),
            KeyPressed key => HandleKey(key),
            _ => null
        };
    }

    public override Screen Build(ScreenContext context)
    {
        return _screen switch
        {
            AppScreen.MainMenu => _renderer.BuildMainMenu(),
            AppScreen.Options => _renderer.BuildOptions(),
            AppScreen.About => _renderer.BuildAbout(),
            _ => _renderer.Build(_state, context)
        };
    }

    public static void RunSelfTest()
    {
        var state = new SnakeGameState(width: 10, height: 8);
        state.QueueDirection(Direction.Down);
        state.Tick();

        var menu = new MainMenu();
        menu.MoveNext();
        _ = new TerminalRenderer(menu).BuildMainMenu();
        _ = new TerminalRenderer(menu).Build(state, new ScreenContext { Width = 80, Height = 30 });
    }

    private TesseraEffect? UpdateGame()
    {
        if (_screen != AppScreen.Playing)
        {
            return null;
        }

        _state.Tick();
        return null;
    }

    private TesseraEffect? HandleKey(KeyPressed key)
    {
        return _screen switch
        {
            AppScreen.MainMenu => HandleMainMenuKey(key),
            AppScreen.Options or AppScreen.About => HandlePageKey(key),
            _ => HandleGameKey(key)
        };
    }

    private TesseraEffect? HandleMainMenuKey(KeyPressed key)
    {
        if (key.Is(Key.Up) || key.IsCharacter('w'))
        {
            _mainMenu.MovePrevious();
            return null;
        }

        if (key.Is(Key.Down) || key.IsCharacter('s'))
        {
            _mainMenu.MoveNext();
            return null;
        }

        if (key.Is(Key.Enter) || key.IsCharacter(' '))
        {
            return ActivateMainMenuItem();
        }

        if (key.IsCharacter('q', ModifierKeys.Ctrl))
        {
            return TesseraEffects.Quit;
        }

        return null;
    }

    private TesseraEffect? ActivateMainMenuItem()
    {
        switch (_mainMenu.SelectedItem)
        {
            case "Start Game":
                _state.Reset();
                _screen = AppScreen.Playing;
                return null;
            case "Options":
                _screen = AppScreen.Options;
                return null;
            case "About Developer":
                _screen = AppScreen.About;
                return null;
            case "Exit":
                return TesseraEffects.Quit;
            default:
                return null;
        }
    }

    private TesseraEffect? HandlePageKey(KeyPressed key)
    {
        if (key.Is(Key.Escape) || key.IsCharacter('b') || key.IsCharacter('q', ModifierKeys.Ctrl))
        {
            _screen = AppScreen.MainMenu;
        }

        return null;
    }

    private TesseraEffect? HandleGameKey(KeyPressed key)
    {
        var input = InputHandler.Read(key);
        switch (input.Kind)
        {
            case GameInputKind.Quit:
                _screen = AppScreen.MainMenu;
                return null;
            case GameInputKind.Restart:
                if (_state.IsGameOver)
                {
                    _state.Reset();
                }

                return null;
            case GameInputKind.Move when input.Direction is { } direction:
                _state.QueueDirection(direction);
                return null;
            default:
                return null;
        }
    }

    private sealed record GameTick : Message;
}
