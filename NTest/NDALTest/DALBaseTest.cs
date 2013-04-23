using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NModel;
namespace NTest.NDALTest
{
    [TestFixture]
    public class DALBaseTest
    {
        [Test]
        public void SaveTest()
        {
            var finfo = new System.IO.FileInfo(Environment.CurrentDirectory + (@"\config\log4net.config"));
            log4net.Config.XmlConfigurator.Configure(finfo);
            NDAL.DalBase<Product> dal = new NDAL.DALProduct();
            Product p = new Product();
            p.SupplierName = "开平市祥云卫浴制品有限公司";
            p.ModelNumber = "XY-3918";
            p.NTSCode = "01.012.0000400012";
            dal.Save(p);

            Product p2 = dal.GetOne(p.Id);

            Assert.AreEqual(p.Id, p2.Id);
          //  dal.Delete(p);

            
        }
    }
}
