using System;
using System.Drawing;
using System.Windows.Forms;

namespace SystemZamawiania;

public partial class TransportForms : Form
{
    private CheckedListBox clbTransport = new CheckedListBox { Location = new Point(10, 40), Size = new Size(260, 120) };
    private Button btnWybierz = new Button { Text = "Wybierz transport", Location = new Point(10, 170), Width = 120 };

    // Publiczna właściwość 
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string WybranyTransport { get; private set; } = "";

    public TransportForms()
    {
        this.Text = "Opcje Transportu";
        this.Size = new Size(300, 250);
        this.StartPosition = FormStartPosition.CenterParent;

        Label lbl = new Label { Text = "Wybierz sposób dostawy:", Location = new Point(10, 10), AutoSize = true };

        clbTransport.Items.AddRange(new string[]
        {
            "Kurier DPD - 15 zł",
            "InPost Paczkomat - 12 zł",
            "Orlen Paczka - 10 zł"
        });

        this.Controls.Add(lbl);
        this.Controls.Add(clbTransport);
        this.Controls.Add(btnWybierz);

        // Obsługa kliknięcia przycisku
        btnWybierz.Click += (s, e) => {
            if (clbTransport.CheckedItems.Count > 0)
            {
                WybranyTransport = clbTransport.CheckedItems[0].ToString() ?? "";
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Proszę wybrać sposób transportu!");
            }
        };
    }
}