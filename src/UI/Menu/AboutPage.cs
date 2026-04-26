using Tessera;
using Tessera.Controls;
using Tessera.Layout;

namespace TesseraSnake.UI.Menu;

internal sealed class AboutPage
{
    private readonly Label _content = new()
    {
        Border = BorderStyle.Rounded,
        Title = " About Developer ",
        Padding = Thickness.Symmetric(2),
        TextStyle = SnakeTheme.PanelText,
        BorderStyleText = SnakeTheme.Border
    };

    public Screen Build()
    {
        _content.Text = "Tessera Snake\nBuilt with C# and Tessera.\n\nDeveloper page content will be expanded in its feature slice.\n\nPress Escape or B to return.";

        return Screen.Build(window =>
        {
            window.Padding(1);
            window.Body(body => body.Center(_content, 64, 11));
        });
    }
}
