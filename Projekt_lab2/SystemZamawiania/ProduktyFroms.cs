using System;
using System.Drawing;
using System.Windows.Forms;

namespace SystemZamawiania;

public partial class ProduktyForms : Form
{
    private ListBox lbProdukty = new ListBox { Location = new Point(10, 40), Size = new Size(260, 200) };
    private Button btnDodaj = new Button { Text = "Dodaj do koszyka", Location = new Point(10, 250), Width = 120 };
    
    // Właściwość publiczna, przez którą MainForm odbierze wybrany produkt
    public string WybranyProdukt { get; private set; } = "";
//tu jest konstruktor
    public ProduktyForms(string infoOdMain)
    {
        this.Text = "Wybierz Produkty";
        this.Size = new Size(300, 350);
        this.StartPosition = FormStartPosition.CenterParent;

        Label lbl = new Label { Text = infoOdMain, Location = new Point(10, 10), AutoSize = true };
        
        //adrange-dodaje elementy do listboxa
        lbProdukty.Items.AddRange(new string[] { 
            "Pizza Margherita - 30 zł", 
            "Burger Wołowy - 25 zł", 
            "Sałatka Cezar - 20 zł",
            "Pasta Carbonara - 28 zł" 
        });

        this.Controls.Add(lbl);
        this.Controls.Add(lbProdukty);
        this.Controls.Add(btnDodaj);

        //gdy użytkownik kilknie dodaj
        btnDodaj.Click += (s, e) => {
            if (lbProdukty.SelectedItem != null) {
                WybranyProdukt = lbProdukty.SelectedItem.ToString() ?? "";
                this.DialogResult = DialogResult.OK; // Ustawienie wyniku na OK
                this.Close(); // Zamknięcie okna
            } else {
                MessageBox.Show("Proszę najpierw wybrać produkt z listy!");
            }
        };
    }
}