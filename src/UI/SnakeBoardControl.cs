using Tessera;
using Tessera.Components.Primitives;
using Tessera.Controls;
using Tessera.Styles;
using TesseraSnake.Core.Entities;

namespace TesseraSnake.UI;

internal sealed class SnakeBoardControl : Control
{
    private const int CellWidth = 2;

    public SnakeGameState? State { get; set; }

    public BorderStyle Border { get; set; } = BorderStyle.Rounded;

    public TesseraStyle BorderStyleText { get; set; } = SnakeTheme.Border;

    public override void Render(Canvas canvas, Rect rect)
    {
        var state = State;
        var clipped = Rect.Intersect(rect, canvas.Bounds);
        if (state is null || clipped.IsEmpty)
        {
            return;
        }

        canvas.DrawBox(clipped, SnakeTheme.Title.Render(" Tessera Snake "), Border, BorderStyleText);

        var content = clipped.Inset(1, 1);
        if (content.IsEmpty)
        {
            return;
        }

        var boardWidth = state.Width * CellWidth;
        var boardHeight = state.Height;
        var originX = content.X + Math.Max(0, (content.Width - boardWidth) / 2);
        var originY = content.Y + Math.Max(0, (content.Height - boardHeight) / 2);
        var renderWidth = Math.Min(boardWidth, Math.Max(0, content.Right - originX));
        var renderHeight = Math.Min(boardHeight, Math.Max(0, content.Bottom - originY));

        DrawField(canvas, state, originX, originY, renderWidth, renderHeight);
        DrawFood(canvas, state, originX, originY, renderWidth, renderHeight);
        DrawSnake(canvas, state, originX, originY, renderWidth, renderHeight);

        if (state.IsGameOver)
        {
            DrawGameOver(canvas, state, content);
        }
    }

    private static void DrawField(Canvas canvas, SnakeGameState state, int originX, int originY, int width, int height)
    {
        for (var y = 0; y < height && y < state.Height; y++)
        {
            for (var x = 0; x < state.Width; x++)
            {
                var cellX = originX + x * CellWidth;
                if (cellX + CellWidth > originX + width)
                {
                    break;
                }

                var style = (x + y) % 2 == 0 ? SnakeTheme.FieldEven : SnakeTheme.FieldOdd;
                WriteCell(canvas, cellX, originY + y, "  ", style);
            }
        }
    }

    private static void DrawFood(Canvas canvas, SnakeGameState state, int originX, int originY, int width, int height)
    {
        if (!IsVisible(state.Food, width, height))
        {
            return;
        }

        WriteCell(canvas, originX + state.Food.X * CellWidth, originY + state.Food.Y, "[]", SnakeTheme.Food);
    }

    private static void DrawSnake(Canvas canvas, SnakeGameState state, int originX, int originY, int width, int height)
    {
        for (var index = state.Snake.Count - 1; index >= 0; index--)
        {
            var segment = state.Snake[index];
            if (!IsVisible(segment, width, height))
            {
                continue;
            }

            var text = index == 0 ? HeadText(state.CurrentDirection) : "  ";
            var style = index == 0 ? SnakeTheme.Head : SnakeTheme.Body;
            WriteCell(canvas, originX + segment.X * CellWidth, originY + segment.Y, text, style);
        }
    }

    private static void DrawGameOver(Canvas canvas, SnakeGameState state, Rect content)
    {
        var headline = state.HasWon ? " BOARD CLEARED " : " GAME OVER ";
        var detail = " Space / Enter restarts ";
        var headlineX = content.X + Math.Max(0, (content.Width - headline.Length) / 2);
        var detailX = content.X + Math.Max(0, (content.Width - detail.Length) / 2);
        var y = content.Y + Math.Max(0, content.Height / 2 - 1);

        canvas.WriteText(headlineX, y, SnakeTheme.Overlay.Render(headline), content.Width);
        canvas.WriteText(detailX, y + 1, SnakeTheme.OverlayDetail.Render(detail), content.Width);
    }

    private static bool IsVisible(GridPoint point, int renderWidth, int renderHeight)
    {
        return point.Y >= 0 && point.Y < renderHeight && point.X * CellWidth + CellWidth <= renderWidth;
    }

    private static string HeadText(Direction direction)
    {
        return direction switch
        {
            Direction.Up => "^^",
            Direction.Down => "vv",
            Direction.Left => "<<",
            Direction.Right => ">>",
            _ => ">>"
        };
    }

    private static void WriteCell(Canvas canvas, int x, int y, string text, TesseraStyle style)
    {
        canvas.WriteText(x, y, style.Render(text), CellWidth);
    }
}
