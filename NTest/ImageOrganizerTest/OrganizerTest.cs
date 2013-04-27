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
            NBiz.Organizer oer = new NBiz.Organizer();
            string msg;
            var list = oer.Execute(@"E:\workspace\code\resources\导入资料\datafiles\已整理好图片\", @"E:\workspace\code\resources\VirtualDirectory\NTSBase\ProductImages\original\", out msg);

            Console.Write(msg);
            Assert.AreEqual(1, list.Count);

            //NBiz.BizProduct bizProduct = new NBiz.BizProduct();
          // NModel.Product p= bizProduct.GetOne(new Guid("92832d2d-b28d-422c-89e5-a1aa01216ec5"));
          // Assert.AreEqual(4, p.ProductImageUrls.Count);
            
        }
    }
}
