﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SoftwareGridLock
{
    public partial class ActualGame : Form
    {
        PictureBox[,] gameBoard = new PictureBox[7, 7];

        private int time = 0;
        //private List<char> fileList = new List<char>();
        public ActualGame()
        {
            InitializeComponent();
        }

        private void ActualGame_Load(object sender, EventArgs e)
        {
            Timer.Start();
            int index = 1;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    gameBoard[i, j] = (PictureBox)Controls.Find("pictureBox" + (index).ToString(), true)[0];
                    index++;
                }
            }

            string[] colours = readFileLine(LevelSelect.levelFile, 1).Split(',');
            int startingColoursIndex = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    gameBoard[i, j].BackColor = Color.FromName(colours[startingColoursIndex]);
                    startingColoursIndex++;
                }
            }
        }

        private string readFileLine(string path, int line)
        {
            StreamReader reader = new StreamReader(path); //Opens the file 
            for (int i = 0; i < line - 1; i++)
            {
                string garbage = reader.ReadLine(); //This is horrible coding but it essentially skips lines until the one you want
            }
            return reader.ReadLine();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = "Time: " + ++time; //the ++ before time adds 1 to time before adding it to the label
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

    }
}
