using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using QRCoder;
using System.Drawing.Printing;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Projekt_lab11;

public partial class MainWindow : Window
{
    private readonly DatabaseService _databaseService;
    private readonly List<Sample> _samples = new();
    private byte[]? _currentQrBytes;
    private string? _selectedSampleId;

    public MainWindow()
    {
        InitializeComponent();
        _databaseService = new DatabaseService(Path.Combine(AppContext.BaseDirectory, "samples.db"));
        InitializeForm();
        LoadSamples();
    }

    private void InitializeForm()
    {
        TypeCombo.ItemsSource = new[] { "DNA", "RNA", "Białko", "Inny" };
        TypeCombo.SelectedIndex = 0;
        DatePicker.SelectedDate = DateTimeOffset.Now;
    }

    private void LoadSamples()
    {
        _samples.Clear();
        _samples.AddRange(_databaseService.LoadSamples());
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        var query = SearchBox.Text?.Trim() ?? string.Empty;

        var filtered = string.IsNullOrEmpty(query)
            ? _samples
            : _samples.Where(sample =>
                sample.Id.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                sample.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                sample.Type.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();

        SamplesList.ItemsSource = filtered;
    }

    private void SearchBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }

    private void SamplesList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (SamplesList.SelectedItem is Sample sample)
        {
            _selectedSampleId = sample.Id;

            IdBox.Text = sample.Id;
            NameBox.Text = sample.Name;
            TypeCombo.SelectedItem = sample.Type;
            DatePicker.SelectedDate = new DateTimeOffset(sample.DateCollected);
            DescriptionBox.Text = sample.Description;

            GeneratePreview(sample);

            SetStatus($"Załadowano próbkę {sample.Name}.");
        }
    }

    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        var sample = ReadSampleFromForm();

        if (sample == null)
            return;

        if (_databaseService.SampleExists(sample.Id))
        {
            SetStatus("Próbka o podanym ID już istnieje.");
            return;
        }

        _databaseService.AddSample(sample);

        LoadSamples();
        ClearForm();

        SetStatus("Próbka została dodana.");
    }

    private void UpdateButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedSampleId == null)
        {
            SetStatus("Wybierz próbkę do edycji.");
            return;
        }

        var sample = ReadSampleFromForm();

        if (sample == null)
            return;

        if (_databaseService.SampleExists(sample.Id, _selectedSampleId))
        {
            SetStatus("Podane ID jest już zajęte przez inną próbkę.");
            return;
        }

        _databaseService.UpdateSample(_selectedSampleId, sample);

        LoadSamples();

        SetStatus("Próbka została zaktualizowana.");
    }

    private void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedSampleId == null)
        {
            SetStatus("Wybierz próbkę do usunięcia.");
            return;
        }

        _databaseService.DeleteSample(_selectedSampleId);

        LoadSamples();
        ClearForm();

        SetStatus("Próbka została usunięta.");
    }

    private void ClearButton_Click(object? sender, RoutedEventArgs e)
    {
        ClearForm();
        SetStatus("Formularz wyczyszczony.");
    }

    private void GenerateButton_Click(object? sender, RoutedEventArgs e)
    {
        var sample = ReadSampleFromForm();

        if (sample == null)
            return;

        GeneratePreview(sample);

        SetStatus("Kod QR został wygenerowany.");
    }

    private async void ExportButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_currentQrBytes == null)
        {
            SetStatus("Najpierw wygeneruj kod QR.");
            return;
        }

        var path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "qr_code.png");

        await File.WriteAllBytesAsync(path, _currentQrBytes);

        SetStatus($"Kod QR zapisano na pulpicie: {path}");
    }

    private void PrintButton_Click(object? sender, RoutedEventArgs e)
    {
        SetStatus("Drukowanie zostało tymczasowo wyłączone.");
    }

    private Sample? ReadSampleFromForm()
    {
        var id = IdBox.Text?.Trim() ?? string.Empty;
        var name = NameBox.Text?.Trim() ?? string.Empty;
        var type = TypeCombo.SelectedItem?.ToString() ?? string.Empty;

        var date = (DatePicker.SelectedDate ?? DateTimeOffset.Now).DateTime;

        var description = DescriptionBox.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(id) ||
            string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(type))
        {
            SetStatus("Wypełnij pola ID, nazwa i typ próbki.");
            return null;
        }

        return new Sample
        {
            Id = id,
            Name = name,
            Type = type,
            DateCollected = date,
            Description = description
        };
    }

    private void GeneratePreview(Sample sample)
    {
        _currentQrBytes = CreateQrCodeBytes(sample);

        using var stream = new MemoryStream(_currentQrBytes);

        QrPreview.Source = new Avalonia.Media.Imaging.Bitmap(stream);
    }

    private static byte[] CreateQrCodeBytes(Sample sample)
    {
        var text =
            $"ID:{sample.Id}\n" +
            $"Nazwa:{sample.Name}\n" +
            $"Typ:{sample.Type}\n" +
            $"Data:{sample.DateCollected:yyyy-MM-dd}\n" +
            $"Opis:{sample.Description}";

        using var generator = new QRCodeGenerator();

        using var qrCodeData =
            generator.CreateQrCode(
                text,
                QRCodeGenerator.ECCLevel.Q);

        var qrCode = new PngByteQRCode(qrCodeData);

        return qrCode.GetGraphic(20);
    }

    private void ClearForm()
    {
        IdBox.Text = string.Empty;
        NameBox.Text = string.Empty;

        TypeCombo.SelectedIndex = 0;

        DatePicker.SelectedDate = DateTimeOffset.Now;

        DescriptionBox.Text = string.Empty;

        SamplesList.SelectedIndex = -1;

        _selectedSampleId = null;
        _currentQrBytes = null;

        QrPreview.Source = null;
    }

    private void SetStatus(string message)
    {
        StatusText.Text = message;
    }
}