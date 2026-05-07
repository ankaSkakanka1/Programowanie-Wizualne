using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace WniosekApp;

public partial class MainWindow : Window
{
    private Database db = new Database();
    private readonly ObservableCollection<Wniosek> requests = new();
    private int? currentEditingId = null;

    public MainWindow()
    {
        InitializeComponent();
        db.Init();
        RequestsList.ItemsSource = requests;
        LoadRequests();
    }

    private void OnSave(object sender, RoutedEventArgs e)
    {
        try
        {
            if (currentEditingId.HasValue)
            {
                db.UpdateWniosek(
                    currentEditingId.Value,
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
                currentEditingId = null;
            }
            else
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
            }

            LoadRequests();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas zapisywania: {ex.Message}");
        }
    }

    private void OnRead(object sender, RoutedEventArgs e)
    {
        LoadRequests();
    }

    private void OnEdit(object sender, RoutedEventArgs e)
    {
        if (RequestsList.SelectedItem is Wniosek selected)
        {
            ImieNazwisko.Text = selected.ImieNazwisko;
            NumerAlbumu.Text = selected.NumerAlbumu;
            Kierunek.Text = selected.Kierunek;
            Specjalnosc.Text = selected.Specjalnosc;
            RokStudiow.Text = selected.RokStudiow;
            FormaStudiow.Text = selected.FormaStudiow;
            Przedmiot.Text = selected.Przedmiot;
            Prowadzacy.Text = selected.Prowadzacy;
            DataEgzaminu.Text = selected.DataEgzaminu;
            Ocena.Text = selected.Ocena;
            Uzasadnienie.Text = selected.Uzasadnienie;
            DataWniosku.Text = selected.DataWniosku;
            currentEditingId = selected.Id;
        }
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
            Console.WriteLine($"Błąd podczas ładowania danych: {ex.Message}");
        }
    }
}
