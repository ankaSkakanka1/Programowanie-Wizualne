using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Projekt_lab12.Models;
using Projekt_lab12.Services;

namespace Projekt_lab12;

public partial class MainWindow : Window
{
    private readonly NotebookDataService _dataService;
    private readonly SessionReportService _reportService;
    private readonly List<Session> _sessions = new();
    private Session? _selectedSession;
    private Entry? _selectedEntry;

    public MainWindow()
    {
        InitializeComponent();

        _dataService = new NotebookDataService(
            Path.Combine(AppContext.BaseDirectory, "notebook_data"));

        _reportService = new SessionReportService();

        LoadSessions();
    }

    private void LoadSessions()
    {
        _sessions.Clear();
        _sessions.AddRange(_dataService.LoadSessions());

        SessionsList.ItemsSource = _sessions;
        ClearSessionSelection();
    }

    private void RefreshSessionList()
    {
        SessionsList.ItemsSource = _sessions;
    }

    private void RefreshEntryList()
    {
        if (_selectedSession is null)
        {
            EntriesList.ItemsSource = new ObservableCollection<Entry>();
        }
        else
        {
            EntriesList.ItemsSource = _selectedSession.Entries;
        }
    }

    private void RefreshAttachmentsList()
    {
        if (_selectedEntry is null)
        {
            AttachmentsList.ItemsSource = new ObservableCollection<Attachment>();
        }
        else
        {
            AttachmentsList.ItemsSource = _selectedEntry.Attachments;
        }
    }

    private void SetSessionDetails()
    {
        if (_selectedSession is null)
        {
            SelectedSessionTitle.Text = "Brak wybranej sesji";
            SelectedSessionDate.Text = string.Empty;
            return;
        }

        SelectedSessionTitle.Text = _selectedSession.Title;
        SelectedSessionDate.Text = $"Data utworzenia: {_selectedSession.CreatedAt:yyyy-MM-dd HH:mm}";
    }

    private void ClearSessionSelection()
    {
        _selectedSession = null;
        _selectedEntry = null;
        RefreshEntryList();
        RefreshAttachmentsList();
        SetSessionDetails();
        ClearEntryForm();
    }

    private void ClearEntryForm()
    {
        EntryTitleBox.Text = string.Empty;
        EntryNotesBox.Text = string.Empty;
        AttachmentsList.SelectedIndex = -1;
        _selectedEntry = null;
    }

    private void AddSessionButton_Click(object? sender, RoutedEventArgs e)
    {
        var title = SessionTitleBox.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(title))
        {
            SetStatus("Wprowadź tytuł nowej sesji.");
            return;
        }

        var session = new Session
        {
            Title = title,
            CreatedAt = DateTime.Now
        };

        _sessions.Add(session);
        _dataService.SaveSessions(_sessions);

