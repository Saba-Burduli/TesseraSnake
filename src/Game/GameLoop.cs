using Tessera;
using TesseraSnake.Core.Entities;
using TesseraSnake.UI;

namespace TesseraSnake.Game;

internal sealed class GameLoop : TesseraApp
{
    private static readonly TimeSpan TickInterval = TimeSpan.FromMilliseconds(110);

    private readonly SnakeGameState _state = new();
    private readonly TerminalRenderer _renderer = new();

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
        return _renderer.Build(_state, context);
    }

    public static void RunSelfTest()
    {
        var state = new SnakeGameState(width: 10, height: 8);
        state.QueueDirection(Direction.Down);
        state.Tick();

        _ = TerminalRenderer.RenderBoard(state);
    }

    private TesseraEffect? UpdateGame()
    {
        _state.Tick();
        return null;
    }

    private TesseraEffect? HandleKey(KeyPressed key)
    {
        var input = InputHandler.Read(key);
        switch (input.Kind)
        {
            case GameInputKind.Quit:
                return TesseraEffects.Quit;
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
