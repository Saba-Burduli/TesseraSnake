using Tessera;
using Tessera.Controls;
using Tessera.Layout;
using TesseraSnake.Core.Entities;
using TesseraSnake.Core.Leaderboard;
using TesseraSnake.Game;

namespace TesseraSnake.UI.Menu;

internal sealed class WidgetShowcasePage
{
    private readonly Button _startButton = new()
    {
        Text = "Start",
        Description = "action widget",
        SurfaceStyle = SnakeTheme.FieldOdd,
        LabelStyle = SnakeTheme.Title
    };

    private readonly Button _pauseButton = new()
    {
        Text = "Pause",
        Description = "command widget",
        SurfaceStyle = SnakeTheme.FieldEven,
        LabelStyle = SnakeTheme.StatusPaused
    };

    private readonly Badge _difficultyBadge = new()
    {
        Text = "Medium",
        Tone = BadgeTone.Warning,
        SuccessTextStyle = SnakeTheme.Title,
        WarningTextStyle = SnakeTheme.StatusPaused,
        ErrorTextStyle = SnakeTheme.StatusGameOver
    };

    private readonly StatsCard _stats = new()
    {
        Title = "StatsCard",
        Border = BorderStyle.Ascii,
        Padding = Thickness.Symmetric(1),
        BorderStyleText = SnakeTheme.Border,
        KeyStyle = SnakeTheme.MenuHint,
        ValueStyle = SnakeTheme.Title
    };

    private readonly ProgressBar _progress = new()
    {
        Title = "ProgressBar",
        Border = BorderStyle.Ascii,
        Padding = Thickness.Symmetric(1),
        BorderStyleText = SnakeTheme.Border,
        FillStyle = SnakeTheme.Body,
        TrackStyle = SnakeTheme.FieldOdd,
        LabelStyle = SnakeTheme.PanelText
    };

    private readonly Gauge _gauge = new()
    {
        Title = "Gauge",
        MinValue = 0,
        MaxValue = 100,
        ValueLabelStyle = SnakeTheme.Title
    };

    private readonly Sparkline _sparkline = new(capacity: 32)
    {
        Title = "Sparkline",
        Border = BorderStyle.Ascii,
        Padding = Thickness.Symmetric(1),
        BorderStyleText = SnakeTheme.Border,
        DataStyle = SnakeTheme.Title,
        MetaStyle = SnakeTheme.PanelText,
        Options = new SparklineOptions(ShowStats: true, Legend: "pace")
    };

    private readonly KeyValueList _inspector = new()
    {
        Title = "KeyValueList",
        Border = BorderStyle.Ascii,
        Padding = Thickness.Symmetric(1),
        PreferredKeyColumnWidth = 12,
        BorderStyleText = SnakeTheme.Border,
        KeyStyle = SnakeTheme.MenuHint,
        ValueStyle = SnakeTheme.PanelText,
        SeparatorStyle = SnakeTheme.Border,
        SelectedRowStyle = SnakeTheme.MenuSelected
    };

    private readonly ListView<string> _families = new()
    {
        Title = "ListView",
        Border = BorderStyle.Ascii,
        Padding = Thickness.Symmetric(1),
        BorderStyleText = SnakeTheme.Border,
        DefaultRowStyle = SnakeTheme.PanelText,
        SelectedRowStyle = SnakeTheme.MenuSelected
    };

    private readonly Tabs _tabs = new("HUD", "Menus", "Data", "Charts")
    {
        Title = "Tabs",
        TitleStyle = SnakeTheme.Title
    };

    private readonly Label _notes = new()
    {
        Border = BorderStyle.Ascii,
        Title = " Label ",
        Padding = Thickness.Symmetric(1),
        TextStyle = SnakeTheme.PanelText,
        BorderStyleText = SnakeTheme.Border
    };

    private readonly StatusBar _footer = new()
    {
        Fill = ' ',
        LeftTextStyle = SnakeTheme.StatusLeft,
        RightTextStyle = SnakeTheme.StatusRight,
        FillStyle = SnakeTheme.StatusFill
    };

