using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBiz;
using NUnit.Framework;
namespace NTest.NBizTest
{
    public class SupplierTest
    {
        BizSupplier bizS = new BizSupplier();
        public void SearchTest()
        { 
            int recordCount;
            var list = bizS.Search("", 0, 10, out recordCount);
            Assert.AreEqual(10, list);
            
        }
    }
}
