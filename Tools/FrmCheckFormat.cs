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
        FormatCheck bizFormatCheck = new NBiz.FormatCheck();
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
        private void CheckSingleFolder(string folderPath)
        {
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            FileInfo[] excelFiles = dir.GetFiles("*.xls");
            if (excelFiles.Length != 1)
            {
                throw new Exception("错误,文件夹 "+folderPath+" 内有多个Excel文件,应该有且仅有一个excel文件");
            }
            FileInfo excelFile = excelFiles[0];
            DirectoryInfo[] dirs = dir.GetDirectories();
            if (dirs.Length !=1)
            {
                throw new Exception("错误,文件夹 " + folderPath + " 内有多个图片文件,应该有且仅有一个图片文件夹");
            }
            DirectoryInfo dirImage = dirs[0];
            Stream stream = new FileStream(excelFile.FullName,FileMode.Open);
            
            IExcelReader<Product> productReader=new ProductExcelReader();
            IList<Product> products = productReader.Read(stream);
            FileInfo[] images= dirImage.GetFiles();

            IList<Product> productsHasPicture = new List<Product>();
            IList<Product> productsNotHasPicture = new List<Product>();

            List<FileInfo> imagesHasProduct = new  List<FileInfo>();
            List<FileInfo> imagesHasNotProduct = new List<FileInfo>();
            
            //写一个通用类,比较两个序列,返回匹配结果.
            //Compare<T1,T2>  T1和T2需要实现他们两者比较的接口
            foreach (Product p in products)
            {
                foreach (FileInfo image in images)
                {
                    if (Path.GetFileNameWithoutExtension(image.Name)
                        .Equals(StringHelper.ReplaceSpace(p.ModelNumber), StringComparison.OrdinalIgnoreCase))
                    {
                        productsHasPicture.Add(p);
                        imagesHasProduct.Add(image);
                        break;
                    }
                }
                productsHasPicture.Add(p);
            }
            foreach (FileInfo f in images)
            {
                foreach (FileInfo f2 in imagesHasProduct)
                {
                    if (f.Name.Equals(f2.Name))
                    {
                        break;
                    }
                }
                imagesHasNotProduct.Add(f);
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
