using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NBiz;
using NModel;
using System.IO;
using NLibrary;
using NBiz;
namespace Tools
{
    
    public partial class FrmCheckFormat : Form
    {
        public FrmCheckFormat()
        {
           
            InitializeComponent();
        }
        ProductImportor bizFormatCheck = new NBiz.ProductImportor();
        private void btnCheck_Click(object sender, EventArgs e)
        {
            /*
               1 读取Excel表
             * 2 根据型号匹配图片
             * 3 导出结果:
             *   1) 完全匹配的
             *   2) 没有匹配的图片
             *   3) 没有匹配的Excel数据
             */
            //1

            bizFormatCheck.Check(tbxOriginal.Text, tbxOut.Text);
            if (MessageBox.Show("检测完成.即将打开结果文件夹.") == System.Windows.Forms.DialogResult.OK)
            {
                System.Diagnostics.Process.Start(tbxOut.Text);
            }
            
        }
      
        private void btnSelectOriginal_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbxOriginal.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnSelectOut_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbxOut.Text = folderBrowserDialog2.SelectedPath;
            }
        }

    }
}
