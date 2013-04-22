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
            string filePath = Environment.CurrentDirectory + @"\TestFiles\NTS产品报价单1756.xls";
            NBiz.ImportFromExcel importer = new ImportFromExcel();
            IList<Product> products= importer.ReadProductFromExcel(filePath);

            Assert.AreEqual(6,products.Count);
        }
    }
}
