using System;
using System.Drawing;
using System.Windows.Forms;
namespace EdytorObrazka;

public partial class Form1 : Form
{
    private PictureBox pictureBox;
    private Button btnLoad;
    private Button btnOnlyGreen;
    public Form1()
    {
        InitializeComponent();
        SetupLayout();
    }

    private void SetupLayout()
    {
        this.Text = "Edytor Obrazu";
        this.Size = new Size(800, 600);

        btnLoad = new Button();
        btnLoad.Text = "Load";
        btnLoad.Location = new Point(20, 400); 
        btnLoad.Click += (s, e) => loadimage();
        this.Controls.Add(btnLoad);

        btnOnlyGreen = new Button { 
        Text = "OnlyGreen", 
        Location = new Point(20, 280), 
        Width = 100, 
        BackColor = Color.LightGreen 
        };
        btnOnlyGreen.Click += (s, e) => onlyGreen();
        this.Controls.Add(btnOnlyGreen);

        pictureBox = new PictureBox();
        pictureBox.Location = new Point(150, 20);
        pictureBox.Size = new Size(600, 500);
        pictureBox.BorderStyle = BorderStyle.FixedSingle;
        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        this.Controls.Add(pictureBox);
    }

    private void loadimage()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Bitmap Files (*.bmp)|*.bmp|All files (*.*)|*.*";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            pictureBox.Image = new Bitmap(openFileDialog.FileName);
        }
    }

    private void onlyGreen()
   {
      if (pictureBox.Image == null) return;
       Bitmap bmp = new Bitmap(pictureBox.Image);

       for (int y = 0; y < bmp.Height; y++)
       {
          for (int x = 0; x < bmp.Width; x++)
          {
             Color p = bmp.GetPixel(x, y);
             if (!(p.G > p.R && p.G > p.B))
             {
                bmp.SetPixel(x, y, Color.Black);
             }
           }
        }
        pictureBox.Image = bmp;
    }

}
