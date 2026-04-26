namespace TesseraSnake.Game;

internal static class DifficultySettings
{
    public static TimeSpan TickInterval(DifficultyLevel difficulty)
    {
        return difficulty switch
        {
            DifficultyLevel.Easy => TimeSpan.FromMilliseconds(160),
            DifficultyLevel.Hard => TimeSpan.FromMilliseconds(70),
            _ => TimeSpan.FromMilliseconds(110)
        };
    }
}
