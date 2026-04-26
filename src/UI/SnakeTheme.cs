using Tessera.Styles;

namespace TesseraSnake.UI;

internal static class SnakeTheme
{
    public static TesseraStyle Title => Foreground(0xA6E3A1).WithBold();
    public static TesseraStyle Border => Foreground(0x74C7EC);
    public static TesseraStyle PanelText => Foreground(0xCDD6F4);

    public static TesseraStyle FieldEven => Surface(0x11111B, 0x181825);
    public static TesseraStyle FieldOdd => Surface(0x11111B, 0x1E1E2E);
    public static TesseraStyle Head => Surface(0x11111B, 0xF9E2AF).WithBold();
    public static TesseraStyle Body => Surface(0x11111B, 0xA6E3A1);
    public static TesseraStyle Food => Surface(0x11111B, 0xF38BA8).WithBold();
    public static TesseraStyle Overlay => Surface(0x11111B, 0xF38BA8).WithBold();
    public static TesseraStyle OverlayDetail => Surface(0x11111B, 0xF9E2AF).WithBold();

    public static TesseraStyle StatusLeft => Surface(0x11111B, 0xA6E3A1).WithBold();
    public static TesseraStyle StatusRight => Foreground(0xCDD6F4);
    public static TesseraStyle StatusFill => Background(0x181825);

    private static TesseraStyle Foreground(int color)
    {
        var (red, green, blue) = Split(color);
        return TesseraStyle.Empty.WithForeground(AnsiColor.Rgb(red, green, blue));
    }

    private static TesseraStyle Background(int color)
    {
        var (red, green, blue) = Split(color);
        return TesseraStyle.Empty.WithBackground(AnsiColor.Rgb(red, green, blue));
    }

    private static TesseraStyle Surface(int foreground, int background)
    {
        var (fgRed, fgGreen, fgBlue) = Split(foreground);
        var (bgRed, bgGreen, bgBlue) = Split(background);
        return TesseraStyle.Empty
            .WithForeground(AnsiColor.Rgb(fgRed, fgGreen, fgBlue))
            .WithBackground(AnsiColor.Rgb(bgRed, bgGreen, bgBlue));
    }

    private static (byte Red, byte Green, byte Blue) Split(int color)
    {
        return (
            (byte)((color >> 16) & 0xFF),
            (byte)((color >> 8) & 0xFF),
            (byte)(color & 0xFF));
    }
}
