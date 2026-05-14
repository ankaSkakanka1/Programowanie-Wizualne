using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnalizaFasta.Models;

namespace AnalizaFasta.Services;

public class FastaParser
{
    public static async Task<(List<FastaSequence>, bool isValid, string message)> ParseAsync(string filePath)
    {
        var sequences = new List<FastaSequence>();
        string? currentHeader = null;
        var currentSequence = new System.Text.StringBuilder();
        bool isValid = true;
        string message = "Plik załadowany pomyślnie";

        try
        {
            var lines = await File.ReadAllLinesAsync(filePath);

            if (lines.Length == 0)
            {
                return (sequences, false, "Plik jest pusty");
            }

            if (!lines[0].StartsWith(">"))
            {
                return (sequences, false, "Błąd: Pierwszy wiersz musi być nagłówkiem (zaczynającym się od '>')");
            }

            foreach (var line in lines)
            {
                if (line.StartsWith(">"))
                {
                    if (currentHeader != null && currentSequence.Length > 0)
                    {
                        sequences.Add(new FastaSequence(currentHeader, currentSequence.ToString()));
                    }
                    currentHeader = line;
                    currentSequence.Clear();
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    currentSequence.Append(line.Trim());
                }
            }

            if (currentHeader != null && currentSequence.Length > 0)
            {
                sequences.Add(new FastaSequence(currentHeader, currentSequence.ToString()));
            }

            if (sequences.Count == 0)
            {
                return (sequences, false, "Błąd: Nie znaleziono sekwencji w pliku");
            }

            // Walidacja sekwencji
            foreach (var seq in sequences)
            {
                seq.Analyze(out _, out _, out _, out _, out _, out _, out bool valid);
                if (!valid)
                {
                    isValid = false;
                    message = "Ostrzeżenie: Niektóre sekwencje zawierają znaki inne niż ATGC (znaki N/X zostały zignorowane)";
                }
            }

            return (sequences, isValid, message);
        }
        catch (FileNotFoundException)
        {
            return (sequences, false, "Błąd: Plik nie znaleziony");
        }
        catch (Exception ex)
        {
            return (sequences, false, $"Błąd: {ex.Message}");
        }
    }
}
