using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NModel;
using NBiz;
namespace NTest.NBizTest
{
    public class ImportFromExcelTest
    {
        public void ReadProductFromExcelTest()
        {
            string filePath = Environment.CurrentDirectory + @"\TestFiles\NTS 产品报价单   哈慈 20130306.xls";
            ProductExcelReader importer = new ProductExcelReader();
            IList<Product> products = importer.Read(new System.IO.FileStream(filePath, System.IO.FileMode.Open));

            Assert.AreEqual(19,products.Count);

        }
        public void ReadSupplierFromExcelTest()
        {

            string filePathSupplier = Environment.CurrentDirectory + @"\TestFiles\供应商104.xls";
            SupplierExcelReader importerSupplier = new SupplierExcelReader();
            IList<Supplier> Supplier = importerSupplier.Read(new System.IO.FileStream(filePathSupplier, System.IO.FileMode.Open));

            Assert.AreEqual(104, Supplier.Count);
        }
    }
}
