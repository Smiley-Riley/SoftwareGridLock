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
        

        private int time = 0;
        //private List<char> fileList = new List<char>();
        public ActualGame()
        {
            InitializeComponent();
        }

        private void ActualGame_Load(object sender, EventArgs e)
        {
            Timer.Start();
            readFile();
            //pictureBox1.BackColor = Color.Red;

        }

        private void readFile()
        {

            StreamReader reader = new StreamReader(@"board1.csv"); //Opens the file
            List<string> lines = new List<string>(); //creates a list of lines
            string line;                            //to store seperate lines
            while ((line = reader.ReadLine()) != null) { //reads each line until there is nothing in the new line
                lines.Add(line); 
            }

            Grid grid = Grid.LoadGame(lines); 

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = "Time: " + ++time; //the ++ before time adds 1 to time before adding it to the label
        }
    }

    class Grid
    {
        public static Grid LoadGame(List<String> lines)
        {
            GreenBlock green = GreenBlock.FromString(lines.First());
            //MessageBox.Show("stub");
            List<Block> blocks = Block.loadBlocks(lines.Skip(1));
            return new Grid(green, blocks);
        }

        private GreenBlock green;
        private List<Block> blocks;

        public Grid(GreenBlock green, List<Block> blocks)
        {
            this.green = green;
            this.blocks = blocks;
        }
    }
    class Block
    {
        public static List<Block> loadBlocks(IEnumerable<String> blockRecords)
        {
            List<Block> blocks = new List<Block>();
            foreach (string blockRecord in blockRecords) //same as i = 0; i < .length... repeats over list
            {
                blocks.Add(Block.fromString(blockRecord)); //adds Block objects to "blocks" list 
            }

            return blocks;
        }

        public static Block fromString(string blockRecord)
        {
            string[] fields = blockRecord.Split(','); //creates an array of 'substrings', distinguished by the commas. so the string "red,hori,3" turns into array {red, hori, 3}
            string colour = fields[0]; //Sets the different aspects of blocks to be accessible
            string orientation = fields[1];
            int length = Convert.ToInt32(fields[2]);
            int x = Convert.ToInt32(fields[3]);
            int y = Convert.ToInt32(fields[4]);
            return new Block(colour, orientation, length, x, y);
        }

        private string colour;
        private string orientation;
        private int length;
        public static int x;
        public static int y;

        public Block(string colour, string orientation, int length, int x, int y)
        {
            this.colour = colour;
            this.orientation = orientation;
            this.length = length;
            Block.x = x;
            Block.y = y;
        }
    }

    class GreenBlock : Block
    {
        public static GreenBlock FromString(String record)
        {
            return new GreenBlock(x, y); //Relates to the Block.x and Block.y
        }

        public GreenBlock(int x, int y) : base("green", "-", 0, x, y)
        { }
    }
}
