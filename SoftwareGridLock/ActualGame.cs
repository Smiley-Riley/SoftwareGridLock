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
        public static int numOfColours = readFileLine(LevelSelect.levelFile, 2).Split(',').Length;
        PictureBox[,] gameBoard = new PictureBox[7, 7]; //Makes a 7x7 array that can store picture boxes
        Button[] colourSelect = new Button[10]; //Same but with buttons
        ee
        Color[] boardLayout = readFileLine(LevelSelect.levelFile, 2).Split(',').Select(name => Color.FromName(name)).ToArray();
        //Color[] colours = readFileLine(LevelSelect.levelFile, 2).Split(',').Select(name => Color.FromName(name)).ToArray();
        //Color[] colours = readFileLine(LevelSelect.levelFile, 2).Split(',').Select(name => Color.FromName(name)).ToArray();
        //Color[] colours = readFileLine(LevelSelect.levelFile, 2).Split(',').Select(name => Color.FromName(name)).ToArray();
        

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

            string[] colours = readFileLine(LevelSelect.levelFile, 2).Split(',');
            int startingColoursIndex = 0;
            for (int i = 0; i < numOfColours; i++)  //Colours all of the buttons that let you select the move colour
            {
                colourSelect[i].BackColor = boardLayout[startingColoursIndex];/*Color.FromName(colours[startingColoursIndex]);*/
                startingColoursIndex++;
            }

            for (int i = 9; i > numOfColours - 1; i--) //Hides all the excess white colour boxes
            {
                colourSelect[i].Hide();
            }
            

            string[] startingConfig = readFileLine(LevelSelect.levelFile, 1).Split(',');
            int startingConfigIndex = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++) //Sets the board up with whatever is in the saved csv file
                {
                    gameBoard[i, j].BackColor = Color.FromName(startingConfig[startingConfigIndex]);
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
            Color[] colours = readFileLine(LevelSelect.levelFile, 2).Split(',').Select(name => Color.FromName(name)).ToArray();
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
    }
}
