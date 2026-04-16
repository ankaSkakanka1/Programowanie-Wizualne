using System;
using System.Windows.Forms;
namespace ProstaGra;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

     private void btnStart_Click(object sender, EventArgs e)
        {
            int width = (int)numWidth.Value;
            int height = (int)numHeight.Value;
            int time = (int)numTime.Value;
            int dydelfs = (int)numDydelf.Value;
            int raccoons = (int)numRaccoon.Value;
            int crocs = (int)numCroc.Value;

            OknoGry game = new OknoGry(width, height, time, dydelfs, raccoons, crocs);
            game.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
}
