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
    public partial class FrmExtractImage : Form
    {
        public FrmExtractImage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
             
                System.IO.FileInfo file = new System.IO.FileInfo(openFileDialog1.FileName);
                string directory = file.Directory + "\\" +NLibrary.StringHelper.ReplaceInvalidChaInFileName(  System.IO.Path.GetFileNameWithoutExtension(file.Name) ,"$") + "\\";
                NBiz.ImageExtractor ie = new NBiz.ImageExtractor();
                ie.Excute(file.FullName, directory);
                MessageBox.Show("提取完成");
                System.Diagnostics.Process.Start(directory);
            }
        }
    }
}
