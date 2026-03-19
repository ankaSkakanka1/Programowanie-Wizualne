using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data; 

namespace GridView;
public partial class Form1 : Form 
{
    private BindingSource bindingSource = new BindingSource();
    private DataGridView dataGridView1 = new DataGridView(); 
    private Button btnDodaj = new Button();
    private Button btnUsun = new Button();
    private Button btnZapis = new Button();
    private Button btnOdczyt = new Button();


    public Form1()
    {
        this.Text = "Tabela";
        this.Size = new Size(800, 500);

        DataTable dataTable = new DataTable();
        
        dataTable.Columns.Add("ID", typeof(int));
        dataTable.Columns.Add("Imię", typeof(string));
        dataTable.Columns.Add("Nazwisko", typeof(string));
        dataTable.Columns.Add("Wiek", typeof(int));
        dataTable.Columns.Add("Stanowisko", typeof(string));

        dataTable.Rows.Add(1, "Jan", "Kowalski", 30, "Asystent");
        dataTable.Rows.Add(2, "Iza", "Pełka", 20, "Asystent");

        bindingSource.DataSource = dataTable;
        
        dataGridView1.Dock = DockStyle.Top; 
        dataGridView1.Height = 300;
        dataGridView1.DataSource = bindingSource;

        this.Controls.Add(dataGridView1);

        btnDodaj.Text = "Dodaj"; 
        btnDodaj.Location = new Point(10, 320);
        btnDodaj.Size = new Size(100, 30);
        btnDodaj.Click += new EventHandler(btnDodaj_Click); // Przypisanie akcji 
        this.Controls.Add(btnDodaj);

        // Przycisk USUŃ
        btnUsun.Text = "Usuń";
        btnUsun.Location = new Point(120, 320);
        btnUsun.Size = new Size(100, 30);
        btnUsun.Click += new EventHandler(btnUsun_Click); // Przypisanie akcji 
        this.Controls.Add(btnUsun);

        btnZapis.Text = "Zapis do .csv"; 
        btnZapis.Location = new Point(230, 320);
        btnZapis.Size = new Size(100, 30);
        btnZapis.Click += new EventHandler(btnZapis_Click); // Przypisanie akcji 
        this.Controls.Add(btnZapis);

        btnOdczyt.Text = "Odczyt z .csv";
        btnOdczyt.Location = new Point(340, 320);
        btnOdczyt.Size = new Size(100, 30);
        btnOdczyt.Click += new EventHandler(btnOdczyt_Click); // Przypisanie akcji 
        this.Controls.Add(btnOdczyt);

    }

    private void btnDodaj_Click(object sender, EventArgs e)
    { 
        MessageBox.Show("Tutaj otworzy się okno dodawania pracownika.");
    }

    private void btnUsun_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
        {
            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
        }
        else
        {
            MessageBox.Show("Proszę zaznaczyć wiersz do usunięcia.");
        }
    }

    private void btnZapis_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Tutaj coś tam coś tam XD");
    }

    private void btnOdczyt_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Tutaj coś tam coś tam XD");
    }
}
