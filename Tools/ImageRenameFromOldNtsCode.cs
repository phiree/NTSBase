using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NModel;
using NBiz;
using System.IO;
using NLibrary;
namespace Tools
{
    public partial class ImageRenameFromOldNtsCode : Form
    {
        public ImageRenameFromOldNtsCode()
        {
            InitializeComponent();
        }

        BizProduct bizProduct = new BizProduct();
        private void button1_Click(object sender, EventArgs e)
        {   
            string msg;
            IList<Product> products = bizProduct.ReadListFromExcel(  new FileStream(tbxExcel.Text.Trim(),FileMode.Open), out msg);
            DirectoryInfo dirImages = new DirectoryInfo(tbxImage.Text.Trim());
            List<FileInfo> images= dirImages.GetImageFiles().ToList();
            foreach (FileInfo image in images)
            {
                foreach (Product p in products)
                {
                    if (p.NTSCode == Path.GetFileNameWithoutExtension(image.Name))
                    {
                        string suppierPath = Environment.CurrentDirectory + "\\整理好的图片\\" + p.SupplierName + "\\";
                       DirectoryInfo supplierDir= IOHelper.EnsureDirectory(suppierPath);

                       string newName = p.ModelNumber + image.Extension;
                       File.Copy(image.FullName, suppierPath + newName,true);

                    }
                }
            }
            MessageBox.Show("整理完毕");
            System.Diagnostics.Process.Start(Environment.CurrentDirectory+"\\整理好的图片\\");
        }
    }
}
