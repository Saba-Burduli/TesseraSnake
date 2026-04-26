using Tessera;
using Tessera.Controls;
using Tessera.Layout;
using TesseraSnake.Core.Entities;
using TesseraSnake.Game;

namespace TesseraSnake.UI;

internal sealed class TerminalRenderer
{
    private readonly Label _board = new()
    {
        Border = BorderStyle.Ascii,
        Title = " Tessera Snake ",
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Top
    };

    private readonly StatusBar _status = new() { Fill = ' ' };

    public Screen Build(SnakeGameState state, ScreenContext context)
    {
        _board.Text = RenderBoard(state);
        _status.LeftText = $"Score: {state.Score}";
        _status.RightText = StatusText(state);

        return Screen.Build(window =>
        {
            window.Padding(1);
            window.Body(body => body.Center(_board, state.Width + 2, state.Height + 2));
            window.Footer(1, _status);
        });
    }

    public static string RenderBoard(SnakeGameState state)
    {
        return Renderer.RenderBoard(state);
    }

    private static string StatusText(SnakeGameState state)
    {
        if (state.HasWon)
        {
            return "You filled the board. Space restarts | Ctrl+Q quits";
        }

        if (state.IsGameOver)
        {
            return "Game over. Space restarts | Ctrl+Q quits";
        }

        return "Arrows/WASD move | Ctrl+Q quits";
    }
}
