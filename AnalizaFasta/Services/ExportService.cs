using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AnalizaFasta.Models;

namespace AnalizaFasta.Services;

public class ExportService
{
    public static async Task<bool> ExportToCsvAsync(List<FastaSequence> sequences, string filePath)
    {
        try
        {
            var sb = new StringBuilder();
            sb.AppendLine("Nazwa,Długość,A,T,G,C,GC%,Kodony,Załadowano");

            foreach (var seq in sequences)
            {
                seq.Analyze(out int a, out int t, out int g, out int c, out double gc, out int codons, out bool valid);
                sb.AppendLine($"{seq.Name},{seq.Length},{a},{t},{g},{c},{gc:F2},{codons},{valid}");
            }

            await File.WriteAllTextAsync(filePath, sb.ToString());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static async Task<bool> ExportToTxtAsync(List<FastaSequence> sequences, string filePath)
    {
        try
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== ANALIZA SEKWENCJI FASTA ===");
            sb.AppendLine($"Data: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Liczba sekwencji: {sequences.Count}");
            sb.AppendLine(new string('=', 40));
            sb.AppendLine();

            foreach (var seq in sequences)
            {
                seq.Analyze(out int a, out int t, out int g, out int c, out double gc, out int codons, out bool valid);

                sb.AppendLine($"Nazwa: {seq.Name}");
                sb.AppendLine($"Nagłówek: {seq.Header}");
                sb.AppendLine($"Długość sekwencji: {seq.Length}");
                sb.AppendLine($"Zasada A: {a}");
                sb.AppendLine($"Zasada T: {t}");
                sb.AppendLine($"Zasada G: {g}");
                sb.AppendLine($"Zasada C: {c}");
                sb.AppendLine($"Zawartość GC: {gc:F2}%");
                sb.AppendLine($"Liczba kodonów: {codons}");
                sb.AppendLine($"Format prawidłowy: {(valid ? "Tak" : "Nie")}");
                sb.AppendLine();
            }

            await File.WriteAllTextAsync(filePath, sb.ToString());
            return true;
        }
        catch
        {
            return false;
        }
    }
}
