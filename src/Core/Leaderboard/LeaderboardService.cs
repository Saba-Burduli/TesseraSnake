using System.Text.Json;

namespace TesseraSnake.Core.Leaderboard;

internal sealed class LeaderboardService
{
    private const int MaxEntries = 10;
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly string _path;
    private readonly List<ScoreEntry> _entries = [];

    public LeaderboardService(string? path = null)
    {
        _path = path ?? Path.Combine(AppContext.BaseDirectory, "leaderboard.json");
        Load();
    }

    public IReadOnlyList<ScoreEntry> Entries => _entries;

    public void AddScore(int score, DateTimeOffset recordedAt)
    {
        _entries.Add(new ScoreEntry(score, recordedAt));
        SortAndTrim();
        Save();
    }

    private void Load()
    {
        if (!File.Exists(_path))
        {
            return;
        }

        try
        {
            var entries = JsonSerializer.Deserialize<List<ScoreEntry>>(File.ReadAllText(_path), JsonOptions);
            if (entries is not null)
            {
                _entries.Clear();
                _entries.AddRange(entries);
                SortAndTrim();
            }
        }
        catch (JsonException)
        {
            _entries.Clear();
        }
        catch (IOException)
        {
            _entries.Clear();
        }
    }

    private void Save()
    {
        var directory = Path.GetDirectoryName(_path);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(_path, JsonSerializer.Serialize(_entries, JsonOptions));
    }

    private void SortAndTrim()
    {
        _entries.Sort(static (left, right) =>
        {
            var scoreCompare = right.Score.CompareTo(left.Score);
            return scoreCompare != 0 ? scoreCompare : right.RecordedAt.CompareTo(left.RecordedAt);
        });

        if (_entries.Count > MaxEntries)
        {
            _entries.RemoveRange(MaxEntries, _entries.Count - MaxEntries);
        }
    }
}
