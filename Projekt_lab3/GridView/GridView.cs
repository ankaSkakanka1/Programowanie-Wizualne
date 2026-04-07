using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data; 
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace GridView;
public partial class Form1 : Form 
{
    private BindingSource bindingSource = new BindingSource();
    private DataGridView dataGridView1 = new DataGridView(); 
    private Button btnDodaj = new Button();
    private Button btnUsun = new Button();
    private Button btnZapis = new Button();
    private Button btnOdczyt = new Button();
    private Button btnXML = new Button();


    public Form1()
    {
        this.Text = "Tabela";
        this.Size = new Size(800, 500);

        DataTable dataTable = new DataTable();
        
        dataTable.Columns.Add("ID", typeof(int));
        dataTable.Columns.Add("Imie", typeof(string));
        dataTable.Columns.Add("Nazwisko", typeof(string));
        dataTable.Columns.Add("Wiek", typeof(int));
        dataTable.Columns.Add("Stanowisko", typeof(string));

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

        btnXML.Text = "Zapis XML";
        btnXML.Location = new Point(450, 320);
        btnXML.Size = new Size(100, 30);
        btnXML.Click += btnXML_Click;
        this.Controls.Add(btnXML);

    }

    private void btnDodaj_Click(object sender, EventArgs e)
    { 
        Form2 dodawaniePracownika = new Form2();
        if (dodawaniePracownika.ShowDialog() == DialogResult.OK)
        {
            DataTable? dataTable = bindingSource.DataSource as DataTable;
            if(dataTable != null)
            {
                int noweID = 1;
                if(dataTable.Rows.Count > 0)
                {
                    noweID = Convert.ToInt32(dataTable.Compute("max([ID])", string.Empty)) + 1;
                }

                dataTable.Rows.Add(noweID, dodawaniePracownika.txtImie.Text, dodawaniePracownika.txtNazwisko.Text, (int)dodawaniePracownika.numWiek.Value, dodawaniePracownika.cmbStanowisko.SelectedItem.ToString() ?? "Brak");
            }
        }
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

    private void btnZapis_Click(object? sender, EventArgs e)
   {
    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
    saveFileDialog1.Filter = "Pliki CSV (*.csv)|*.csv|Wszystkie pliki (*.*)|*.*";
    saveFileDialog1.Title = "Zapisz dane pracowników do pliku CSV";

    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
    {
        ZapiszDoCSV(dataGridView1, saveFileDialog1.FileName);
    }
   }  

private void ZapiszDoCSV(DataGridView dgv, string filePath)
{
    try
    {
        var columnNames = dgv.Columns.Cast<DataGridViewColumn>()
                            .Select(column => column.HeaderText);
        string header = string.Join(",", columnNames);

        var rows = dgv.Rows.Cast<DataGridViewRow>()
                    .Where(row => !row.IsNewRow)
                    .Select(row => string.Join(",", row.Cells.Cast<DataGridViewCell>()
                                                    .Select(cell => cell.Value?.ToString() ?? "")));

        string csvContent = header + Environment.NewLine + string.Join(Environment.NewLine, rows);

        File.WriteAllText(filePath, csvContent);

        MessageBox.Show("Dane zostały pomyślnie zapisane!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show("Błąd podczas zapisu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

   private void btnOdczyt_Click(object? sender, EventArgs e)
{
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.Filter = "Pliki CSV (*.csv)|*.csv|Wszystkie pliki (*.*)|*.*";
    openFileDialog1.Title = "Wybierz plik CSV do wczytania";

    if (openFileDialog1.ShowDialog() == DialogResult.OK)
    {
        LoadCSVToDataGridView(openFileDialog1.FileName);
    }
}

private void LoadCSVToDataGridView(string filePath)
{
    if (!File.Exists(filePath))
    {
        MessageBox.Show("Plik CSV nie istnieje.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    try
    {
        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length > 0)
        {
            DataTable dt = new DataTable();
            string[] headers = lines[0].Split(',');
            foreach (string header in headers)
            {
                dt.Columns.Add(header.Trim());
            }

            for (int i = 1; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    string[] values = lines[i].Split(',');
                    dt.Rows.Add(values);
                }
            }

            bindingSource.DataSource = dt;
            
            MessageBox.Show("Dane zostały wczytane pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Wystąpił błąd podczas odczytu pliku: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private List<Osoba> PobierzOsoby()
{
    List<Osoba> lista = new List<Osoba>();

    foreach (DataGridViewRow row in dataGridView1.Rows)
    {
        if (!row.IsNewRow)
        {
            lista.Add(new Osoba
            {
                ID = Convert.ToInt32(row.Cells["ID"].Value),
                Imie = row.Cells["Imie"].Value.ToString(),
                Nazwisko = row.Cells["Nazwisko"].Value.ToString(),
                Wiek = Convert.ToInt32(row.Cells["Wiek"].Value),
                Stanowisko = row.Cells["Stanowisko"].Value.ToString()
            });
        }
    }

    return lista;
}

private void btnXML_Click(object sender, EventArgs e)
{
    List<Osoba> osoby = PobierzOsoby();

    XmlSerializer serializer = new XmlSerializer(typeof(List<Osoba>));

    using (FileStream fs = new FileStream("osoby.xml", FileMode.Create))
    {
        serializer.Serialize(fs, osoby);
    }

    MessageBox.Show("Zapisano do XML!");
}
}
