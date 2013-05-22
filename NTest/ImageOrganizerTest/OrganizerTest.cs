using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
namespace NTest
{
    public class OrganizerTest
    {
        
        public void ExecuteTest()
        {
            NBiz.ProductImageImporter oer = new NBiz.ProductImageImporter();
            string msg;
            var list = oer.ImportImage(@"E:\workspace\document\2013.3.25（第二期数据）\newName"
                , @"Y:\original\", out msg);

            Console.Write(msg);
            Assert.AreEqual(1, list.Count);

            //NBiz.BizProduct bizProduct = new NBiz.BizProduct();
          // NModel.Product p= bizProduct.GetOne(new Guid("92832d2d-b28d-422c-89e5-a1aa01216ec5"));
          // Assert.AreEqual(4, p.ProductImageUrls.Count);
            
        }
    }
}