        RefreshSessionList();
        SessionsList.SelectedItem = session;
        SetStatus("Nowa sesja została utworzona.");
        SessionTitleBox.Text = string.Empty;
    }

    private void DeleteSessionButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedSession is null)
        {
            SetStatus("Wybierz sesję do usunięcia.");
            return;
        }

        _dataService.DeleteSessionAttachments(_selectedSession.Id);
        _sessions.Remove(_selectedSession);
        _dataService.SaveSessions(_sessions);

        RefreshSessionList();
        ClearSessionSelection();
        SetStatus("Sesja została usunięta.");
    }

    private void SessionsList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (SessionsList.SelectedItem is Session session)
        {
            _selectedSession = session;
            _selectedEntry = null;
            SetSessionDetails();
            RefreshEntryList();
            RefreshAttachmentsList();
            ClearEntryForm();
        }
    }

    private void AddEntryButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedSession is null)
        {
            SetStatus("Wybierz sesję przed dodaniem wpisu.");
            return;
        }

        var title = EntryTitleBox.Text?.Trim() ?? string.Empty;
        var notes = EntryNotesBox.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(notes))
        {
            SetStatus("Wprowadź tytuł i opis wpisu.");
            return;
        }

        var entry = new Entry
        {
            Title = title,
            Notes = notes,
            CreatedAt = DateTime.Now
        };

        _selectedSession.Entries.Add(entry);
        _dataService.SaveSessions(_sessions);

        RefreshEntryList();
        EntriesList.SelectedItem = entry;
        SetStatus("Wpis został dodany do sesji.");
        ClearEntryForm();
    }

    private void DeleteEntryButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedSession is null || _selectedEntry is null)
        {
            SetStatus("Wybierz wpis do usunięcia.");
            return;
        }

        _dataService.DeleteEntryAttachments(_selectedSession.Id, _selectedEntry.Id);
        _selectedSession.Entries.Remove(_selectedEntry);
        _dataService.SaveSessions(_sessions);

        RefreshEntryList();
        RefreshAttachmentsList();
        _selectedEntry = null;
        SetStatus("Wpis został usunięty.");
        ClearEntryForm();
    }

    private void ClearEntryButton_Click(object? sender, RoutedEventArgs e)
    {
        ClearEntryForm();
        SetStatus("Formularz wpisu został wyczyszczony.");
    }

    private void EntriesList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (EntriesList.SelectedItem is Entry entry)
        {
            _selectedEntry = entry;
            EntryTitleBox.Text = entry.Title;
            EntryNotesBox.Text = entry.Notes;
            RefreshAttachmentsList();
            SetStatus($"Wybrano wpis: {entry.Title}.");
        }
    }

    private async void AddAttachmentButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedEntry is null || _selectedSession is null)
        {
            SetStatus("Wybierz wpis, do którego chcesz dodać załącznik.");
            return;
        }

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null)
        {
            SetStatus("Nie można otworzyć okna dialogowego.");
            return;
        }

        var filters = new List<FilePickerFileType>
        {
            new("Załączniki") { Patterns = new[] { "*.fasta", "*.fa", "*.txt", "*.csv", "*.png", "*.jpg", "*.jpeg" } }
        };

        var result = await topLevel.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Wybierz pliki załączników",
                AllowMultiple = true,
                FileTypeFilter = filters
            });

        if (result is null || result.Count == 0)
        {
            SetStatus("Nie wybrano plików.");
            return;
        }
        foreach (var storageFile in result)
        {
            try
            {
                var path = storageFile.TryGetLocalPath() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    var attachment = _dataService.SaveAttachment(_selectedSession.Id, _selectedEntry.Id, path);
                    _selectedEntry.Attachments.Add(attachment);
                }
            }
            catch (Exception ex)
            {
                SetStatus($"Błąd dodawania załącznika: {ex.Message}");
            }
        }

        _dataService.SaveSessions(_sessions);
        RefreshAttachmentsList();
        SetStatus("Załączniki zostały dodane.");
    }

    private void RemoveAttachmentButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedEntry is null)
        {
            SetStatus("Wybierz załącznik do usunięcia.");
            return;
        }

        if (AttachmentsList.SelectedItem is not Attachment attachment)
        {
            SetStatus("Wybierz załącznik z listy.");
            return;
        }

        _dataService.DeleteAttachment(attachment);
        _selectedEntry.Attachments.Remove(attachment);
        _dataService.SaveSessions(_sessions);

        RefreshAttachmentsList();
        SetStatus("Załącznik został usunięty.");
    }

    private async void ExportPdfButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedSession is null)
        {
            SetStatus("Wybierz sesję do eksportu.");
            return;
        }

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null)
        {
            SetStatus("Nie można otworzyć okna dialogowego.");
            return;
        }

        var filters = new List<FilePickerFileType>
        {
            new("PDF") { Patterns = new[] { "*.pdf" } }
        };

        var result = await topLevel.StorageProvider.SaveFilePickerAsync(
            new FilePickerSaveOptions
            {
                Title = "Zapisz raport PDF",
                DefaultExtension = "pdf",
                SuggestedFileName = $"Raport_{_selectedSession.Title}_{DateTime.Now:yyyyMMdd}.pdf",
                FileTypeChoices = filters
            });

        if (result is null)
        {
            SetStatus("Eksport przerwany.");
            return;
        }

        try
        {
            var path = result.TryGetLocalPath() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(path))
            {
                _reportService.GenerateReport(_selectedSession, path, _dataService.DataDirectory);
                SetStatus($"Raport PDF został zapisany: {path}");
            }
            else
            {
                SetStatus("Nie udało się uzyskać ścieżki pliku.");
            }
        }
        catch (Exception ex)
        {
            SetStatus($"Błąd eksportu PDF: {ex.Message}");
        }
    }

    private void SetStatus(string message)
    {
        StatusText.Text = message;
    }
}
