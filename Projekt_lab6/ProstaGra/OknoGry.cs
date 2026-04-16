using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProstaGra
{
    public partial class OknoGry : Form
    {
        int width, height, timeLeft;
        string[,] board;
        Button[,] buttons;
        Timer timer = new Timer();

        public OknoGry(int width, int height, int time, int dydelfs, int raccoons, int crocs)
        {
            InitializeComponent();

            width = w;
            height = h;
            timeLeft = time;

            board = new string[width, height];
            buttons = new Button[width, height];

            GenerateBoard(dydelfs, raccoons, crocs);
            CreateButtons();

            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        
    }
}