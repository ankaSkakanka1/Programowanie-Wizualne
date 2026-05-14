using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BioManager.Data;
using BioManager.Models;
using BioManager.Services;

namespace BioManager.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly AppDbContext _db = new();
    private readonly QrCodeService _qrService = new();

    private BiologicalSample _newSample = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<BiologicalSample> Samples { get; }

    public BiologicalSample NewSample
    {
        get => _newSample;
        set
        {
            if (Equals(_newSample, value))
            {
                return;
            }

            _newSample = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddSampleCommand { get; }
    public ICommand ExportQrCommand { get; }

    public MainWindowViewModel()
    {
        _db.Database.EnsureCreated();
        Samples = new ObservableCollection<BiologicalSample>(_db.Samples.ToList());
        AddSampleCommand = new DelegateCommand(_ => AddSample());
        ExportQrCommand = new DelegateCommand(parameter => ExportQr(parameter as BiologicalSample));
    }

    private void AddSample()
    {
        _db.Samples.Add(NewSample);
        _db.SaveChanges();
        Samples.Add(NewSample);
        NewSample = new BiologicalSample();
    }

    private void ExportQr(BiologicalSample? sample)
    {
        if (sample is null)
        {
            return;
        }

        var qrData = $"ID:{sample.Id}|Typ:{sample.Type}|Data:{sample.CollectionDate:yyyy-MM-dd}";
        var imageBytes = _qrService.GenerateQrCode(qrData);
        File.WriteAllBytes($"{sample.Id}_qr.png", imageBytes);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private sealed class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}