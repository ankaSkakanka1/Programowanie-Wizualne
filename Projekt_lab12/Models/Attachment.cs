using System;
using System.IO;

namespace Projekt_lab12.Models;

public class Attachment
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string FileName { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;

    public string Extension => Path.GetExtension(FileName).ToLowerInvariant();
}
