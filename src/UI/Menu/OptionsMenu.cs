using Tessera;
using Tessera.Components.Primitives;
using Tessera.Controls;
using TesseraSnake.Game;

namespace TesseraSnake.UI.Menu;

internal sealed class OptionsMenu : Control
{
    private readonly DifficultyLevel[] _difficulties =
    [
        DifficultyLevel.Easy,
        DifficultyLevel.Medium,
        DifficultyLevel.Hard
    ];

    public int SelectedIndex { get; private set; }

    public DifficultyLevel SelectedDifficulty => _difficulties[Math.Clamp(SelectedIndex, 0, _difficulties.Length - 1)];

    public bool IsLeaderboardSelected => SelectedIndex == _difficulties.Length;

    public bool IsBackSelected => SelectedIndex == _difficulties.Length + 1;

    public void MovePrevious()
    {
        SelectedIndex = (SelectedIndex - 1 + _difficulties.Length + 2) % (_difficulties.Length + 2);
    }

    public void MoveNext()
    {
        SelectedIndex = (SelectedIndex + 1) % (_difficulties.Length + 2);
    }

    public void SelectDifficulty(DifficultyLevel difficulty)
    {
        var index = Array.IndexOf(_difficulties, difficulty);
        if (index >= 0)
        {
            SelectedIndex = index;
        }
    }

    public override void Render(Canvas canvas, Rect rect)
    {
        var clipped = Rect.Intersect(rect, canvas.Bounds);
        if (clipped.IsEmpty)
        {
            return;
        }

        canvas.DrawBox(clipped, SnakeTheme.Title.Render(" Options "), BorderStyle.Rounded, SnakeTheme.Border);
        var content = clipped.Inset(2, 1);
        if (content.IsEmpty)
        {
            return;
        }

        var title = "Difficulty";
        canvas.WriteText(content.X + Math.Max(0, (content.Width - title.Length) / 2), content.Y + 1,
            SnakeTheme.Title.Render(title), content.Width);

        var startY = content.Y + 4;
        for (var index = 0; index < _difficulties.Length; index++)
        {
            var text = _difficulties[index].ToString();
            DrawOption(canvas, content, startY + index * 2, index, text);
        }

        DrawOption(canvas, content, startY + _difficulties.Length * 2 + 1, _difficulties.Length, "Leaderboard");
        DrawOption(canvas, content, startY + _difficulties.Length * 2 + 3, _difficulties.Length + 1, "Back");

        var hint = "Arrows/WASD select   Enter/Space apply";
        canvas.WriteText(content.X + Math.Max(0, (content.Width - hint.Length) / 2), content.Bottom - 2,
            SnakeTheme.MenuHint.Render(hint), content.Width);
    }

    private void DrawOption(Canvas canvas, Rect content, int y, int index, string label)
    {
        var selected = SelectedIndex == index;
        var text = selected ? $"> {label} <" : $"  {label}  ";
        var style = selected ? SnakeTheme.MenuSelected : SnakeTheme.MenuItem;
        canvas.WriteText(content.X + Math.Max(0, (content.Width - text.Length) / 2), y, style.Render(text),
            content.Width);
    }
}
