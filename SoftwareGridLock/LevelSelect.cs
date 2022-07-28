using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareGridLock
{
    public partial class LevelSelect : Form
    {
        public LevelSelect()
        {
            InitializeComponent();
        }

        private void btnLvl3_Click(object sender, EventArgs e)
        {

        }

        private void btnLvl1_Click(object sender, EventArgs e)
        {
            ActualGame actGame = new ActualGame();
            actGame.Show();
            this.Hide();
        }
    }
}
