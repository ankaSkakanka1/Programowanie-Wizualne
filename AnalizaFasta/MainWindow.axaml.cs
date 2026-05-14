using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnalizaFasta.Models;
using AnalizaFasta.Services;
using Rectangle = Avalonia.Controls.Shapes.Rectangle;
using Line = Avalonia.Controls.Shapes.Line;

namespace AnalizaFasta;

public partial class MainWindow : Window
{
    private List<FastaSequence> _sequences = new();
    private FastaSequence? _selectedSequence;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void LoadButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Wybierz plik FASTA",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType("FASTA files") { Patterns = new[] { "*.fasta", "*.fa", "*.faa", "*.fna", "*.txt" } } }
        });

        if (files.Count == 0) return;

        var file = files[0];
        var path = file.Path.LocalPath;

        var (sequences, isValid, message) = await FastaParser.ParseAsync(path);

        _sequences = sequences;
        UpdateSequencesList();

        StatusText.Text = isValid ? $"✓ {message}" : $"✗ {message}";
        StatusText.Foreground = isValid ?
            new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Green) :
            new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red);

        if (_sequences.Count > 0)
        {
            SequencesList.SelectedIndex = 0;
        }
    }

    private void UpdateSequencesList()
    {
        SequencesList.Items.Clear();
        foreach (var seq in _sequences)
        {
            SequencesList.Items.Add($"{seq.Name} ({seq.Length} bp)");
        }
    }

    private void SequencesList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (SequencesList.SelectedIndex < 0 || SequencesList.SelectedIndex >= _sequences.Count)
            return;

        _selectedSequence = _sequences[SequencesList.SelectedIndex];
        UpdateDetails();
    }

    private void UpdateDetails()
    {
        if (_selectedSequence == null) return;

        _selectedSequence.Analyze(out int a, out int t, out int g, out int c, out double gc, out int codons, out bool valid);

        DetailName.Text = _selectedSequence.Name;
        DetailHeader.Text = _selectedSequence.Header;
        DetailLength.Text = $"{_selectedSequence.Length} bp";
        DetailGC.Text = $"{gc:F2}%";
        DetailCodons.Text = codons.ToString();
        DetailValid.Text = valid ? "Prawidłowy" : "Zawiera znaki spoza ATGC";

        BasesA.Text = a.ToString();
        BasesT.Text = t.ToString();
        BasesG.Text = g.ToString();
        BasesC.Text = c.ToString();

        SequenceTextBox.Text = _selectedSequence.Sequence;

        UpdateChart();
    }

    private void UpdateChart()
    {
        if (_selectedSequence == null || !(SequencesChart is Canvas canvas)) return;

        _selectedSequence.Analyze(out int a, out int t, out int g, out int c, out _, out _, out _);

        canvas.Children.Clear();

        var values = new[] { a, c, g, t };
        var labels = new[] { "A", "C", "G", "T" };
        var colors = new[] {
            new SolidColorBrush(Color.FromRgb(220, 20, 60)),   // Crimson - A
            new SolidColorBrush(Color.FromRgb(65, 105, 225)),  // RoyalBlue - C
            new SolidColorBrush(Color.FromRgb(34, 139, 34)),   // ForestGreen - G
            new SolidColorBrush(Color.FromRgb(255, 165, 0))    // Orange - T
        };

        var maxValue = values.Max() > 0 ? values.Max() : 1;
        const double barWidth = 50;
        const double spacing = 30;
        const double chartHeight = 200;
        const double padding = 20;
        double startX = padding;

        for (int i = 0; i < values.Length; i++)
        {
            double barHeight = (values[i] / (double)maxValue) * chartHeight;
            double x = startX + i * (barWidth + spacing);
            double y = chartHeight + padding - barHeight;

            // Draw bar rectangle
            var rect = new Rectangle
            {
                Width = barWidth,
                Height = barHeight,
                Fill = colors[i],
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 1
            };
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            canvas.Children.Add(rect);

            // Draw value on top of bar
            var valueText = new TextBlock
            {
                Text = values[i].ToString(),
                FontSize = 12,
                FontWeight = FontWeight.Bold,
                TextAlignment = TextAlignment.Center,
                Foreground = new SolidColorBrush(Colors.Black)
            };
            Canvas.SetLeft(valueText, x);
            Canvas.SetTop(valueText, y - 20);
            canvas.Children.Add(valueText);

            // Draw label below bar
            var label = new TextBlock
            {
                Text = labels[i],
                FontSize = 14,
                FontWeight = FontWeight.Bold,
                TextAlignment = TextAlignment.Center,
                Foreground = new SolidColorBrush(Colors.Black),
                Width = barWidth
            };
            Canvas.SetLeft(label, x);
            Canvas.SetTop(label, chartHeight + padding + 10);
            canvas.Children.Add(label);
        }

        // Draw axes
        var axisLine = new Line
        {
            StartPoint = new Avalonia.Point(padding - 5, chartHeight + padding),
            EndPoint = new Avalonia.Point(startX + 4 * (barWidth + spacing), chartHeight + padding),
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 2
        };
        canvas.Children.Add(axisLine);

        var yAxisLine = new Line
        {
            StartPoint = new Avalonia.Point(padding - 5, chartHeight + padding),
            EndPoint = new Avalonia.Point(padding - 5, padding),
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 2
        };
        canvas.Children.Add(yAxisLine);
    }

    private async void ExportCsvButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_sequences.Count == 0)
        {
            StatusText.Text = "✗ Brak sekwencji do eksportu";
            StatusText.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red);
            return;
        }

        try
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"analiza_fasta_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
            var filePath = Path.Combine(desktopPath, fileName);

            var success = await ExportService.ExportToCsvAsync(_sequences, filePath);
            StatusText.Text = success ? $"✓ Eksportowano do: {fileName}" : "✗ Błąd eksportu CSV";
            StatusText.Foreground = success ?
                new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Green) :
                new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red);
        }
        catch (Exception ex)
        {
            StatusText.Text = $"✗ Błąd: {ex.Message}";
            StatusText.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red);
        }
    }

    private async void ExportTxtButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_sequences.Count == 0)
        {
            StatusText.Text = "✗ Brak sekwencji do eksportu";
            StatusText.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red);
            return;
        }

        try
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"analiza_fasta_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            var filePath = Path.Combine(desktopPath, fileName);

            var success = await ExportService.ExportToTxtAsync(_sequences, filePath);
            StatusText.Text = success ? $"✓ Eksportowano do: {fileName}" : "✗ Błąd eksportu TXT";
            StatusText.Foreground = success ?
                new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Green) :
                new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red);
        }
        catch (Exception ex)
        {
            StatusText.Text = $"✗ Błąd: {ex.Message}";
            StatusText.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red);
        }
    }
}