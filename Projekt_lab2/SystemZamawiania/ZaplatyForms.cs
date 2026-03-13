using System;
using System.Drawing;
using System.Windows.Forms;

namespace SystemZamawiania;

public partial class ZaplatyForms : Form
{
    private RadioButton rbZaplaty = new RadioButton { Location = new Point(10, 40), Size = new Size(200, 30) };
    private RadioButton rbBlik = new RadioButton { Location = new Point(10, 70), Size = new Size(200, 30) };
    private Button btnZaplac = new Button { Text = "Zaplać", Location = new Point(10, 110), Width = 120 };

    public string WybranaZaplata { get; private set; } = "";

    public ZaplatyForms()
    {
        this.Text = "Opcje Płatności";
        this.Size = new Size(300, 180);
        this.StartPosition = FormStartPosition.CenterParent;

        Label lbl = new Label { Text = "Wybierz formę płatności:", Location = new Point(10, 10), AutoSize = true };
        Label lblGlobals = new Label { Text = $"Variable1: {Globals.Variable1}, Variable2: {Globals.Variable2}", Location = new Point(10, 140), AutoSize = true };

        rbZaplaty.Text = "Płatność kartą";
        rbBlik.Text = "Płatność blikiem";

        this.Controls.Add(lbl);
        this.Controls.Add(rbZaplaty);
        this.Controls.Add(rbBlik);
        this.Controls.Add(btnZaplac);
        this.Controls.Add(lblGlobals);

        btnZaplac.Click += (s, e) =>
        {
            if (rbZaplaty.Checked)
            {
                WybranaZaplata = rbZaplaty.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (rbBlik.Checked)
            {
                WybranaZaplata = rbBlik.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Proszę wybrać formę płatności!");
            }
        };
    }
}