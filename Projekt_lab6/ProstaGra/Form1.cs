using System;
using System.Windows.Forms;

namespace ProstaGra
{
    public partial class Form1 : Form
    {
        NumericUpDown numWidth, numHeight, numTime, numDydelf, numRaccoon, numCroc;
        Button btnStart;

        public Form1()
        {
            this.Text = "Menu";
            this.Width = 300;
            this.Height = 350;

            InitUI();
        }

        private void InitUI()
        {
            Label l1 = new Label() { Text = "Width", Top = 20, Left = 10 };
            numWidth = new NumericUpDown() { Top = 20, Left = 120, Minimum = 3, Maximum = 10, Value = 5 };

            Label l2 = new Label() { Text = "Height", Top = 50, Left = 10 };
            numHeight = new NumericUpDown() { Top = 50, Left = 120, Minimum = 3, Maximum = 10, Value = 5 };

            Label l3 = new Label() { Text = "Time", Top = 80, Left = 10 };
            numTime = new NumericUpDown() { Top = 80, Left = 120, Minimum = 10, Maximum = 60, Value = 30 };

            Label l4 = new Label() { Text = "Dydelfs", Top = 110, Left = 10 };
            numDydelf = new NumericUpDown() { Top = 110, Left = 120, Minimum = 1, Maximum = 6, Value = 2 };

            Label l5 = new Label() { Text = "Raccoons", Top = 140, Left = 10 };
            numRaccoon = new NumericUpDown() { Top = 140, Left = 120, Minimum = 3, Maximum = 8, Value = 3 };

            Label l6 = new Label() { Text = "Crocs", Top = 170, Left = 10 };
            numCroc = new NumericUpDown() { Top = 170, Left = 120, Minimum = 0, Maximum = 1, Value = 1 };

            btnStart = new Button() { Text = "Start", Top = 220, Left = 40 };

            btnStart.Click += BtnStart_Click;

            this.Controls.AddRange(new Control[] {
                l1, numWidth,
                l2, numHeight,
                l3, numTime,
                l4, numDydelf,
                l5, numRaccoon,
                l6, numCroc,
                btnStart
            });
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            OknoGry game = new OknoGry(
                (int)numWidth.Value,
                (int)numHeight.Value,
                (int)numTime.Value,
                (int)numDydelf.Value,
                (int)numRaccoon.Value,
                (int)numCroc.Value
            );

            game.Show();
            this.Close();
        }
    }
}