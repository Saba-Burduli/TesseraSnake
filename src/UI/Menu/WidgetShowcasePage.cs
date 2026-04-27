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
        LabelStyle = SnakeTheme.Title,
        FocusedSurfaceStyle = SnakeTheme.MenuSelected,
        FocusedLabelStyle = SnakeTheme.MenuSelected
    };

    private readonly Button _pauseButton = new()
    {
        Text = "Pause",
        Description = "command widget",
        SurfaceStyle = SnakeTheme.FieldEven,
        LabelStyle = SnakeTheme.StatusPaused,
        FocusedSurfaceStyle = SnakeTheme.MenuSelected,
        FocusedLabelStyle = SnakeTheme.MenuSelected
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
        FocusedBorderStyleText = SnakeTheme.Title,
        TitleStyle = SnakeTheme.Title,
        FocusedTitleStyle = SnakeTheme.MenuSelected,
        KeyStyle = SnakeTheme.MenuHint,
        ValueStyle = SnakeTheme.Title
    };

    private readonly ProgressBar _progress = new()
    {
        Title = "ProgressBar",
        Border = BorderStyle.Ascii,
        Padding = Thickness.Symmetric(1),
        BorderStyleText = SnakeTheme.Border,
        FocusedBorderStyleText = SnakeTheme.Title,
        TitleStyle = SnakeTheme.Title,
        FocusedTitleStyle = SnakeTheme.MenuSelected,
        FillStyle = SnakeTheme.Body,
        TrackStyle = SnakeTheme.FieldOdd,
        LabelStyle = SnakeTheme.PanelText
    };

    private readonly Gauge _gauge = new()
    {
        Title = "Gauge",
        MinValue = 0,
        MaxValue = 100,
        TitleStyle = SnakeTheme.Title,
        FocusedTitleStyle = SnakeTheme.MenuSelected,
        ValueLabelStyle = SnakeTheme.Title
    };

    private readonly Sparkline _sparkline = new(capacity: 32)
    {
        Title = "Sparkline",
        Border = BorderStyle.Ascii,
        Padding = Thickness.Symmetric(1),
        BorderStyleText = SnakeTheme.Border,
        FocusedBorderStyleText = SnakeTheme.Title,
        TitleStyle = SnakeTheme.Title,
        FocusedTitleStyle = SnakeTheme.MenuSelected,
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
        FocusedBorderStyleText = SnakeTheme.Title,
        TitleStyle = SnakeTheme.Title,
        FocusedTitleStyle = SnakeTheme.MenuSelected,
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
        FocusedBorderStyleText = SnakeTheme.Title,
        TitleStyle = SnakeTheme.Title,
        FocusedTitleStyle = SnakeTheme.MenuSelected,
        DefaultRowStyle = SnakeTheme.PanelText,
        SelectedRowStyle = SnakeTheme.MenuSelected,
        PageSize = 6
    };

    private readonly Tabs _tabs = new("HUD", "Menus", "Data", "Charts")
    {
        Title = "Tabs",
        TitleStyle = SnakeTheme.Title,
        FocusedTitleStyle = SnakeTheme.MenuSelected
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

    private readonly Control[] _focusOrder;
    private string _actionMessage = "Tab changes focus. Arrows change focused lists/tabs. Enter activates buttons.";
    private int _focusIndex;
    private int _familiesIndex;
    private int _inspectorIndex;

    public WidgetShowcasePage()
    {
        _focusOrder =
        [
            _tabs,
            _startButton,
            _pauseButton,
            _families,
            _inspector,
            _progress,
            _gauge,
            _sparkline,
            _stats
        ];

        _families.SetItems([
            "Action: Button",
            "Status: Badge / StatusBar",
            "Navigation: Tabs / ListView",
            "Inspection: KeyValueList",
            "Dashboard: StatsCard / ProgressBar / Gauge",
            "Chart: Sparkline"
        ]);
        _families.SetSelectedIndex(0);
        FocusFirst();
    }

    public Screen Build(
        ScreenContext context,
        SnakeGameState state,
        DifficultyLevel difficulty,
        IReadOnlyList<ScoreEntry> leaderboard)
    {
        Refresh(state, difficulty, leaderboard);
        var shellWidth = Math.Min(122, Math.Max(48, context.Width - 2));
        var shellHeight = Math.Min(34, Math.Max(22, context.Height - 2));
        var wideLayout = shellWidth >= 96 && shellHeight >= 28;

        return Screen.Build(window =>
        {
            window.Body(body => body.Center(center => center.Column(column =>
            {
                column.Fixed(1, _tabs);
                column.Gap(1);
                column.Fill(widgets =>
                {
                    if (wideLayout)
                    {
                        widgets.Row(row =>
                        {
                            row.Gap(1);
                            row.Weighted(1, left => left.Column(leftColumn =>
                            {
                                leftColumn.Gap(1);
                                leftColumn.Fixed(3, actions => actions.Row(actionRow =>
                                {
                                    actionRow.Gap(1);
                                    actionRow.Weighted(1, _startButton);
                                    actionRow.Weighted(1, _pauseButton);
                                }));
                                leftColumn.Fixed(1, _difficultyBadge);
                                leftColumn.Fixed(5, _stats);
                                leftColumn.Fixed(5, _progress);
                                leftColumn.Fixed(4, _gauge);
                            }));
                            row.Weighted(1, right => right.Column(rightColumn =>
                            {
                                rightColumn.Gap(1);
                                rightColumn.Fixed(5, _sparkline);
                                rightColumn.Fixed(8, _inspector);
                                rightColumn.Fixed(8, _families);
                                rightColumn.Fixed(4, _notes);
                            }));
                        });
                    }
                    else
                    {
                        widgets.Column(compact =>
                        {
                            compact.Gap(1);
                            compact.Fixed(3, top => top.Row(row =>
                            {
                                row.Gap(1);
                                row.Weighted(1, _startButton);
                                row.Weighted(1, _pauseButton);
                                row.Fixed(16, _difficultyBadge);
                            }));
                            compact.Fixed(6, middle => middle.Row(row =>
                            {
                                row.Gap(1);
                                row.Weighted(1, _stats);
                                row.Weighted(1, _progress);
                                row.Weighted(1, _sparkline);
                            }));
                            compact.Fill(bottom => bottom.Row(row =>
                            {
                                row.Gap(1);
                                row.Weighted(1, _families);
                                row.Weighted(1, _inspector);
                                row.Weighted(1, details => details.Column(stack =>
                                {
                                    stack.Gap(1);
                                    stack.Fixed(4, _gauge);
                                    stack.Fill(_notes);
                                }));
                            }));
                        });
                    }
                });
                column.Fixed(1, _footer);
            }), shellWidth, shellHeight));
        });
    }

    public void FocusFirst()
    {
        _focusIndex = 0;
        _focusOrder[_focusIndex].RequestFocus();
    }

    public WidgetPageAction HandleKey(KeyPressed key)
    {
        if (key.Is(Key.Escape) || key.IsCharacter('b') || key.IsCharacter('q', ModifierKeys.Ctrl))
        {
            return WidgetPageAction.Back;
        }

        if (key.Is(Key.Tab))
        {
            FocusNext();
            return WidgetPageAction.None;
        }

        if (key.IsCharacter('['))
        {
            FocusPrevious();
            return WidgetPageAction.None;
        }

        if (FocusedControl == _tabs && (key.Is(Key.Left) || key.IsCharacter('a')))
        {
            SelectTab(-1);
            return WidgetPageAction.None;
        }

        if (FocusedControl == _tabs && (key.Is(Key.Right) || key.IsCharacter('d')))
        {
            SelectTab(1);
            return WidgetPageAction.None;
        }

        if (FocusedControl == _families && (key.Is(Key.Up) || key.IsCharacter('w')))
        {
            MoveListSelection(-1);
            return WidgetPageAction.None;
        }

        if (FocusedControl == _families && (key.Is(Key.Down) || key.IsCharacter('s')))
        {
            MoveListSelection(1);
            return WidgetPageAction.None;
        }

        if (FocusedControl == _inspector && (key.Is(Key.Up) || key.IsCharacter('w')))
        {
            MoveInspectorSelection(-1);
            return WidgetPageAction.None;
        }

        if (FocusedControl == _inspector && (key.Is(Key.Down) || key.IsCharacter('s')))
        {
            MoveInspectorSelection(1);
            return WidgetPageAction.None;
        }

        if (IsActivation(key) && FocusedControl == _startButton)
        {
            _actionMessage = "Start Game activated from a Tessera Button.";
            return WidgetPageAction.StartGame;
        }

        if (IsActivation(key) && FocusedControl == _pauseButton)
        {
            _actionMessage = "Pause toggled from a Tessera Button.";
            return WidgetPageAction.TogglePause;
        }

        return WidgetPageAction.None;
    }

    private void FocusNext()
    {
        _focusIndex = (_focusIndex + 1) % _focusOrder.Length;
        _focusOrder[_focusIndex].RequestFocus();
    }

    private void FocusPrevious()
    {
        _focusIndex = (_focusIndex - 1 + _focusOrder.Length) % _focusOrder.Length;
        _focusOrder[_focusIndex].RequestFocus();
    }

    private Control FocusedControl => _focusOrder[_focusIndex];

    private void SelectTab(int delta)
    {
        const int tabCount = 4;
        var next = (_tabs.SelectedIndex + delta + tabCount) % tabCount;
        _tabs.SetSelectedIndex(next);
        _actionMessage = $"Tabs selected: {_tabs.SelectedIndex + 1}/{tabCount}.";
    }

    private void MoveListSelection(int delta)
    {
        const int itemCount = 6;
        _familiesIndex = (_familiesIndex + delta + itemCount) % itemCount;
        _families.SetSelectedIndex(_familiesIndex);
        _actionMessage = $"ListView selected: {_families.SelectedIndex + 1}/{itemCount}.";
    }

    private void MoveInspectorSelection(int delta)
    {
        const int itemCount = 5;
        _inspectorIndex = (_inspectorIndex + delta + itemCount) % itemCount;
        _inspector.SetSelectedIndex(_inspectorIndex);
        _actionMessage = $"KeyValueList selected: {_inspector.SelectedIndex + 1}/{itemCount}.";
    }

    private static bool IsActivation(KeyPressed key)
    {
        return key.Is(Key.Enter) || key.IsCharacter(' ');
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
        _families.SetSelectedIndex(_familiesIndex);
        _inspector.SetSelectedIndex(_inspectorIndex);

        _notes.Text = $"{_actionMessage}\nEscape/B returns. [ moves focus backward.";
        _footer.LeftText = $" {difficulty} widget lab ";
        _footer.RightText = "Tab focus | Enter activate | arrows move";
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

internal enum WidgetPageAction
{
    None,
    Back,
    StartGame,
    TogglePause
}
