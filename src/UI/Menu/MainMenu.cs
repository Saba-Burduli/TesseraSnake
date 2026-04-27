using Tessera;
using Tessera.Components.Primitives;
using Tessera.Controls;
using Tessera.Styles;

namespace TesseraSnake.UI.Menu;

internal sealed class MainMenu : Control
{
    private readonly string[] _items = ["Start Game", "Options", "About Developer", "Exit"];

    public int SelectedIndex { get; private set; }

    public string SelectedItem => _items[SelectedIndex];

    public void MovePrevious()
    {
        SelectedIndex = (SelectedIndex - 1 + _items.Length) % _items.Length;
    }

    public void MoveNext()
    {
        SelectedIndex = (SelectedIndex + 1) % _items.Length;
    }

    public override void Render(Canvas canvas, Rect rect)
    {
        var clipped = Rect.Intersect(rect, canvas.Bounds);
        if (clipped.IsEmpty)
        {
            return;
        }

        canvas.DrawBox(clipped, SnakeTheme.Title.Render(" Main Menu "), BorderStyle.Ascii, SnakeTheme.Border);
        var content = clipped.Inset(2, 1);
        if (content.IsEmpty)
        {
            return;
        }

        var title = SnakeTheme.Title.Render("TESSERA SNAKE");
        canvas.WriteText(content.X + Math.Max(0, (content.Width - "TESSERA SNAKE".Length) / 2), content.Y + 1, title,
            content.Width);

        var startY = content.Y + 4;
        for (var index = 0; index < _items.Length; index++)
        {
            var selected = index == SelectedIndex;
            var prefix = selected ? "> " : "  ";
            var suffix = selected ? " <" : "  ";
            var text = $"{prefix}{_items[index]}{suffix}";
            var style = selected ? SnakeTheme.MenuSelected : SnakeTheme.MenuItem;
            var x = content.X + Math.Max(0, (content.Width - text.Length) / 2);
            canvas.WriteText(x, startY + index * 2, style.Render(text), content.Width);
        }

        var hint = "Arrows/WASD select   Enter/Space confirm";
        canvas.WriteText(content.X + Math.Max(0, (content.Width - hint.Length) / 2), content.Bottom - 2,
            SnakeTheme.MenuHint.Render(hint), content.Width);
    }
}
