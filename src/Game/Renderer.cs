using System.Text;
using TesseraSnake.Core.Entities;

namespace TesseraSnake.Game;

internal static class Renderer
{
    public static string RenderBoard(SnakeGameState state)
    {
        var cells = CreateEmptyBoard(state.Width, state.Height);

        cells[state.Food.Y, state.Food.X] = '*';

        for (var index = state.Snake.Count - 1; index >= 0; index--)
        {
            var segment = state.Snake[index];
            cells[segment.Y, segment.X] = index == 0 ? HeadGlyph(state.CurrentDirection) : 'o';
        }

        var output = new StringBuilder((state.Width + 1) * state.Height);
        for (var y = 0; y < state.Height; y++)
        {
            if (y > 0)
            {
                output.AppendLine();
            }

            for (var x = 0; x < state.Width; x++)
            {
                output.Append(cells[y, x]);
            }
        }

        return output.ToString();
    }

    private static char[,] CreateEmptyBoard(int width, int height)
    {
        var cells = new char[height, width];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                cells[y, x] = ' ';
            }
        }

        return cells;
    }

    private static char HeadGlyph(Direction direction)
    {
        return direction switch
        {
            Direction.Up => '^',
            Direction.Down => 'v',
            Direction.Left => '<',
            Direction.Right => '>',
            _ => '>'
        };
    }
}
