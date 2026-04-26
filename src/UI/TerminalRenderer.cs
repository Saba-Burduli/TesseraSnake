using Tessera;
using Tessera.Controls;
using Tessera.Layout;
using Tessera.Styles;
using TesseraSnake.Core.Entities;
using TesseraSnake.Game;
using TesseraSnake.UI.Menu;

namespace TesseraSnake.UI;

internal sealed class TerminalRenderer
{
    private readonly SnakeBoardControl _board = new();
    private readonly MainMenu _mainMenu;
    private readonly OptionsMenu _optionsMenu = new();
    private readonly AboutPage _aboutPage = new();

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

    public TerminalRenderer(MainMenu mainMenu)
    {
        _mainMenu = mainMenu;
    }

    public Screen BuildMainMenu()
    {
        return Screen.Build(window =>
        {
            window.Padding(1);
            window.Body(body => body.Center(_mainMenu, 58, 18));
        });
    }

    public OptionsMenu OptionsMenu => _optionsMenu;

    public Screen BuildOptions()
    {
        return Screen.Build(window =>
        {
            window.Padding(1);
            window.Body(body => body.Center(_optionsMenu, 58, 18));
        });
    }

    public Screen BuildAbout()
    {
        return _aboutPage.Build();
    }

    public Screen Build(SnakeGameState state, ScreenContext context, DifficultyLevel difficulty)
    {
        _board.State = state;
        _header.Text = "TESSERA SNAKE";
        _help.Text = "Move: arrows / WASD\nRestart: Space / Enter\nQuit: Ctrl+Q\n\nFood grows the snake.\nWalls and self hits end the run.";
        _stats.Text = $"Score       {state.Score:D3}\nLength      {state.Snake.Count:D3}\nDifficulty  {difficulty}\nDirection   {state.CurrentDirection}\nBoard       {state.Width} x {state.Height}";
        _status.LeftText = $" Score {state.Score:D3}   Length {state.Snake.Count:D3}   Difficulty {difficulty} ";
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
