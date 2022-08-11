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
            string[] fields = readFile(@"board1NoClass.csv").Split(',');

            //pictureBox1.BackColor = Color.Red;

        }

        private string readFile(string path)
        {

            StreamReader reader = new StreamReader(path); //Opens the file 
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
