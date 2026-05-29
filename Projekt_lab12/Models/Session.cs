using System;
using System.Collections.ObjectModel;

namespace Projekt_lab12.Models;

public class Session
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ObservableCollection<Entry> Entries { get; set; } = new();
}
