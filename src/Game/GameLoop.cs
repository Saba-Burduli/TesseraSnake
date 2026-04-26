using Tessera;
using TesseraSnake.Core.Entities;
using TesseraSnake.Core.Leaderboard;
using TesseraSnake.UI;
using TesseraSnake.UI.Menu;

namespace TesseraSnake.Game;

internal sealed class GameLoop : TesseraApp
{
    private static readonly TimeSpan HeartbeatInterval = TimeSpan.FromMilliseconds(35);

    private readonly SnakeGameState _state = new();
    private readonly LeaderboardService _leaderboard;
    private readonly MainMenu _mainMenu = new();
    private readonly TerminalRenderer _renderer;
    private DateTimeOffset? _lastStep;
    private DifficultyLevel _difficulty = DifficultyLevel.Medium;
    private bool _scoreRecordedForRun;
    private AppScreen _screen = AppScreen.MainMenu;

    public GameLoop() : this(new LeaderboardService())
    {
    }

    private GameLoop(LeaderboardService leaderboard)
    {
        _leaderboard = leaderboard;
        _renderer = new TerminalRenderer(_mainMenu);
    }

    public override TesseraEffect? Initialize()
    {
        return TesseraEffects.Periodic(HeartbeatInterval, now => new GameTick(now));
    }

    public override TesseraEffect? Update(Message message)
    {
        return message switch
        {
            GameTick tick => UpdateGame(tick),
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
            AppScreen.Leaderboard => _renderer.BuildLeaderboard(_leaderboard.Entries),
            AppScreen.About => _renderer.BuildAbout(),
            _ => _renderer.Build(_state, context, _difficulty)
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
        _ = new TerminalRenderer(menu).Build(state, new ScreenContext { Width = 80, Height = 30 },
            DifficultyLevel.Medium);

        var tempPath = Path.Combine(Path.GetTempPath(), $"tessera-snake-{Guid.NewGuid():N}.json");
        var leaderboard = new LeaderboardService(tempPath);
        leaderboard.AddScore(3, DateTimeOffset.UtcNow);
        leaderboard.AddScore(9, DateTimeOffset.UtcNow);
        if (leaderboard.Entries[0].Score != 9)
        {
            throw new InvalidOperationException("Leaderboard sorting failed.");
        }

        File.Delete(tempPath);
    }

    private TesseraEffect? UpdateGame(GameTick tick)
    {
        if (_screen != AppScreen.Playing)
        {
            return null;
        }

        if (_lastStep is { } last && tick.At - last < DifficultySettings.TickInterval(_difficulty))
        {
            return null;
        }

        _lastStep = tick.At;
        var wasGameOver = _state.IsGameOver;
        _state.Tick();
        if (!wasGameOver && _state.IsGameOver)
        {
            RecordScore(tick.At);
        }

        return null;
    }

    private TesseraEffect? HandleKey(KeyPressed key)
    {
        return _screen switch
        {
            AppScreen.MainMenu => HandleMainMenuKey(key),
            AppScreen.Options => HandleOptionsKey(key),
            AppScreen.Leaderboard or AppScreen.About => HandlePageKey(key),
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
                _lastStep = null;
                _scoreRecordedForRun = false;
                _screen = AppScreen.Playing;
                return null;
            case "Options":
                _renderer.OptionsMenu.SelectDifficulty(_difficulty);
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

    private TesseraEffect? HandleOptionsKey(KeyPressed key)
    {
        if (key.Is(Key.Escape) || key.IsCharacter('b') || key.IsCharacter('q', ModifierKeys.Ctrl))
        {
            _screen = AppScreen.MainMenu;
            return null;
        }

        var options = _renderer.OptionsMenu;
        if (key.Is(Key.Up) || key.IsCharacter('w'))
        {
            options.MovePrevious();
            return null;
        }

        if (key.Is(Key.Down) || key.IsCharacter('s'))
        {
            options.MoveNext();
            return null;
        }

        if (key.Is(Key.Enter) || key.IsCharacter(' '))
        {
            if (options.IsLeaderboardSelected)
            {
                _screen = AppScreen.Leaderboard;
                return null;
            }

            if (options.IsBackSelected)
            {
                _screen = AppScreen.MainMenu;
                return null;
            }

            _difficulty = options.SelectedDifficulty;
            return null;
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
                    _lastStep = null;
                    _scoreRecordedForRun = false;
                }

                return null;
            case GameInputKind.Move when input.Direction is { } direction:
                _state.QueueDirection(direction);
                return null;
            default:
                return null;
        }
    }

    private void RecordScore(DateTimeOffset recordedAt)
    {
        if (_scoreRecordedForRun)
        {
            return;
        }

        _leaderboard.AddScore(_state.Score, recordedAt);
        _scoreRecordedForRun = true;
    }

    private sealed record GameTick(DateTimeOffset At) : Message;
}
