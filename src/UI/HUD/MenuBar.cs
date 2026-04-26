using Tessera.Components.Primitives;
using Tessera.Controls;
using Tessera.Styles;
using TesseraSnake.Game;

namespace TesseraSnake.UI.HUD;

internal sealed class MenuBar : Control
{
    public int Score { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public GameStatus Status { get; set; }

    public override void Render(Canvas canvas, Rect rect)
    {
        var clipped = Rect.Intersect(rect, canvas.Bounds);
        if (clipped.IsEmpty || clipped.Height < 1)
        {
            return;
        }

        canvas.WriteText(clipped.X, clipped.Y, SnakeTheme.StatusFill.Render(new string(' ', clipped.Width)),
            clipped.Width);

        var left = $" Score {Score:D3} ";
        var middle = $" Difficulty {Difficulty} ";
        var right = $" Status {StatusText()} ";

        canvas.WriteText(clipped.X, clipped.Y, SnakeTheme.StatusLeft.Render(left), clipped.Width);
        canvas.WriteText(clipped.X + Math.Max(0, (clipped.Width - middle.Length) / 2), clipped.Y,
            SnakeTheme.StatusMiddle.Render(middle), clipped.Width);
        canvas.WriteText(clipped.X + Math.Max(0, clipped.Width - right.Length), clipped.Y,
            StatusStyle().Render(right), clipped.Width);
    }

    private string StatusText()
    {
        return Status switch
        {
            GameStatus.GameOver => "Game Over",
            _ => Status.ToString()
        };
    }

    private TesseraStyle StatusStyle()
    {
        return Status switch
        {
            GameStatus.Paused => SnakeTheme.StatusPaused,
            GameStatus.GameOver => SnakeTheme.StatusGameOver,
            _ => SnakeTheme.StatusRight
        };
    }
}
