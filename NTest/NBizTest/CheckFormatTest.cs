using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NModel;
using NBiz;
using System.IO;
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
            /*
              测试点:
             * 1)报价单有空白行
             * 
             */
            IList<Product> productsHasPicture, productsNotHasPicture;

            IList<FileInfo> imagesHasProduct, imagesHasNotProduct;
            string folderName = Environment.CurrentDirectory + "\\TestFiles\\FormatCheck\\潮州市金源利陶瓷制作有限公司\\";
            FormatCheck checker = new FormatCheck();
            checker.CheckSingleFolder(folderName,
                out productsHasPicture,
                out productsNotHasPicture, out imagesHasProduct
                , out imagesHasNotProduct);
            // Assert.AreEqual("Success", FormatChecker.Check(folderContainsExcelAndImages));
            Assert.AreEqual(2,productsHasPicture.Count);
                Assert.AreEqual(1,productsNotHasPicture.Count);
                    Assert.AreEqual(3,imagesHasNotProduct.Count);
                    Assert.AreEqual(2, imagesHasProduct.Count);

                    checker.HandlerCheckResult(
                        productsHasPicture
                        , productsNotHasPicture
                        , imagesHasProduct
                        , imagesHasNotProduct
                        , Environment.CurrentDirectory + "\\TestFiles\\FormatCheck\\检测结果\\");
        }


        [Test]
        public void CheckFormat2()
        {
            IList<Product> productsHasPicture, productsNotHasPicture;

            IList<FileInfo> imagesHasProduct, imagesHasNotProduct;
            string folderName = Environment.CurrentDirectory + "\\TestFiles\\FormatCheck\\河南华夏丝毯厂\\";
            FormatCheck checker = new FormatCheck();
            checker.CheckSingleFolder(folderName,
                out productsHasPicture,
                out productsNotHasPicture, out imagesHasProduct
                , out imagesHasNotProduct);
            // Assert.AreEqual("Success", FormatChecker.Check(folderContainsExcelAndImages));
            Assert.AreEqual(0, productsHasPicture.Count);
            Assert.AreEqual(5, productsNotHasPicture.Count);
            Assert.AreEqual(5, imagesHasNotProduct.Count);
            Assert.AreEqual(0, imagesHasProduct.Count);

            checker.HandlerCheckResult(
                productsHasPicture
                , productsNotHasPicture
                , imagesHasProduct
                , imagesHasNotProduct
                , Environment.CurrentDirectory + "\\TestFiles\\FormatCheck\\检测结果\\");
        }


        /// <summary>
        ///
        /// </summary>
        [Test]
        public void CheckFormat_CantFindTifFile()
        {
            IList<Product> productsHasPicture, productsNotHasPicture;

            IList<FileInfo> imagesHasProduct, imagesHasNotProduct;
            string folderName = Environment.CurrentDirectory + "\\TestFiles\\FormatCheck\\恒铵\\恒铵A级\\";
            FormatCheck checker = new FormatCheck();
            checker.CheckSingleFolder(folderName,
                out productsHasPicture,
                out productsNotHasPicture, out imagesHasProduct
                , out imagesHasNotProduct);
            // Assert.AreEqual("Success", FormatChecker.Check(folderContainsExcelAndImages));
            Assert.AreEqual(14, productsHasPicture.Count);
            Assert.AreEqual(0, productsNotHasPicture.Count);
            Assert.AreEqual(0, imagesHasNotProduct.Count);
            Assert.AreEqual(14, imagesHasProduct.Count);

            checker.HandlerCheckResult(
                productsHasPicture
                , productsNotHasPicture
                , imagesHasProduct
                , imagesHasNotProduct
                , Environment.CurrentDirectory + "\\TestFiles\\FormatCheck\\检测结果\\");
        }


        /// <summary>
       
        /// 2 支持多种图片格式
        /// </summary>
        [Test]
        public void CheckFormat_IllegalCharactorInPath()
        {
            IList<Product> productsHasPicture, productsNotHasPicture;

            IList<FileInfo> imagesHasProduct, imagesHasNotProduct;
            string folderName = Environment.CurrentDirectory + "\\TestFiles\\FormatCheck\\巴非\\";
            FormatCheck checker = new FormatCheck();
            checker.CheckSingleFolder(folderName,
                out productsHasPicture,
                out productsNotHasPicture, out imagesHasProduct
                , out imagesHasNotProduct);
            // Assert.AreEqual("Success", FormatChecker.Check(folderContainsExcelAndImages));
            Assert.AreEqual(1, productsHasPicture.Count);
            Assert.AreEqual(0, productsNotHasPicture.Count);
            Assert.AreEqual(1, imagesHasNotProduct.Count);
            Assert.AreEqual(1, imagesHasProduct.Count);

            checker.HandlerCheckResult(
                productsHasPicture
                , productsNotHasPicture
                , imagesHasProduct
                , imagesHasNotProduct
                , Environment.CurrentDirectory + "\\TestFiles\\FormatCheck\\检测结果\\");
        }

    }
}
