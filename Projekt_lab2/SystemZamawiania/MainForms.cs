using System;
using System.Drawing;
using System.Windows.Forms;
//using MojaBiblioteka; // póżniej podłączyć, na razie bez kombinowania bo są błędy

namespace SystemZamawiania;
//tworzenie głównego okna aplikacji
public partial class MainForms : Form
{
    private ListView lvKoszyk = new ListView
    {
        Location = new Point(20, 40),
        Size = new Size(250, 200),
        View = System.Windows.Forms.View.Details, //widok jak tabelka
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
        BackColor = Color.LightPink //graphic design is my passion <3
    };

    private Label lblCena = new Label
    {
        Text = "Suma: 0.00 zł",
        Location = new Point(20, 250),
        Font = new Font("Arial", 10, FontStyle.Bold),
        AutoSize = true
    };

    public MainForms()
    {
        // InitializeComponent(); // zakomentowane bo był błąd w designerze
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
        btnTransport.Click += BtnTransport_Click;
        btnZaplac.Click += BtnZaplac_Click;

        UpdateTotal(); // Inicjalizacja sumy
    }

    private void UpdateTotal()
    {
        double total = 0.0;
        foreach (ListViewItem item in lvKoszyk.Items)
        {
            string cenaStr = item.SubItems[1].Text; // np. "15 zł"
            string cenaBezZl = cenaStr.Replace(" zł", "").Trim();
            if (double.TryParse(cenaBezZl, out double cena))
            {
                total += cena;
            }
        }
        lblCena.Text = $"Suma: {total:F2} zł";
    }

    private void BtnWybierz_Click(object? sender, EventArgs e)
    {
        // Tworzenie drugiego okna i przekazanie danych przez konstruktor
        using (ProduktyForms oknoProduktow = new ProduktyForms("Wybierz produkt:")) //otwieranie
        {
            // Wyświetlenie okna jako modalne (blokuje okno główne do czasu zamknięcia)
            if (oknoProduktow.ShowDialog() == DialogResult.OK)
            {
                //pobiera produkt z drugiego okna
                string produkt = oknoProduktow.WybranyProdukt;

                // Rozdzielenie nazwy od ceny 
                string[] czesci = produkt.Split(" - ");

                // Dodanie produktu do koszyka w MainForm
                ListViewItem item = new ListViewItem(czesci[0]);
                item.SubItems.Add(czesci[1]); // Cena
                lvKoszyk.Items.Add(item);

                UpdateTotal(); // Aktualizacja sumy po dodaniu produktu

                MessageBox.Show($"Dodano do koszyka: {czesci[0]}");
            }
        }
    }

    private void BtnTransport_Click(object? sender, EventArgs e)
    {
        // Tworzenie okna z opcjami transportu
        using (TransportForms oknoTransport = new TransportForms())
        {
            // Ustawienie właściwości przed pokazaniem okna
            oknoTransport.Variable1 = "Wartość 1";
            oknoTransport.Variable2 = "Wartość 2";

            if (oknoTransport.ShowDialog() == DialogResult.OK)
            {
                string transport = oknoTransport.WybranyTransport;
                MessageBox.Show($"Wybrany transport: {transport}");
            }
        }
    }

    private void BtnZaplac_Click(object? sender, EventArgs e)
    {
        // Ustawienie zmiennych globalnych 
        Globals.Variable1 = "Wartość 1 dla płatności";
        Globals.Variable2 = "Wartość 2 dla płatności";

        // Tworzenie okna płatności
        using (ZaplatyForms oknoZaplac = new ZaplatyForms())
        {
            if (oknoZaplac.ShowDialog() == DialogResult.OK)
            {
                string zaplata = oknoZaplac.WybranaZaplata;
                MessageBox.Show($"Wybrana forma płatności: {zaplata}");
            }
        }
    }
}
