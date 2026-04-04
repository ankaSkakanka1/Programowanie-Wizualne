using System;
using System.Drawing;
using System.Windows.Forms;
namespace EdytorObrazka;

public partial class Form1 : Form
{
    private PictureBox pictureBox;
    private Button btnLoad;
    private RadioButton rb90, rb180, rb270;
    private Button btnRotate;
    private Button btnInvert;
    private Button btnUpsideDown;

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

        rb90 = new RadioButton { Text = "90°", Location = new Point(20, 50), Checked = true };
        rb180 = new RadioButton { Text = "180°", Location = new Point(20, 80) };
        rb270 = new RadioButton { Text = "270°", Location = new Point(20, 110) };

        btnRotate = new Button { Text = "Rotate", Location = new Point(20, 150), BackColor = Color.LightPink };
        btnRotate.Click += (s, e) => HandleRotation();
        this.Controls.AddRange(new Control[] { rb90, rb180, rb270, btnRotate });

        btnInvert = new Button { Text = "Invert Colors", Location = new Point(20, 200), Width = 100, BackColor = Color.LightBlue };
        btnInvert.Click += (s, e) => InvertColors();

        btnUpsideDown = new Button { Text = "Upside Down", Location = new Point(20, 240), Width = 100, BackColor = Color.LightBlue };
        btnUpsideDown.Click += (s, e) => UpsideDown();

        this.Controls.AddRange(new Control[] { btnInvert, btnUpsideDown });

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

     private void InvertColors() 
    {
        if (pictureBox.Image == null) return;
        Bitmap bmp = new Bitmap(pictureBox.Image);

        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                Color p = bmp.GetPixel(x, y);
                bmp.SetPixel(x, y, Color.FromArgb(p.A, 255 - p.R, 255 - p.G, 255 - p.B));
            }
        }
        pictureBox.Image = bmp;
    }

    private void UpsideDown() 
    {
        if (pictureBox.Image == null) return;
        pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
        pictureBox.Refresh();
    }

    private void HandleRotation()
    {
        if (pictureBox.Image == null) return;

        if (rb90.Checked) rotateImage90();
        else if (rb180.Checked) rotateImage180();
        else if (rb270.Checked) rotateImage270();
    }

    private void rotateImage90()
    {
        pictureBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
        pictureBox.Refresh();
    }

    private void rotateImage180()
    {
        pictureBox.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
        pictureBox.Refresh();
    }

    private void rotateImage270() 
    {
        pictureBox.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
        pictureBox.Refresh();
    }
}