    public WidgetShowcasePage()
    {
        _families.SetItems([
            "Action: Button",
            "Status: Badge / StatusBar",
            "Navigation: Tabs / ListView",
            "Inspection: KeyValueList",
            "Dashboard: StatsCard / ProgressBar / Gauge",
            "Chart: Sparkline"
        ]);
        _families.SetSelectedIndex(0);
    }

    public Screen Build(
        SnakeGameState state,
        DifficultyLevel difficulty,
        IReadOnlyList<ScoreEntry> leaderboard)
    {
        Refresh(state, difficulty, leaderboard);

        return Screen.Build(window =>
        {
            window.Padding(1);
            window.Header(1, _tabs);
            window.Body(body => body.Row(row =>
            {
                row.Gap(1);
                row.Weighted(1, left => left.Column(column =>
                {
                    column.Gap(1);
                    column.Fixed(5, actions => actions.Row(actionRow =>
                    {
                        actionRow.Gap(1);
                        actionRow.Weighted(1, _startButton);
                        actionRow.Weighted(1, _pauseButton);
                    }));
                    column.Fixed(2, _difficultyBadge);
                    column.Fixed(5, _stats);
                    column.Fixed(5, _progress);
                    column.Fixed(4, _gauge);
                }));
                row.Weighted(1, right => right.Column(column =>
                {
                    column.Gap(1);
                    column.Fixed(5, _sparkline);
                    column.Fixed(8, _inspector);
                    column.Fixed(8, _families);
                    column.Fixed(4, _notes);
                }));
            }));
            window.Footer(1, _footer);
        });
    }

    private void Refresh(
        SnakeGameState state,
        DifficultyLevel difficulty,
        IReadOnlyList<ScoreEntry> leaderboard)
    {
        var capacity = state.Width * state.Height;
        var fillRatio = Math.Clamp((double)state.Snake.Count / capacity, 0, 1);
        var bestScore = leaderboard.Count == 0 ? 0 : leaderboard[0].Score;

        _difficultyBadge.Text = difficulty.ToString();
        _difficultyBadge.Tone = difficulty switch
        {
            DifficultyLevel.Easy => BadgeTone.Success,
            DifficultyLevel.Hard => BadgeTone.Error,
            _ => BadgeTone.Warning
        };

        _stats.SetItems([
            new StatItem("Score", state.Score.ToString("D3")),
            new StatItem("Length", state.Snake.Count.ToString("D3")),
            new StatItem("Best", bestScore.ToString("D3"))
        ]);

        _progress.SetValue(fillRatio);
        _gauge.Value = Math.Round(fillRatio * 100);
        _gauge.Label = $"{_gauge.Value:0}%";
        _sparkline.SetSamples(BuildSamples(state.Score, state.Snake.Count, difficulty));
        _inspector.SetEntries([
            new KeyValueListEntry("Difficulty", difficulty.ToString()),
            new KeyValueListEntry("Board", $"{state.Width} x {state.Height}"),
            new KeyValueListEntry("Head", $"{state.Head.X}, {state.Head.Y}"),
            new KeyValueListEntry("Food", $"{state.Food.X}, {state.Food.Y}"),
            new KeyValueListEntry("Top scores", $"{leaderboard.Count}/10")
        ]);
        _notes.Text = "Built-in Tessera widgets embedded in the game shell.\nPress Escape or B to return.";
        _footer.LeftText = $" {difficulty} widget lab ";
        _footer.RightText = "Escape/B returns";
    }

    private static double[] BuildSamples(int score, int length, DifficultyLevel difficulty)
    {
        var speed = difficulty switch
        {
            DifficultyLevel.Easy => 2,
            DifficultyLevel.Hard => 8,
            _ => 5
        };

        var samples = new double[24];
        for (var index = 0; index < samples.Length; index++)
        {
            samples[index] = (index * speed + score * 3 + length) % 32;
        }

        return samples;
    }
}
