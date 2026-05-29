using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Projekt_lab12.Models;

namespace Projekt_lab12.Services;

public class NotebookDataService
{
    private readonly string _dataDirectory;
    private readonly string _sessionsFile;
    private readonly string _attachmentsDirectory;

    public NotebookDataService(string dataDirectory)
    {
        _dataDirectory = dataDirectory;
        _sessionsFile = Path.Combine(_dataDirectory, "sessions.json");
        _attachmentsDirectory = Path.Combine(_dataDirectory, "attachments");

        Directory.CreateDirectory(_dataDirectory);
        Directory.CreateDirectory(_attachmentsDirectory);
    }

    public string DataDirectory => _dataDirectory;

    public List<Session> LoadSessions()
    {
        if (!File.Exists(_sessionsFile))
            return new List<Session>();

        var json = File.ReadAllText(_sessionsFile);
        var sessions = JsonSerializer.Deserialize<List<Session>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Session>();

        // Convert List<Entry> to ObservableCollection<Entry>
        foreach (var session in sessions)
        {
            var entries = session.Entries.ToList();
            session.Entries.Clear();
            foreach (var entry in entries)
            {
                var attachments = entry.Attachments.ToList();
                entry.Attachments.Clear();
                foreach (var attachment in attachments)
                {
                    entry.Attachments.Add(attachment);
                }
                session.Entries.Add(entry);
            }
        }

        return sessions;
    }

    public void SaveSessions(IEnumerable<Session> sessions)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(sessions, options);
        File.WriteAllText(_sessionsFile, json);
    }

    public Attachment SaveAttachment(string sessionId, string entryId, string sourcePath)
    {
        var extension = Path.GetExtension(sourcePath);
        var destinationDirectory = Path.Combine(_attachmentsDirectory, sessionId, entryId);
        Directory.CreateDirectory(destinationDirectory);

        var fileId = Guid.NewGuid().ToString("N");
        var destinationFileName = fileId + extension;
        var destinationPath = Path.Combine(destinationDirectory, destinationFileName);

        File.Copy(sourcePath, destinationPath, overwrite: true);

        var relativePath = Path.GetRelativePath(_dataDirectory, destinationPath);

        return new Attachment
        {
            FileName = Path.GetFileName(sourcePath),
            RelativePath = relativePath
        };
    }

    public void DeleteAttachment(Attachment attachment)
    {
        var filePath = Path.Combine(_dataDirectory, attachment.RelativePath);
        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    public void DeleteEntryAttachments(string sessionId, string entryId)
    {
        var entryFolder = Path.Combine(_attachmentsDirectory, sessionId, entryId);
        if (Directory.Exists(entryFolder))
            Directory.Delete(entryFolder, recursive: true);
    }

    public void DeleteSessionAttachments(string sessionId)
    {
        var sessionFolder = Path.Combine(_attachmentsDirectory, sessionId);
        if (Directory.Exists(sessionFolder))
            Directory.Delete(sessionFolder, recursive: true);
    }

    public string ResolveAttachmentPath(Attachment attachment)
    {
        return Path.Combine(_dataDirectory, attachment.RelativePath);
    }
}
