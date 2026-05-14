namespace AnalizaFasta.Models;

public class FastaSequence
{
    public string Header { get; set; } = string.Empty;
    public string Sequence { get; set; } = string.Empty;
    public string Name => Header.Split(' ')[0].TrimStart('>');
    public int Length => Sequence.Length;

    public FastaSequence(string header, string sequence)
    {
        Header = header;
        Sequence = sequence.ToUpper();
    }

    public void Analyze(out int a, out int t, out int g, out int c, out double gcContent, out int codonCount, out bool isValid)
    {
        a = 0;
        t = 0;
        g = 0;
        c = 0;
        isValid = true;

        foreach (char ch in Sequence)
        {
            if (ch == 'A') a++;
            else if (ch == 'T') t++;
            else if (ch == 'G') g++;
            else if (ch == 'C') c++;
            else if (ch != 'N' && ch != 'X' && !char.IsWhiteSpace(ch))
            {
                isValid = false;
            }
        }

        int total = a + t + g + c;
        gcContent = total > 0 ? (double)(g + c) / total * 100 : 0;
        codonCount = Sequence.Length / 3;
    }
}
