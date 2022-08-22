using System;
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
        Color[] startingConfig = readFileLine(LevelSelect.levelFile, 1).Split(',').Select(colour => Color.FromName(colour)).ToArray();
        Color[] colours = readFileLine(LevelSelect.levelFile, 2).Split(',').Select(colour => Color.FromName(colour)).ToArray();
        Color[] horizontalMove = readFileLine(LevelSelect.levelFile, 3).Split(',').Select(colour => Color.FromName(colour)).ToArray();
        Color[] verticalMove = readFileLine(LevelSelect.levelFile, 4).Split(',').Select(colour => Color.FromName(colour)).ToArray();
        int[] endingPos = readFileLine(LevelSelect.levelFile, 5).Split(',').Select(posCoord => Convert.ToInt32(posCoord)).ToArray();
        int[] finishLinesSelected = readFileLine(LevelSelect.levelFile, 6).Split(',').Select(posCoord => Convert.ToInt32(posCoord)).ToArray();

        bool gameWon = false;

        List<int> endPosArrY = new List<int>();
        List<int> endPosArrX = new List<int>();

        PictureBox[,] gameBoard = new PictureBox[7, 7]; //Makes a 7x7 array that can store picture boxes
        Button[] colourSelect = new Button[11]; //Same but with buttons

        int time = 0;
        DateTime start = DateTime.Now;

        private static string readFileLine(string path, int line)
        {
            StreamReader reader = new StreamReader(path); //Opens the file 
            for (int i = 0; i < line - 1; i++)
            {
                reader.ReadLine(); //skips lines until the one you want
            }
            return reader.ReadLine();
        }
        public ActualGame() { InitializeComponent(); }

        private void ActualGame_Load(object sender, EventArgs e)
        {
            pictureBoxSelectedColour.BackColor = colours[0];
            lblWinText.Hide();
            Timer.Start();
            for (int i = 1; i < 9; i++)
            {
                if (i != finishLinesSelected[0] && i != finishLinesSelected[1]) //If the finishLines aren't the ones specificed in the file they are hidden
                {
                    ((PictureBox)Controls.Find("finishLine" + (i).ToString(), true)[0]).Hide();
                }
            }   

            for (int i = 0; i < endingPos.Length; i += 2)
            {
                endPosArrY.Add(endingPos[i]);
                endPosArrX.Add(endingPos[i + 1]);
            }
            
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
            for (int i = 0; i < colours.Length; i++)  //Colours all of the buttons that let you select the move colour
            {
                colourSelect[i].BackColor = colours[startingColoursIndex];/*Color.FromName(colours[startingColoursIndex]);*/
                startingColoursIndex++;
            }

            for (int i = 10; i > colours.Length - 1; i--) //Hides all the excess white colour boxes
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = "Time: " + ++time; //the ++ Before time adds 1 to time and then adds the increased time to the label
        }

        private void moveCommand(string moveDirection)
        {
            if (gameWon == false)
            {
                Color selectedColour = pictureBoxSelectedColour.BackColor;
                bool canMoveHorizontal = true;
                bool canMoveVertical = true;
                for (int i = 0; i < horizontalMove.Length; i++)
                {
                    if (horizontalMove[i] == selectedColour) //if the block is in the horizontal move array you know that it can only move horizontal and can't move vertical
                    {
                        canMoveVertical = false;
                        break;
                    }
                }
                for (int i = 0; i < verticalMove.Length; i++)
                {
                    if (verticalMove[i] == selectedColour) //same but with vertical - horizontal
                    {
                        canMoveHorizontal = false;
                        break;
                    } //The reason that it's backwards like this is that the canMoves need to start true, so that the Green can move any direction if it's not in any of the catagories
                }
                bool canMove = true;
                List<int> arrayX = new List<int>(); //Two lists that store the x and y positions of any blocks of
                List<int> arrayY = new List<int>(); //the colour going to be moved while they're being checked.

                if (canMoveHorizontal)
                {
                    if (moveDirection == "right")
                    {
                        for (int y = 0; y < 7; y++)
                        {
                            for (int x = 0; x < 7; x++)
                            {
                                if (selectedColour == gameBoard[y, x].BackColor) //Checks if that tile is the one you're looking to move
                                {
                                    arrayX.Add(x);
                                    arrayY.Add(y);
                                    if (x == 6)
                                    {
                                        canMove = false; //If it's on the most right level it can't move right
                                    }
                                    else if (x < 6) //if y = 6, the y + 1 is out of the index
                                    {
                                        if (gameBoard[y, x + 1].BackColor != gameBoard[y, x].BackColor && gameBoard[y, x + 1].BackColor != Color.White)
                                        {
                                            canMove = false; //If the tile to the right of each of the tiles in the car are not White or not the colour of the tile
                                        }
                                    }
                                }
                            }
                        }


                    }
                    else if (moveDirection == "left")
                    {
                        for (int y = 0; y < 7; y++)
                        {
                            for (int x = 0; x < 7; x++)
                            {
                                if (selectedColour == gameBoard[y, x].BackColor)
                                {
                                    arrayX.Add(x);
                                    arrayY.Add(y);
                                    if (x == 0)
                                    {
                                        canMove = false;
                                    }
                                    else if (x > 0)
                                    {
                                        if (gameBoard[y, x - 1].BackColor != gameBoard[y, x].BackColor && gameBoard[y, x - 1].BackColor != Color.White)
                                        {
                                            canMove = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (canMoveVertical)
                {
                    if (moveDirection == "up")
                    {
                        for (int y = 0; y < 7; y++)
                        {
                            for (int x = 0; x < 7; x++)
                            {
                                if (selectedColour == gameBoard[y, x].BackColor)
                                {
                                    arrayX.Add(x);
                                    arrayY.Add(y);
                                    if (y == 0)
                                    {
                                        canMove = false;
                                    }
                                    else if (y > 0)
                                    {
                                        if (gameBoard[y - 1, x].BackColor != gameBoard[y, x].BackColor && gameBoard[y - 1, x].BackColor != Color.White)
                                        {
                                            canMove = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (moveDirection == "down") 
                    {
                        for (int y = 0; y < 7; y++)
                        {
                            for (int x = 0; x < 7; x++)
                            {
                                if (selectedColour == gameBoard[y, x].BackColor)
                                {
                                    arrayX.Add(x);
                                    arrayY.Add(y);
                                    if (y == 6)
                                    {
                                        canMove = false;
                                    }
                                    else if (y < 6)
                                    {
                                        if (gameBoard[y + 1, x].BackColor != gameBoard[y, x].BackColor && gameBoard[y + 1, x].BackColor != Color.White)
                                        {
                                            canMove = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (canMove) //I could put this in the seperate check if can move sections, but it's nicer having them seperated
                {
                    if (moveDirection == "left")
                    {
                        for (int i = 0; i < arrayX.Count; i++)
                        {
                            gameBoard[arrayY[i], arrayX[i]].BackColor = Color.White;
                            gameBoard[arrayY[i], arrayX[i] - 1].BackColor = selectedColour;
                        }
                    }
                    else if (moveDirection == "up")
                    {
                        for (int i = 0; i < arrayX.Count; i++)
                        {
                            gameBoard[arrayY[i], arrayX[i]].BackColor = Color.White;
                            gameBoard[arrayY[i] - 1, arrayX[i]].BackColor = selectedColour;
                        }
                    }
                    else if (moveDirection == "right")
                    {
                        for (int i = arrayX.Count - 1; i >= 0; i--) //The reason the for loops are different is that if it goes top left - bot right while moving right
                        {                                           //the later updates will undo the previous updates and you will end up with only one block left
                            gameBoard[arrayY[i], arrayX[i]].BackColor = Color.White;
                            gameBoard[arrayY[i], arrayX[i] + 1].BackColor = selectedColour;
                        }
                    }
                    else if (moveDirection == "down")
                    {
                        /*for (int y = 6; y >=0 ; y--)
                        {
                            for (int x = 6; x >= 0; x--)
                            {
                                if (selectedColour == gameBoard[y, x].BackColor)  //Im stuck between whether this is better than having the two lists that store the positions
                                {
                                    gameBoard[y, x].BackColor = Color.White;
                                    gameBoard[y + 1, x].BackColor = selectedColour;
                                }
                            }
                        }*/
                        
                        for (int i = arrayX.Count - 1; i >= 0; i--)
                        {
                            gameBoard[arrayY[i], arrayX[i]].BackColor = Color.White;
                            gameBoard[arrayY[i] + 1, arrayX[i]].BackColor = selectedColour;
                        }
                    }


                    /*if (moveDirection == "up" || moveDirection == "left") //This difference between up/left and right/down is so that all the blocks show in the end
                    {
                        for (int i = 0; i < arrayX.Count; i++) //rather than there being all white and one block left behind
                        {
                            if (moveDirection == "left") //Like if the board checking goes left - right/up - down anything going the other way will get cut off
                            {
                                gameBoard[arrayX[i], arrayY[i]].BackColor = Color.White;
                                gameBoard[arrayX[i], arrayY[i] - 1].BackColor = selectedColour;
                            }
                            if (moveDirection == "up")
                            {
                                gameBoard[arrayX[i], arrayY[i]].BackColor = Color.White;
                                gameBoard[arrayX[i] - 1, arrayY[i]].BackColor = selectedColour;
                            }
                        }
                    }
                    if (moveDirection == "right" || moveDirection == "down") 
                    {
                        for (int i = arrayX.Count - 1; i >= 0; i--) 
                        {
                            if (moveDirection == "right")
                            {
                                gameBoard[arrayX[i], arrayY[i]].BackColor = Color.White;
                                gameBoard[arrayX[i], arrayY[i] + 1].BackColor = selectedColour;
                            }
                            if (moveDirection == "down")
                            {
                                gameBoard[arrayX[i], arrayY[i]].BackColor = Color.White;
                                gameBoard[arrayX[i] + 1, arrayY[i]].BackColor = selectedColour;
                            }
                        }
                    }
                }*/
                    bool didYouWin = true;
                    for (int i = 0; i < endingPos.Length / 2; i++) //Checks that all the coordinates of where the winning tiles are (found in csv file) are green 
                    {
                        if (gameBoard[endPosArrY[i], endPosArrX[i]].BackColor != Color.Green)
                        {
                            didYouWin = false;
                        }
                    }
                    if (didYouWin)
                    {
                        gameWon = true;
                        lblWinText.Show();
                        Timer.Stop();
                        DateTime end = DateTime.Now;
                        TimeSpan ts = end - start; //Compares the exact time of the start of the program and the win time
                        /*string str = Convert.ToString(ts.TotalMilliseconds);
                        str = str.Split('.')[0]; //This is what I first wrote and felt really smart about but also Convert.ToInt32() is just the better option
                        int time = Convert.ToInt32(str);*/
                        int time = Convert.ToInt32(ts.TotalMilliseconds);
                        int numOfMinutes = 0;
                        while (time > 60000) //Subtracts minutes at a time until there is less than a minute of time left
                        {
                            time -= 60000;
                            numOfMinutes++;
                        }
                        int numOfSeconds = 0;
                        while (time > 1000) //Subtracts seconds until less than a second
                        {
                            time -= 1000;
                            numOfSeconds++;
                        }
                        if (numOfMinutes > 0)
                        {
                            MessageBox.Show(Convert.ToString(numOfMinutes) + " minutes " + Convert.ToString(numOfSeconds) + " seconds " + Convert.ToString(time) + " milliseconds");
                        }
                        else if (numOfSeconds > 0)
                        {
                            MessageBox.Show(Convert.ToString(numOfSeconds) + " seconds " + Convert.ToString(time) + " milliseconds");
                        }
                        else
                        {
                            MessageBox.Show(Convert.ToString(time) + " milliseconds");
                        }
                    }
                }
            }
        }
        private void selectCar(int button) { pictureBoxSelectedColour.BackColor = colours[button - 1];}
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

        private void btnUp_Click(object sender, EventArgs e) { moveCommand("up"); } 
        private void btnRight_Click(object sender, EventArgs e) { moveCommand("right"); } 
        private void btnDown_Click(object sender, EventArgs e) { moveCommand("down"); } 
        private void btnLeft_Click(object sender, EventArgs e) { moveCommand("left"); }

        private void btnLevelSelect_Click(object sender, EventArgs e)
        {
            LevelSelect lvlSelect = new LevelSelect();
            lvlSelect.Show();
            this.Close();
        }

        private void btnResetLvl_Click(object sender, EventArgs e)
        {
            ActualGame actualGame = new ActualGame();
            actualGame.Show();
            this.Close();
        }
    }
}