using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NModel;
using NBiz;
namespace NTest.NBizTest
{
    [TestFixture]
    public class ImportFromExcelTest
    {
        [Test]
        public void ReadProductFromExcelTest()
        {
            string filePath = Environment.CurrentDirectory + @"\TestFiles\NTS 产品报价单   哈慈 20130306.xls";
            ProductExcelReader importer = new ProductExcelReader();
            IList<Product> products = importer.Read(new System.IO.FileStream(filePath, System.IO.FileMode.Open));

            Assert.AreEqual(19,products.Count);

        }
        [Test]
        public void ReadSupplierFromExcelTest()
        {

            string filePathSupplier = Environment.CurrentDirectory + @"\TestFiles\供应商104.xls";
            SupplierExcelReader importerSupplier = new SupplierExcelReader();
            IList<Supplier> Supplier = importerSupplier.Read(new System.IO.FileStream(filePathSupplier, System.IO.FileMode.Open));

            Assert.AreEqual(104, Supplier.Count);
        }

        /// <summary>
        /// 提取excel里的图片
        /// </summary>
        [Test]
        public void ReadProductWithImageFromExcelTest()
        {

            string filePath = Environment.CurrentDirectory + @"\TestFiles\联丰报价单 20130108 --餐厨部2.xls";
            string savePath = @"d:\saveimages\";
            ImageExtractor ie = new ImageExtractor();
            ie.Excute(filePath, savePath);
            Assert.AreEqual(3, System.IO.Directory.GetFiles(savePath).Length);
           // Assert.AreEqual(19, products.Count);
        }
        //导入Erp格式
        [Test]
        public void ReadProductFromErpExcelTest()
        {
            string filePath = Environment.CurrentDirectory + @"\TestFiles\吧台设备及用具.XLS";
            ProductExcelReader importer = new ProductExcelReader();
            IList<Product> products = importer.Read(new System.IO.FileStream(filePath, System.IO.FileMode.Open));

            Assert.AreEqual(40, products.Count);
            Assert.AreEqual("01.001", products[0].CategoryCode);

        }
        //导入分类列表
        [Test]
        public void ReadCategoryFromExcel()
        {
            string filePath = Environment.CurrentDirectory + @"\TestFiles\分类表.xls";
            CategoryExcelReader importer = new CategoryExcelReader();
            IList<Category> products = importer.Read(new System.IO.FileStream(filePath, System.IO.FileMode.Open));

            Assert.AreEqual(465, products.Count);
           
        }
        //英文ERP格式问题
        [Test]
        public void ReadProductErpEnglishFromExcel()
        {
            string filePath = Environment.CurrentDirectory + @"\TestFiles\英文——2013-3-26家具（brighthome）数据表.XLS";
            ProductExcelReader importer = new ProductExcelReader();
            IList<Product> products = importer.Read(new System.IO.FileStream(filePath, System.IO.FileMode.Open));

            Assert.AreEqual(153, products.Count);
            //Assert.AreEqual("01.001", products[0].CategoryCode);

        }
    }
}
