using System.Windows.Forms;

namespace SystemZamawiania;

public partial class ProduktyFroms : Form
{
    // Konstruktor przyjmujący parametry (Komunikacja nr 1 z instrukcji)
    public ProduktyFroms(string infoOdMain)
    {
        this.Text = "Wybierz Produkty";
        this.Size = new Size(300, 400);
        
        Label lbl = new Label { Text = infoOdMain, Location = new Point(10, 10), AutoSize = true };
        this.Controls.Add(lbl);
    }
}