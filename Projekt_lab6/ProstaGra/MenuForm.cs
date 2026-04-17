using System;
using System.Windows.Forms;

namespace ProstaGra
{
    public class MenuForm : Form
    {
        Button btnStart, btnSettings, btnExit;

        public MenuForm()
        {
            this.Text = "Menu główne";
            this.Width = 300;
            this.Height = 250;

            btnStart = new Button() { Text = "Start", Top = 30, Left = 80, Width = 120 };
            btnSettings = new Button() { Text = "Ustawienia", Top = 80, Left = 80, Width = 120 };
            btnExit = new Button() { Text = "Koniec", Top = 130, Left = 80, Width = 120 };

            btnStart.Click += BtnStart_Click;
            btnSettings.Click += BtnSettings_Click;
            btnExit.Click += (s, e) => Application.Exit();

            this.Controls.AddRange(new Control[] { btnStart, btnSettings, btnExit });
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            // Start z domyślnymi wartościami
            OknoGry game = new OknoGry(
                Ustawienia.Width,
                Ustawienia.Height,
                Ustawienia.Time,
                Ustawienia.Dydelfs,
                Ustawienia.Raccoons,
                Ustawienia.Crocs
            );
            game.Show();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            Form1 settings = new Form1();
            settings.Show();
        }
    }
}