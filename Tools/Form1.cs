using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new FrmExtractImage().Show();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            
                new FrmCheckFormat().Show();
           
        }

        private void btnImageRename_Click(object sender, EventArgs e)
        {
            new RenameImage().Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            new ImageRenameFromOldNtsCode().Show();
        }
    }
}
