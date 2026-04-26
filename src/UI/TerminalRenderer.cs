using Tessera;
using Tessera.Controls;
using Tessera.Layout;
using Tessera.Styles;
using TesseraSnake.Core.Entities;

namespace TesseraSnake.UI;

internal sealed class TerminalRenderer
{
    private readonly SnakeBoardControl _board = new();

    private readonly Label _header = new()
    {
        Border = BorderStyle.None,
        HorizontalAlignment = HorizontalAlignment.Center,
        TextStyle = SnakeTheme.Title
    };

    private readonly Label _help = new()
    {
        Border = BorderStyle.Rounded,
        Title = " controls ",
        Padding = Thickness.Symmetric(1),
        TextStyle = SnakeTheme.PanelText,
        BorderStyleText = SnakeTheme.Border
    };

    private readonly Label _stats = new()
    {
        Border = BorderStyle.Rounded,
        Title = " run ",
        Padding = Thickness.Symmetric(1),
        TextStyle = SnakeTheme.PanelText,
        BorderStyleText = SnakeTheme.Border
    };

    private readonly StatusBar _status = new() { Fill = ' ' };

    public Screen Build(SnakeGameState state, ScreenContext context)
    {
        _board.State = state;
        _header.Text = "TESSERA SNAKE";
        _help.Text = "Move: arrows / WASD\nRestart: Space / Enter\nQuit: Ctrl+Q\n\nFood grows the snake.\nWalls and self hits end the run.";
        _stats.Text = $"Score       {state.Score:D3}\nLength      {state.Snake.Count:D3}\nDirection   {state.CurrentDirection}\nBoard       {state.Width} x {state.Height}";
        _status.LeftText = $" Score {state.Score:D3}   Length {state.Snake.Count:D3} ";
        _status.RightText = StatusText(state);
        _status.LeftTextStyle = SnakeTheme.StatusLeft;
        _status.RightTextStyle = SnakeTheme.StatusRight;
        _status.FillStyle = SnakeTheme.StatusFill;

        return Screen.Build(window =>
        {
            window.Padding(1);
            window.Header(1, _header);
            if (context.Width >= 102)
            {
                window.Right(30, side => side.Column(column =>
                {
                    column.Gap(1);
                    column.Fixed(8, _stats);
                    column.Fixed(11, _help);
                }));
            }

            window.Body(body => body.Center(_board, state.Width * 2 + 4, state.Height + 4));
            window.Footer(1, _status);
        });
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
