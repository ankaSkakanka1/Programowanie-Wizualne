namespace MojaBiblioteka;

public class Kalkulator
{
    public int Dodaj(int a, int b) => a + b;
    public int Odejmij(int a, int b) => a - b;
    public int Pomnoz(int a, int b) => a * b;
    public double Podziel(int a, int b)
    {
            if (b == 0) return 0; 
            return (double)a / b;
    }
}
