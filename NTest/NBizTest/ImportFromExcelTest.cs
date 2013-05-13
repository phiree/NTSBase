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
        BizProduct bizProduct = new BizProduct();
        BizSupplier bizSupplier = new BizSupplier();
        BizCategory bizCategory = new BizCategory();
        string errMsg;
        [Test]
        public void ReadProductFromExcelTest()
        {
            /*
             文件末尾有空白行
             */
            string filePath = Environment.CurrentDirectory + @"\TestFiles\NTS 产品报价单   哈慈 20130306.xls";
        
            IList<Product> products = bizProduct.ReadListFromExcel(new System.IO.FileStream(filePath, System.IO.FileMode.Open)
                ,out errMsg);

            Assert.AreEqual(19,products.Count);

        }
        [Test]
        public void ReadSupplierFromExcelTest()
        {

            string filePathSupplier = Environment.CurrentDirectory + @"\TestFiles\供应商104.xls";
       
            IList<Supplier> Supplier = bizSupplier.ReadSupplierListFromExcel(new System.IO.FileStream(filePathSupplier, System.IO.FileMode.Open)
                ,out errMsg
                );

            Assert.AreEqual(104, Supplier.Count);
        }

        /// <summary>
        /// 提取excel里的图片
        /// </summary>
        [Test]
        public void ReadProductWithImageFromExcelTest()
        {

            string filePath = Environment.CurrentDirectory + @"\TestFiles\图片提取20130306.xls";
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
            string filePath = Environment.CurrentDirectory + @" \TestFiles\吧台设备及用具.XLS";

            IList<Product> products = bizProduct.ReadListFromExcel(new System.IO.FileStream(filePath, System.IO.FileMode.Open)
                   , out errMsg);

            Assert.AreEqual(40, products.Count);
            Assert.AreEqual("01.001", products[0].CategoryCode);

        }
        //导入分类列表
        [Test]
        public void ReadCategoryFromExcel()
        {
            string filePath = Environment.CurrentDirectory + @"\TestFiles\分类表.xls";
           
            IList<Category> products = bizCategory.ReadListFromExcel(new System.IO.FileStream(filePath, System.IO.FileMode.Open),out errMsg);

            Assert.AreEqual(465, products.Count);
           
        }
        //英文ERP格式问题
        [Test]
        public void ReadProductErpEnglishFromExcel()
        {
            string filePath = Environment.CurrentDirectory + @"\TestFiles\英文——2013-3-26家具（brighthome）数据表.XLS";
            IList<Product> products = bizProduct.ReadListFromExcel(new System.IO.FileStream(filePath, System.IO.FileMode.Open)
                , out errMsg);

            Assert.AreEqual(153, products.Count);
            //Assert.AreEqual("01.001", products[0].CategoryCode);

        }
        //包含英文名的供應商列表
        [Test]
        public void ReadSupplierWithSupplierFromExcelTest()
        {

            string filePathSupplier = Environment.CurrentDirectory + @"\TestFiles\供应商119(WithEnglishNames)  2013-5-7.xls";
            IList<Supplier> Supplier = bizSupplier.ReadSupplierListFromExcel(new System.IO.FileStream(filePathSupplier, System.IO.FileMode.Open)
               , out errMsg
               );

            Assert.AreEqual(119, Supplier.Count);
        }
    }
}
