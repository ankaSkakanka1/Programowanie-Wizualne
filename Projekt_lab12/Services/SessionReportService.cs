using System;
using System.IO;
using System.Linq;
using Projekt_lab12.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Projekt_lab12.Services;

public class SessionReportService
{
    public void GenerateReport(Session session, string destinationPath, string dataDirectory)
    {
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.MarginVertical(20);
                page.MarginHorizontal(20);
                page.PageColor("#FFFFFF");
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Column(column =>
                {
                    column.Item().Text("Raport sesji analitycznej").FontSize(20).SemiBold();
                    column.Item().Text(session.Title).FontSize(16).SemiBold().FontColor("#1F4788");
                    column.Item().Text($"Data utworzenia: {session.CreatedAt:yyyy-MM-dd HH:mm}").FontSize(12).FontColor("#666666");
                });

                page.Content().Column(column =>
                {
                    if (session.Entries.Count == 0)
                    {
                        column.Item().Padding(10).Text("Brak wpisów w tej sesji.");
                        return;
                    }

                    foreach (var entry in session.Entries.OrderBy(e => e.CreatedAt))
                    {
                        column.Item().Padding(8).Border(1).BorderColor("#CCCCCC").Column(entryColumn =>
                        {
                            entryColumn.Item().Text(entry.Title).SemiBold().FontSize(14);
                            entryColumn.Item().Text($"Data wpisu: {entry.CreatedAt:yyyy-MM-dd HH:mm}").FontSize(11).FontColor("#666666");
                            entryColumn.Item().PaddingTop(6).Text(entry.Notes).FontSize(12);

                            if (entry.Attachments.Any())
                            {
                                entryColumn.Item().PaddingTop(8).Text("Załączniki:").Bold();
                                entryColumn.Item().Column(attachments =>
                                {
                                    foreach (var attachment in entry.Attachments)
                                    {
                                        attachments.Item().Text($"• {attachment.FileName}").FontSize(11);
                                        if (attachment.Extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                                        {
                                            var filePath = Path.Combine(dataDirectory, attachment.RelativePath);
                                            if (File.Exists(filePath))
                                            {
                                                try
                                                {
                                                    attachments.Item().PaddingTop(4).Image(filePath);
                                                }
                                                catch (Exception ex)
                                                {
                                                    attachments.Item().Text($"(Błąd wczytania obrazu: {ex.Message})").FontSize(9).FontColor("#FF0000");
                                                }
                                            }
                                        }
                                    }
                                });
                            }
                        });
                    }
                });

                page.Footer().AlignCenter().Text($"Wygenerowano: {DateTime.Now:yyyy-MM-dd HH:mm}").FontSize(10).FontColor("#666666");
            });
        }).GeneratePdf(destinationPath);
    }
}
