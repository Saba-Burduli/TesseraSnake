using Tessera;
using Tessera.Controls;
using Tessera.Layout;
using TesseraSnake.Core.Leaderboard;

namespace TesseraSnake.UI.Menu;

internal sealed class LeaderboardPage
{
    private readonly Label _content = new()
    {
        Border = BorderStyle.Rounded,
        Title = " Leaderboard ",
        Padding = Thickness.Symmetric(2),
        TextStyle = SnakeTheme.PanelText,
        BorderStyleText = SnakeTheme.Border
    };

    public Screen Build(IReadOnlyList<ScoreEntry> entries)
    {
        _content.Text = RenderEntries(entries);

        return Screen.Build(window =>
        {
            window.Padding(1);
            window.Body(body => body.Center(_content, 72, 18));
        });
    }

    private static string RenderEntries(IReadOnlyList<ScoreEntry> entries)
    {
        if (entries.Count == 0)
        {
            return "No completed runs yet.\n\nPress Escape or B to return.";
        }

        var rows = new List<string> { "Rank  Score  Date/time" };
        for (var index = 0; index < entries.Count; index++)
        {
            var entry = entries[index];
            rows.Add($"{index + 1,2}.   {entry.Score,4}   {entry.RecordedAt.LocalDateTime:yyyy-MM-dd HH:mm}");
        }

        rows.Add(string.Empty);
        rows.Add("Press Escape or B to return.");
        return string.Join(Environment.NewLine, rows);
    }
}
