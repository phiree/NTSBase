using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NLibrary;
namespace Tools
{
    public partial class RenameImage : Form
    {
        public RenameImage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;

            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string organized = @"E:\workspace\code\resources\导入资料\datafiles\已整理好图片\";
            DirectoryInfo folder = new DirectoryInfo(textBox1.Text);
            foreach (DirectoryInfo dir1 in folder.GetDirectories())
            {
                //供应商

                foreach (DirectoryInfo dir in dir1.GetDirectories())
                {
                    //子文件夹
                    foreach (FileInfo fi in dir.GetFiles("*.jpg"))
                    {
                        //图片文件
                        string ntsCodePatern = @"\d{2}\.\d{3}\.\d{10}.*";
                        string supplierFolder = organized + dir1.Name+"\\";
                        NLibrary.IOHelper.EnsureFileDirectory(supplierFolder);
                        if (System.Text.RegularExpressions.Regex.IsMatch(fi.Name, ntsCodePatern))
                        {
                            fi.CopyTo(supplierFolder + "\\" + dir.Name  + fi.Extension, true);
                        }
                        else
                        {
                            fi.CopyTo(supplierFolder + "\\" + fi.Name, true);
                        }
                    }
                }
            }
            MessageBox.Show("done");
        }
    }
}
