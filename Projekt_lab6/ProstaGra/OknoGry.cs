using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProstaGra
{
    public class OknoGry : Form
    {
        int width, height, timeLeft;
        string[,] board;
        Button[,] buttons;
        System.Windows.Forms.Timer timer;
        Button activeCrocButton = null;
        bool crocActive = false;

        public OknoGry(int width, int height, int time, int dydelfs, int raccoons, int crocs)
        {
            this.width = width;
            this.height = height;
            this.timeLeft = time;

            this.Width = width * 50 + 20;
            this.Height = height * 50 + 40;

            board = new string[width, height];
            buttons = new Button[width, height];

            GenerateBoard(dydelfs, raccoons, crocs);
            CreateButtons();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void GenerateBoard(int dydelfs, int raccoons, int crocs)
        {
            Random rand = new Random();

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    board[i, j] = "empty";

            PlaceRandom("dydelf", dydelfs, rand);
            PlaceRandom("raccoon", raccoons, rand);
            PlaceRandom("croc", crocs, rand);
        }

        private void PlaceRandom(string type, int count, Random rand)
        {
            while (count > 0)
            {
                int x = rand.Next(width);
                int y = rand.Next(height);

                if (board[x, y] == "empty")
                {
                    board[x, y] = type;
                    count--;
                }
            }
        }

        private void CreateButtons()
        {
            int size = 50;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Button btn = new Button();
                    btn.Width = size;
                    btn.Height = size;
                    btn.Left = i * size;
                    btn.Top = j * size;
                    btn.Tag = new Point(i, j);

                    btn.Click += Button_Click;

                    buttons[i, j] = btn;
                    this.Controls.Add(btn);
                }
            }
        }

       private async void Button_Click(object sender, EventArgs e)
       {  
          Button btn = sender as Button;
          if (btn == null) return;

          Point p = (Point)btn.Tag;
          string value = board[p.X, p.Y];

    
          if (crocActive && btn == activeCrocButton)
         {
            btn.Text = "";
            btn.Enabled = true;

            crocActive = false;
            activeCrocButton = null;

           return;
         }
          
          if(value != "croc")
          btn.Enabled = false;

          if (value == "dydelf")
         {
           btn.Text = "D";
         }
          else if (value == "raccoon")
         {
           btn.Text = "R";

           await System.Threading.Tasks.Task.Delay(2000);
           CloseNeighbors(p.X, p.Y);
         }
          else if (value == "croc")
         {
           btn.Text = "C";

           crocActive = true;
           activeCrocButton = btn;

           await System.Threading.Tasks.Task.Delay(2000);

        
           if (crocActive)
           {
              MessageBox.Show("Przegrałeś! Krokodyl!");
              this.Close();
           }
          }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            this.Text = "Czas: " + timeLeft;

            if (timeLeft <= 0)
            {
                timer.Stop();
                MessageBox.Show("Koniec czasu!");
                this.Close();
            }
        }

        private void CloseNeighbors(int x, int y)
       {
          for (int i = -1; i <= 1; i++)
          {
            for (int j = -1; j <= 1; j++)
            {
               int nx = x + i;
               int ny = y + j;

               if (nx >= 0 && nx < width && ny >= 0 && ny < height)
               {
                  Button neighbor = buttons[nx, ny];

                  neighbor.Text = "";
                  neighbor.Enabled = true; // można kliknąć ponownie
               }
            }
          }
        }
    }
}