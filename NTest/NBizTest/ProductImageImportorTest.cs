using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FizzWare.NBuilder;
using Rhino.Mocks;
using NLibrary;
using NDAL;
using NModel;
namespace NTest
{
    public class ProductImageImportorTest
    {
        
        public void ImportTest()
        {
            bool needMockDAL = false;
            NBiz.ProductImageImporter oer = new NBiz.ProductImageImporter();
            if (needMockDAL)
            {
            var dalProduct = MockRepository.GenerateMock<NDAL.DALProduct>();
         
            var dalSupplier = MockRepository.GenerateMock<DALSupplier>();
            //根据名称获取供应商
            //var args = dalSupplier.GetArgumentsForCallsMadeOn(x => x.GetOneByName(""), y => y.IgnoreArguments());
            //int argLenth = args.Count;
            //string paramSupplierName = (string)args[argLenth - 1][0];
            dalSupplier.Expect(x => x.GetOneByName("佛山市可纳家具有限公司"))
                .Return(Builder<Supplier>.CreateNew().With(x => x.Code = "001")
                                                     .With(x => x.Name = "佛山市可纳家具有限公司").Build());
            dalSupplier.Expect(x => x.GetOneByName("佛山市南海承标金属制品有限公司"))
             .Return(Builder<Supplier>.CreateNew().With(x => x.Code = "002")
                                                  .With(x => x.Name = "佛山市南海承标金属制品有限公司").Build());

        
            
            DateTime beginInvockTest = DateTime.Now;


            dalProduct.Expect(x => x.GetListBySupplierCode("001"))
             .Return(Builder<Product>.CreateListOfSize(2)
             .TheFirst(1).With(x => x.ModelNumber = "KNB$5029")
             .Build());
            dalProduct.Expect(x => x.GetListBySupplierCode("002"))
           .Return(Builder<Product>.CreateListOfSize(2)
           .TheFirst(1).With(x => x.ModelNumber = "OD-84$啡色")
           .Build());

            dalProduct.Expect(x => x.Update(null)).IgnoreArguments();

                oer.DalProduct = dalProduct;
                oer.DalSupplier = dalSupplier;
            }
            string msg;
            var list = oer.ImportImage(Environment.CurrentDirectory + "\\TestFiles\\ProductImages\\"
                , @"d:\original\", out msg);

            Console.Write(msg);
            Assert.AreEqual(2, list.Count);

            //NBiz.BizProduct bizProduct = new NBiz.BizProduct();
          // NModel.Product p= bizProduct.GetOne(new Guid("92832d2d-b28d-422c-89e5-a1aa01216ec5"));
          // Assert.AreEqual(4, p.ProductImageUrls.Count);
            
        }
    }
}
