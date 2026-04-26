namespace TesseraSnake.Core.Entities;

internal sealed class SnakeGameState
{
    private readonly Random _random = new();
    private readonly List<GridPoint> _snake = [];

    public SnakeGameState(int width = 32, int height = 18)
    {
        Width = width;
        Height = height;
        Reset();
    }

    public int Width { get; }
    public int Height { get; }
    public int Score { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool HasWon { get; private set; }
    public Direction CurrentDirection { get; private set; }
    public Direction PendingDirection { get; private set; }
    public GridPoint Food { get; private set; }
    public IReadOnlyList<GridPoint> Snake => _snake;
    public GridPoint Head => _snake[0];

    public void Reset()
    {
        _snake.Clear();

        var centerX = Width / 2;
        var centerY = Height / 2;
        _snake.Add(new GridPoint(centerX, centerY));
        _snake.Add(new GridPoint(centerX - 1, centerY));
        _snake.Add(new GridPoint(centerX - 2, centerY));

        Score = 0;
        IsGameOver = false;
        HasWon = false;
        CurrentDirection = Direction.Right;
        PendingDirection = Direction.Right;
        SpawnFood();
    }

    public void QueueDirection(Direction direction)
    {
        if (IsOpposite(CurrentDirection, direction))
        {
            return;
        }

        PendingDirection = direction;
    }

    public void Tick()
    {
        if (IsGameOver)
        {
            return;
        }

        CurrentDirection = PendingDirection;
        var nextHead = Head.Move(CurrentDirection);
        if (IsOutside(nextHead))
        {
            IsGameOver = true;
            return;
        }

        var grows = nextHead == Food;
        var collisionBody = grows ? _snake : _snake.Take(_snake.Count - 1);
        if (collisionBody.Contains(nextHead))
        {
            IsGameOver = true;
            return;
        }

        _snake.Insert(0, nextHead);
        if (grows)
        {
            Score++;
            SpawnFood();
            return;
        }

        _snake.RemoveAt(_snake.Count - 1);
    }

    private void SpawnFood()
    {
        var emptyCells = new List<GridPoint>(Width * Height - _snake.Count);
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var point = new GridPoint(x, y);
                if (!_snake.Contains(point))
                {
                    emptyCells.Add(point);
                }
            }
        }

        if (emptyCells.Count == 0)
        {
            HasWon = true;
            IsGameOver = true;
            return;
        }

        Food = emptyCells[_random.Next(emptyCells.Count)];
    }

    private bool IsOutside(GridPoint point)
    {
        return point.X < 0 || point.X >= Width || point.Y < 0 || point.Y >= Height;
    }

    private static bool IsOpposite(Direction current, Direction next)
    {
        return current switch
        {
            Direction.Up => next == Direction.Down,
            Direction.Down => next == Direction.Up,
            Direction.Left => next == Direction.Right,
            Direction.Right => next == Direction.Left,
            _ => false
        };
    }
}
