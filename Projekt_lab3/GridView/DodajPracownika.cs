using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data; 

namespace GridView; 

public partial class Form2 : Form
{
    public TextBox txtImie = new TextBox();
    public TextBox txtNazwisko = new TextBox();
    public NumericUpDown numWiek = new NumericUpDown();
    public ComboBox cmbStanowisko = new ComboBox();
    private Button btnZatwierdz = new Button();
    private Button btnAnuluj = new Button();

    public Form2()
    {
        this.Text = "Dodaj Pracownika";
        this.Size = new Size(300, 400);
        this.FormBorderStyle = FormBorderStyle.FixedDialog; 

        Label lblImie = new Label() { Text = "Imię:", Location = new Point(20, 20), AutoSize = true };
        txtImie.Location = new Point(20, 40);
        txtImie.Size = new Size(240, 20);

        Label lblNazwisko = new Label() { Text = "Nazwisko:", Location = new Point(20, 70), AutoSize = true };
        txtNazwisko.Location = new Point(20, 90);
        txtNazwisko.Size = new Size(240, 20);

        Label lblWiek = new Label() { Text = "Wiek:", Location = new Point(20, 120), AutoSize = true };
        numWiek.Location = new Point(20, 140);
        numWiek.Minimum = 18;
        numWiek.Maximum = 100;

        Label lblStanowisko = new Label() { Text = "Stanowisko:", Location = new Point(20, 170), AutoSize = true };
        cmbStanowisko.Location = new Point(20, 190);
        cmbStanowisko.Size = new Size(240, 20);
        cmbStanowisko.Items.AddRange(new string[] { "Programista", "Tester", "Projektant", "Manager" });
        cmbStanowisko.SelectedIndex = 0; 

        btnZatwierdz.Text = "Zatwierdź";
        btnZatwierdz.Location = new Point(20, 250);
        btnZatwierdz.DialogResult = DialogResult.OK; 

        btnAnuluj.Text = "Anuluj";
        btnAnuluj.Location = new Point(140, 250);
        btnAnuluj.DialogResult = DialogResult.Cancel;

        this.Controls.Add(lblImie);
        this.Controls.Add(txtImie);
        this.Controls.Add(lblNazwisko);
        this.Controls.Add(txtNazwisko);
        this.Controls.Add(lblWiek);
        this.Controls.Add(numWiek);
        this.Controls.Add(lblStanowisko);
        this.Controls.Add(cmbStanowisko);
        this.Controls.Add(btnZatwierdz);
        this.Controls.Add(btnAnuluj);

        this.AcceptButton = btnZatwierdz;
    }
}