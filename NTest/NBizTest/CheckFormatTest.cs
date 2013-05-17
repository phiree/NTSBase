using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NModel;
using NBiz;
using System.IO;
using NDAL;
using FizzWare.NBuilder;
using Rhino.Mocks;
using NLibrary;
namespace NTest.NBizTest
{

    /// <summary>
    /// excel 格式检查. 对应图片统计信息.
    /// </summary>
    [TestFixture]
    public class CheckFormatTest
    {
       
        [Test]
        public void CheckFormat_ImageFolderHasSubDirectories_ImageFileNameCaseAndSpace()
        {
            DateTime beginMock = DateTime.Now;
            var dalProduct = MockRepository.GenerateMock<NDAL.DALProduct>();
            var dalSupplier = MockRepository.GenerateMock<DALSupplier>();
            dalSupplier.Expect(x => x.GetOneByName("Chaozhou Jinyuanli Ceramics Co.,1 Ltd."))

                .Return(Builder<Supplier>.CreateNew().With(x => x.Code = "001").Build())
                .IgnoreArguments()
                ;
            DateTime beginInvockTest = DateTime.Now;
            dalProduct.Expect(x => x.SaveList(new List<Product>())).IgnoreArguments();
                

            dalProduct.Expect(x => x.GetOneByModelNumberAndSupplier("J10335", "001"))
             .Return(Builder<Product>.CreateNew().Build());
            //mock对象严重影响性能, Time Cost EndMock:17.6875;ook 19.02 seconds 
            Console.WriteLine("Time Cost EndMock:" + (DateTime.Now - beginMock).TotalSeconds);
         
            /*
            // 1 完全合格的产品,可以导入(还未导入过,有图片)
            //,2 未导入,且没图片的,3 productExistsInDb,4 imagesNotHasProduct
             * */
            CheckSingleFolderTest("潮州市金源利陶瓷制作有限公司",1,1, 1,4, true, dalProduct, dalSupplier);
            
        }

        


        /// <summary>
        ///
        /// </summary>
        [Test]
        public void CheckFormat_CantFindTifFile()
        {
            CheckSingleFolderTest("恒铵", 14, 0, 0, 0, false, null, null);
            
        }


        /// <summary>

        /// 2 支持多种图片格式
        /// </summary>
        [Test]
        public void CheckFormat_IllegalCharactorInPath()
        {
            CheckSingleFolderTest("巴非", 1, 0,0, 1, false, null, null);
            
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName">包含excel和对应产品图片的文件夹</param>
        /// <param name="amountProductsHasPicture_NotExitsted">可供导入的产品(没有导入过,有图片)</param>
        /// <param name="amountProductNotHasPicture_OrHasExisted">不合格产品(已导入,或 没图片</param>
        /// <param name="amountProductExistsInDb_HasExisted">不合格产品(已导入)</param>
        /// <param name="amountImagesHasNotProduct_OrHasExisted">没有产品信息的图片</param>
        /// <param name="needCheckDataBase">是否从数据库查询产品是否存在</param>
        /// <param name="dalProduct">供mock,判断是否存在的方法</param>
        /// <param name="dalSupplier">供mock,获取供应商的方法.</param>
      
        private void CheckSingleFolderTest(
             string folderName
            , int amountProductsHasPicture
            , int amountProductNotHasPicture
            , int amountProductExistsInDb
            , int amountImagesHasNotProduct
            , bool needCheckDataBase
            , DALProduct dalProduct
            , DALSupplier dalSupplier)
        {
            IList<Product> productsHasPicture, productsNotHasPicture, productsExistedInDB;
            IList<FileInfo> imagesHasProduct, imagesHasNotProduct;
            string folderFullPath = Environment.CurrentDirectory + "\\TestFiles\\FormatCheck\\" + folderName + "\\";
            ProductImportor checker = new ProductImportor();
            checker.BizProduct.DalBase = dalProduct;
            checker.BizProduct.DalSupplier = dalSupplier;
            checker.CheckWithDatabase = needCheckDataBase;
            checker.CheckSingleFolder(folderFullPath
                , out productsHasPicture
                , out productsNotHasPicture
                , out productsExistedInDB
                , out imagesHasProduct
                , out imagesHasNotProduct);
            // Assert.AreEqual("Success", FormatChecker.Check(folderContainsExcelAndImages));
            Assert.AreEqual(amountProductsHasPicture, productsHasPicture.Count);
            Assert.AreEqual(amountProductNotHasPicture, productsNotHasPicture.Count);
            Assert.AreEqual(amountProductExistsInDb, productsExistedInDB.Count);
            Assert.AreEqual(amountImagesHasNotProduct, imagesHasNotProduct.Count);
            DateTime beginSaveResult = DateTime.Now;
            string saveFolder=Environment.CurrentDirectory + "\\TestFiles\\FormatCheck\\检测结果\\";
              string saveFolderOfSupplier;
           if (productsHasPicture.Count > 0) saveFolderOfSupplier = productsHasPicture[0].SupplierName;
           else if (productsNotHasPicture.Count > 0) saveFolderOfSupplier = productsNotHasPicture[0].SupplierName;
           else throw new Exception(); 
            
            DirectoryInfo dirOfSavedSupplier = new DirectoryInfo(saveFolder + "合格数据\\" + saveFolderOfSupplier+"\\");
           if (dirOfSavedSupplier.Exists)
           {
               dirOfSavedSupplier.Delete(true);
           }
           string supplierName = string.Empty;
           if (productsExistedInDB.Count > 0) supplierName = productsExistedInDB[0].SupplierName;
           else if (productsHasPicture.Count > 0) supplierName = productsHasPicture[0].SupplierName;
           else if (productsNotHasPicture.Count > 0) supplierName = productsNotHasPicture[0].SupplierName;
           else
           {
               return;
           }
           supplierName = StringHelper.ReplaceInvalidChaInFileName(supplierName, string.Empty);
           checker.HandlerCheckResult(
               supplierName,
                 productsHasPicture
                , productsNotHasPicture
                , productsExistedInDB
                , imagesHasProduct
                , imagesHasNotProduct
                , saveFolder);
        
         
           Assert.AreEqual(productsHasPicture.Count, dirOfSavedSupplier.GetImageFiles().ToArray().Length);
          
           Console.WriteLine("Time Cost CheckImage:" + (DateTime.Now - beginSaveResult).TotalSeconds);
          
            
        }

    }
}
