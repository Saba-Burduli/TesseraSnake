using Tessera;
using TesseraSnake.Core.Entities;

namespace TesseraSnake.Game;

internal static class InputHandler
{
    public static GameInput Read(KeyPressed key)
    {
        if (key.IsCharacter('q', ModifierKeys.Ctrl))
        {
            return GameInput.Quit();
        }

        if (key.IsCharacter(' ') || key.Is(Key.Enter))
        {
            return GameInput.Restart();
        }

        if (key.Is(Key.Up) || key.IsCharacter('w'))
        {
            return GameInput.Move(Direction.Up);
        }

        if (key.Is(Key.Down) || key.IsCharacter('s'))
        {
            return GameInput.Move(Direction.Down);
        }

        if (key.Is(Key.Left) || key.IsCharacter('a'))
        {
            return GameInput.Move(Direction.Left);
        }

        if (key.Is(Key.Right) || key.IsCharacter('d'))
        {
            return GameInput.Move(Direction.Right);
        }

        return GameInput.None;
    }
}

internal readonly record struct GameInput(GameInputKind Kind, Direction? Direction = null)
{
    public static GameInput None => new(GameInputKind.None);

    public static GameInput Move(Direction direction)
    {
        return new GameInput(GameInputKind.Move, direction);
    }

    public static GameInput Restart()
    {
        return new GameInput(GameInputKind.Restart);
    }

    public static GameInput Quit()
    {
        return new GameInput(GameInputKind.Quit);
    }
}

internal enum GameInputKind
{
    None,
    Move,
    Restart,
    Quit
}
