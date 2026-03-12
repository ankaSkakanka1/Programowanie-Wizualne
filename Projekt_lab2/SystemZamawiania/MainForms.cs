using System;
using System.Drawing;
using System.Windows.Forms;
//using MojaBiblioteka; // Tu może być błąd, jeśli DLL nie jest podpięta - naprawimy to w kroku 2

namespace SystemZamawiania;

public partial class MainForm : Form
{
    private ListView lvKoszyk = new ListView 
    { 
        Location = new Point(20, 40), 
        Size = new Size(250, 200), 
        View = System.Windows.Forms.View.Details, 
        FullRowSelect = true 
    };

    private Button btnWybierz = new Button 
    { 
        Text = "Wybierz produkt", 
        Location = new Point(280, 40), 
        Width = 120 
    };

    private Button btnTransport = new Button 
    { 
        Text = "Transport", 
        Location = new Point(280, 80), 
        Width = 120 
    };

    private Button btnZaplac = new Button 
    { 
        Text = "Zapłać", 
        Location = new Point(280, 120), 
        Width = 120,
        BackColor = Color.LightGreen 
    };

    private Label lblCena = new Label 
    { 
        Text = "Suma: 0.00 zł", 
        Location = new Point(20, 250), 
        Font = new Font("Arial", 10, FontStyle.Bold),
        AutoSize = true 
    };

    public MainForm()
    {
        // InitializeComponent(); // Jeśli masz błąd w Designerze, zakomentuj tę linię na chwilę
        this.Text = "System Zamawiania Jedzenia";
        this.Size = new Size(450, 350);

        lvKoszyk.Columns.Add("Produkt", 150);
        lvKoszyk.Columns.Add("Cena", 80);

        this.Controls.Add(lvKoszyk);
        this.Controls.Add(btnWybierz);
        this.Controls.Add(btnTransport);
        this.Controls.Add(btnZaplac);
        this.Controls.Add(lblCena);

        btnWybierz.Click += BtnWybierz_Click;
    }

    private void BtnWybierz_Click(object sender, EventArgs e) 
    {
        // Tutaj otworzymy podokno w kolejnym kroku
    }
}