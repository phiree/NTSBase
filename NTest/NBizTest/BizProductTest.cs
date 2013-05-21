using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NBiz;
using NUnit.Framework;
namespace NTest.NBizTest
{
    [TestFixture]
    public class BizProductTest
    {
        BizProduct bizProduct = new BizProduct();
        [Test]
        public void GetListByProvidedModelNumberSupplierNameListTest()
        { 
            string list=@"brighthome	BP-811
brighthome	BP-823
brighthome	BP-852
brighthome	BP-227
brighthome	BP-BC32
";
            string msg;
            IList<Product> products = bizProduct.GetListByProvidedModelNumberSupplierNameList(list, out msg);

            
        }
    }
    
}
