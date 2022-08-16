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
        Button[] colourSelect = new Button[11]; //Same but with buttons

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
            for (int i = 0; i < 11; i++) //Same as picture boxes but buttons
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

            for (int i = 10; i > numOfColours - 1; i--) //Hides all the excess white colour boxes
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

        private void moveCommand(string moveDirection)
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
            bool canMove = true;
            List<int> arrayX = new List<int>(); //Two arrays that store the x and y positions of any blocks of the colour being moved while they're being checked
            int arrayXCounter = 0;
            List<int> arrayY = new List<int>(); 
            int arrayYCounter = 0;

            if (canMoveVertical)
            {
                if (moveDirection == "up")
                {
                    for (int x = 0; x < 7; x++)
                    {
                        for (int y = 0; y < 7; y++)
                        {
                            if(pictureBoxSelectedColour.BackColor == gameBoard[x,y].BackColor) //Checks if that tile is the one you're looking to move
                            {
                                arrayX.Add(x);
                                arrayXCounter++;
                                arrayY.Add(y);
                                arrayYCounter++;
                                if (y < 6) //if y = 6, the y + 1 is out of the index
                                {
                                    if (gameBoard[x, y + 1].BackColor != gameBoard[x,y].BackColor && gameBoard[x, y + 1].BackColor != Color.White)
                                    {
                                        canMove = false; //If the tile above each of the tiles in the car are not White or not the colour of the tile
                                    }
                                }

                                if (y == 6)
                                {
                                    canMove = false; //If it's on the top level it can't move up
                                }
                            }
                        }
                    }


                }
                if (moveDirection == "down")
                {
                    for (int x = 0; x < 7; x++)
                    {
                        for (int y = 0; y < 7; y++)
                        {
                            if (pictureBoxSelectedColour.BackColor == gameBoard[x, y].BackColor) //Checks if that tile is the one you're looking to move
                            {
                                arrayX.Add(x);
                                arrayXCounter++;
                                arrayY.Add(y);
                                arrayYCounter++;
                                if (y > 0) 
                                {
                                    if (gameBoard[x, y - 1].BackColor != gameBoard[x, y].BackColor && gameBoard[x, y - 1].BackColor != Color.White)
                                    {
                                        canMove = false; 
                                    }
                                }

                                if (y == 0)
                                {
                                    canMove = false;
                                }
                            }
                        }
                    }
                }
            }
            if (canMoveHorizontal)
            {
                if (moveDirection == "left")
                {
                    for (int x = 0; x < 7; x++)
                    {
                        for (int y = 0; y < 7; y++)
                        {
                            if (pictureBoxSelectedColour.BackColor == gameBoard[x, y].BackColor) //Checks if that tile is the one you're looking to move
                            {
                                arrayX.Add(x);
                                arrayXCounter++;
                                arrayY.Add(y);
                                arrayYCounter++;
                                if (x > 0) 
                                {
                                    if (gameBoard[x - 1, y].BackColor != gameBoard[x, y].BackColor && gameBoard[x - 1, y].BackColor != Color.White)
                                    {
                                        canMove = false; 
                                    }
                                }

                                if (x == 0)
                                {
                                    canMove = false; //If it's on the top level it can't move up
                                }
                            }
                        }
                    }
                }
                if (moveDirection == "right")
                {
                    for (int x = 0; x < 7; x++)
                    {
                        for (int y = 0; y < 7; y++)
                        {
                            if (pictureBoxSelectedColour.BackColor == gameBoard[x, y].BackColor) //Checks if that tile is the one you're looking to move
                            {
                                arrayX.Add(x);
                                arrayXCounter++;
                                arrayY.Add(y);
                                arrayYCounter++;
                                if (x < 6)
                                {
                                    if (gameBoard[x + 1, y].BackColor != gameBoard[x, y].BackColor && gameBoard[x + 1, y].BackColor != Color.White)
                                    {
                                        canMove = false; //If the tile above each of the tiles in the car are not White or not the colour of the tile
                                    }
                                }

                                if (x == 6)
                                {
                                    canMove = false; //If it's on the top level it can't move up
                                }
                            }
                        }
                    }
                }
            }
            //I could experiment with having the for loop on the outside and the direction check on the inside? idk.
            if (canMove) {
                if (moveDirection == "left" || moveDirection == "down") //again up and left. this difference is so that all the blocks show in the end
                {
                    for (int i = 0; i < arrayX.Count; i++) //rather than there being all white and one block left behind
                    {
                        if (moveDirection == "down") //Like if the board checking goes left - right up - down anything going the other way will get cut off
                        {
                            gameBoard[arrayX[i], arrayY[i]].BackColor = Color.White;
                            gameBoard[arrayX[i], arrayY[i] - 1].BackColor = pictureBoxSelectedColour.BackColor;
                        }
                        
                        if (moveDirection == "left")
                        {
                            gameBoard[arrayX[i], arrayY[i]].BackColor = Color.White;
                            gameBoard[arrayX[i] - 1, arrayY[i]].BackColor = pictureBoxSelectedColour.BackColor;
                        }
                    }
                }
                if (moveDirection == "up" || moveDirection == "right") //right and down?
                {
                    for (int i = arrayX.Count - 1; i >= 0; i--) {
                        if (moveDirection == "up")
                        {
                            gameBoard[arrayX[i], arrayY[i]].BackColor = Color.White;
                            gameBoard[arrayX[i], arrayY[i] + 1].BackColor = pictureBoxSelectedColour.BackColor;
                        }

                        if (moveDirection == "right")
                        {
                            gameBoard[arrayX[i], arrayY[i]].BackColor = Color.White;
                            gameBoard[arrayX[i] + 1, arrayY[i]].BackColor = pictureBoxSelectedColour.BackColor;
                        }
                    }
                }
            }
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
        private void button11_Click(object sender, EventArgs e) { selectCar(11); }

        private void btnUp_Click(object sender, EventArgs e) { moveCommand("left"); } //I actually don't know what's going on here
        private void btnRight_Click(object sender, EventArgs e) { moveCommand("up"); } //Like genuinely it only works if I switch these around
        private void btnDown_Click(object sender, EventArgs e) { moveCommand("right"); } //Uh I'm not even gonna bother it works and I'm not gonna touch
        private void btnLeft_Click(object sender, EventArgs e) { moveCommand("down"); } //it until I need to.

    }
}
