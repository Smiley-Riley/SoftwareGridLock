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
        
        
        public static Color[] startingConfig = readFileLine(LevelSelect.levelFile, 1).Split(',').Select(name => Color.FromName(name)).ToArray();
        public static Color[] colours = readFileLine(LevelSelect.levelFile, 2).Split(',').Select(name => Color.FromName(name)).ToArray();
        public static Color[] horizontalMove = readFileLine(LevelSelect.levelFile, 3).Split(',').Select(name => Color.FromName(name)).ToArray();
        public static Color[] verticalMove = readFileLine(LevelSelect.levelFile, 4).Split(',').Select(name => Color.FromName(name)).ToArray();

        public static int numOfColours = colours.Length;
        PictureBox[,] gameBoard = new PictureBox[7, 7]; //Makes a 7x7 array that can store picture boxes
        Button[] colourSelect = new Button[10]; //Same but with buttons

        private int time = 0;

        public ActualGame()
        {
            InitializeComponent();
        }



        private void ActualGame_Load(object sender, EventArgs e)
        {
            Timer.Start();
            int index = 1;
            for (int i = 0; i < 7; i++) //Adds picture boxes to array by searching through their names (pictureBox1, pictureBox2, etc)
            {
                for (int j = 0; j < 7; j++)
                {
                    gameBoard[i, j] = (PictureBox)Controls.Find("pictureBox" + (index).ToString(), true)[0];
                    index++;
                }
            }

            index = 1;
            for (int i = 0; i < 10; i++) //Same as picture boxes but buttons
            {
                colourSelect[i] = (Button)Controls.Find("button" + (index).ToString(), true)[0];
                index++;
            }

            int startingColoursIndex = 0;
            for (int i = 0; i < numOfColours; i++)  //Colours all of the buttons that let you select the move colour
            {
                colourSelect[i].BackColor = colours[startingColoursIndex];/*Color.FromName(colours[startingColoursIndex]);*/
                startingColoursIndex++;
            }

            for (int i = 9; i > numOfColours - 1; i--) //Hides all the excess white colour boxes
            {
                colourSelect[i].Hide();
            }
            
  
            int startingConfigIndex = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++) //Sets the board up with whatever is in the saved csv file
                {
                    gameBoard[i, j].BackColor = startingConfig[startingConfigIndex];
                    startingConfigIndex++;
                }
            }
        }

        private static string readFileLine(string path, int line)
        {
            StreamReader reader = new StreamReader(path); //Opens the file 
            for (int i = 0; i < line - 1; i++)
            {
                string garbage = reader.ReadLine(); //This feels like bad coding but it essentially skips lines until the one you want
            }
            return reader.ReadLine();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = "Time: " + ++time; //the ++ before time adds 1 to time before adding it to the label
        }

        private void selectCar(int button)
        {
            /*string temp = Convert.ToString(colourSelect[button - 1].BackColor);*/
            //string[] colours = readFileLine(LevelSelect.levelFile, 2).Split(',');
            /*temp = temp.Remove(0, 7);
            temp = temp.Remove(temp.Length - 1);                         //This is just overcomplicating things
            MessageBox.Show(temp);
            pictureBoxSelectedColour.BackColor = Color.FromName(temp);*/
            //pictureBoxSelectedColour.BackColor = Color.FromName(colours[button - 1]);
          //Color[] colours = readFileLine(LevelSelect.levelFile, 2).Split(',').Select(name => Color.FromName(name)).ToArray();
            pictureBoxSelectedColour.BackColor = colours[button - 1];
        } 
        private void button1_Click(object sender, EventArgs e) { selectCar(1); }
        private void button2_Click(object sender, EventArgs e) { selectCar(2); }
        private void button3_Click(object sender, EventArgs e) { selectCar(3); }
        private void button4_Click(object sender, EventArgs e) { selectCar(4); }
        private void button5_Click(object sender, EventArgs e) { selectCar(5); }
        private void button6_Click(object sender, EventArgs e) { selectCar(6); }
        private void button7_Click(object sender, EventArgs e) { selectCar(7); }
        private void button8_Click(object sender, EventArgs e) { selectCar(8); }
        private void button9_Click(object sender, EventArgs e) { selectCar(9); }
        private void button10_Click(object sender, EventArgs e) { selectCar(10); }

        private void btnUp_Click(object sender, EventArgs e)
        {
            
        }

        private void btnRight_Click(object sender, EventArgs e)
        {

        }

        private void btnDown_Click(object sender, EventArgs e)
        {

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {

        }

        private void moveCommand()
        {
            Color selectedColour = pictureBoxSelectedColour.BackColor;
            bool canMoveHorizontal = true;
            bool canMoveVertical = true;
            for (int i = 0; i < horizontalMove.Length; i++)
            {
                if (horizontalMove[i] == selectedColour)
                {
                    canMoveHorizontal = false;
                } 
              
            }
            for (int i = 0; i < verticalMove.Length; i++)
            {
                if (verticalMove[i] == selectedColour)
                {
                    canMoveVertical = false;
                }
            }


        }
    }
}
