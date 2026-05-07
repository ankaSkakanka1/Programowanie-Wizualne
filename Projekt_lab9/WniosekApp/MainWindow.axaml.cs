using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace WniosekApp;

public partial class MainWindow : Window
{
    private Database db = new Database();
    private readonly ObservableCollection<string> requests = new();

    public MainWindow()
    {
        InitializeComponent();
        db.Init();
        RequestsList.ItemsSource = requests;
    }

    private void OnSave(object sender, RoutedEventArgs e)
    {
        try
        {
            db.AddWniosek(
                ImieNazwisko.Text ?? string.Empty,
                NumerAlbumu.Text ?? string.Empty,
                Kierunek.Text ?? string.Empty,
                Specjalnosc.Text ?? string.Empty,
                RokStudiow.Text ?? string.Empty,
                FormaStudiow.Text ?? string.Empty,
                Przedmiot.Text ?? string.Empty,
                Prowadzacy.Text ?? string.Empty,
                DataEgzaminu.Text ?? string.Empty,
                Ocena.Text ?? string.Empty,
                Uzasadnienie.Text ?? string.Empty,
                DataWniosku.Text ?? string.Empty
            );

            LoadRequests();
        }
        catch (Exception ex)
        {
            // Pokaż błąd użytkownikowi
            var dialog = new Window
            {
                Title = "Błąd",
                Content = $"Wystąpił błąd podczas zapisywania: {ex.Message}",
                Width = 400,
                Height = 200
            };
            dialog.ShowDialog(this);
        }
    }

    private void OnRead(object sender, RoutedEventArgs e)
    {
        LoadRequests();
    }

    private void OnClear(object sender, RoutedEventArgs e)
    {
        ImieNazwisko.Text = string.Empty;
        NumerAlbumu.Text = string.Empty;
        Kierunek.Text = string.Empty;
        Specjalnosc.Text = string.Empty;
        RokStudiow.Text = string.Empty;
        FormaStudiow.Text = string.Empty;
        Przedmiot.Text = string.Empty;
        Prowadzacy.Text = string.Empty;
        DataEgzaminu.Text = string.Empty;
        Ocena.Text = string.Empty;
        Uzasadnienie.Text = string.Empty;
        DataWniosku.Text = string.Empty;
    }

    private void LoadRequests()
    {
        try
        {
            requests.Clear();
            foreach (var item in db.ReadAll())
            {
                requests.Add(item);
            }
        }
        catch (Exception ex)
        {
            // Pokaż błąd użytkownikowi
            var dialog = new Window
            {
                Title = "Błąd",
                Content = $"Wystąpił błąd podczas ładowania danych: {ex.Message}",
                Width = 400,
                Height = 200
            };
            dialog.ShowDialog(this);
        }
    }
}
