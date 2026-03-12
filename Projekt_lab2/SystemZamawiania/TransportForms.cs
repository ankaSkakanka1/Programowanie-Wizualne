using System.Windows.Forms;

namespace SystemZamawiania;

public partial class TransportForms : Form
{
    // Publiczna właściwość (Komunikacja nr 2 z instrukcji)
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string WybranyTransport { get; set; } = "";

    public TransportForms()
    {
        this.Text = "Opcje Transportu";
        this.Size = new Size(300, 200);
    }
}