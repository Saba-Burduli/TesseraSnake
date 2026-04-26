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
        _content.Text =
            "Developer       Saba Burduli\n" +
            "Project         Tessera Snake\n" +
            "Stack           C# / .NET 10 / Tessera\n" +
            "Repository      Saba-Burduli/tessera-snake\n\n" +
            "This project is a real-time terminal Snake game built around Tessera's app model,\n" +
            "periodic effects, keyboard messages, custom controls, themed rendering, and\n" +
            "alternate-screen terminal output.\n\n" +
            "Press Escape or B to return.";

        return Screen.Build(window =>
        {
            window.Padding(1);
            window.Body(body => body.Center(_content, 82, 15));
        });
    }
}
