using Tessera;
using Tessera.Controls;
using Tessera.Layout;

namespace TesseraSnake.UI.Menu;

internal sealed class OptionsMenu
{
    private readonly Label _content = new()
    {
        Border = BorderStyle.Rounded,
        Title = " Options ",
        Padding = Thickness.Symmetric(2),
        TextStyle = SnakeTheme.PanelText,
        BorderStyleText = SnakeTheme.Border
    };

    public Screen Build()
    {
        _content.Text = "Difficulty settings arrive in the next feature slice.\n\nPress Escape or B to return.";

        return Screen.Build(window =>
        {
            window.Padding(1);
            window.Body(body => body.Center(_content, 56, 9));
        });
    }
}
