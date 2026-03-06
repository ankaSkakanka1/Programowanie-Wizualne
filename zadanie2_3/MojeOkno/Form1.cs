using MojaBiblioteka;

namespace MojeOkno;

public partial class Form1 : Form
{
    // Pola tekstowe
    private TextBox txtA = new TextBox { Location = new Point(20, 20), Width = 50 };
    private TextBox txtB = new TextBox { Location = new Point(80, 20), Width = 50 };
    
    // Przyciski
    private Button btnDodaj = new Button { Text = "+", Location = new Point(20, 50), Width = 30 };
    private Button btnOdejmij = new Button { Text = "-", Location = new Point(60, 50), Width = 30 };
    private Button btnPomnoz = new Button { Text = "*", Location = new Point(100, 50), Width = 30 };
    private Button btnPodziel = new Button { Text = "/", Location = new Point(140, 50), Width = 30 };
    
    // Wynik
    private Label lblWynik = new Label { Text = "Wynik: ?", Location = new Point(20, 90), AutoSize = true };

    public Form1()
    {
        InitializeComponent();
        this.Text = "Kalkulator DLL v2";

        // Dodanie wszystkiego do okna
        this.Controls.AddRange(new Control[] { txtA, txtB, btnDodaj, btnOdejmij, btnPomnoz, btnPodziel, lblWynik });

        Kalkulator k = new Kalkulator();

        // Logika dla każdego przycisku
        btnDodaj.Click += (s, e) => Wykonaj(k.Dodaj);
        btnOdejmij.Click += (s, e) => Wykonaj(k.Odejmij);
        btnPomnoz.Click += (s, e) => Wykonaj(k.Pomnoz);
        
        // Dzielenie obsługujemy osobno bo zwraca doubl i ustawienie dwóch miejcs po przecinku
        btnPodziel.Click += (s, e) => {
            if (PobierzLiczby(out int a, out int b)) {
                lblWynik.Text = $"Wynik: {k.Podziel(a, b):F2}";
            }
        };
    }

    private void Wykonaj(Func<int, int, int> operacja) {
        if (PobierzLiczby(out int a, out int b)) {
            lblWynik.Text = $"Wynik: {operacja(a, b)}";
        }
    }

    private bool PobierzLiczby(out int a, out int b) {
        if (int.TryParse(txtA.Text, out a) && int.TryParse(txtB.Text, out b)) return true;
        MessageBox.Show("Wpisz poprawne liczby!");
        b = 0; return false;
    }
}