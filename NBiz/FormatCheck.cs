using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NModel;
using System.Data;
using NLibrary;
namespace NBiz
{
    public class FormatCheck
    {

        public void Check(string originalFolder,string outFolder)
        {
            originalFolder = IOHelper.EnsureFoldEndWithSlash(originalFolder);
            outFolder = IOHelper.EnsureFoldEndWithSlash(outFolder);
           //判断是多个文件夹 还是一个.
            DirectoryInfo dir = new DirectoryInfo(originalFolder);
            var subdirs = dir.GetDirectories();
            var files = dir.GetFiles("*.xls");
            //是多个文件夹
            if (subdirs.Length > 1 && files.Length == 0)
            {
                foreach (DirectoryInfo childDir in subdirs)
                {
                    CheckAndSaveSingleFold(childDir.FullName, outFolder);
                }
            }
            else {
                CheckAndSaveSingleFold(originalFolder, outFolder);
            }

            
        }


        public void CheckAndSaveSingleFold(string folderPath,string outSavePath)
        {
            IList<Product> productsHasPicture, productsNotHasPicture;

            IList<FileInfo> imagesHasProduct, imagesHasNotProduct;
            string folderName = folderPath;
            FormatCheck checker = new FormatCheck();
            checker.CheckSingleFolder(folderName,
                out productsHasPicture,
                out productsNotHasPicture, out imagesHasProduct
                , out imagesHasNotProduct);
            // Assert.AreEqual("Success", FormatChecker.Check(folderContainsExcelAndImages));

            checker.HandlerCheckResult(
                productsHasPicture
                , productsNotHasPicture
                , imagesHasProduct
                , imagesHasNotProduct
                , outSavePath);
        }

        public void CheckSingleFolder(string folderPath,
            out IList<Product> productsHasPicture
            , out IList<Product> productsNotHasPicture
            , out IList<FileInfo> imagesHasProduct
            , out IList<FileInfo> imagesHasNotProduct)
        {
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            FileInfo[] excelFiles = dir.GetFiles("*.xls", SearchOption.TopDirectoryOnly);
            if (excelFiles.Length != 1)
            {
                throw new Exception("错误,文件夹 " + folderPath + " 内有多个Excel文件,应该有且仅有一个excel文件");
            }
            FileInfo excelFile = excelFiles[0];
            //DirectoryInfo[] dirs = dir.GetDirectories();
            //if (dirs.Length != 1)
            //{
            //    throw new Exception("错误,文件夹 " + folderPath + " 内有多个图片文件,应该有且仅有一个图片文件夹");
            //}
           // DirectoryInfo dirImage = dirs[0];
            Stream stream = new FileStream(excelFile.FullName, FileMode.Open);

            IDataTableConverter<Product> productReader = new ProductDataTableConverter();
            string errMsg;
            DataTable dt = new NLibrary.ReadExcelToDataTable(stream).Read(out errMsg);
            IList<Product> products = productReader.Convert(dt);
            FileInfo[] images = dir.GetFilesByExtensions(BizVariables.SupportImageExtensionsForImport).ToArray<FileInfo>();// dirImage.GetFiles();
           
            productsHasPicture = new List<Product>();
            productsNotHasPicture = new List<Product>();

            imagesHasProduct = new List<FileInfo>();
            imagesHasNotProduct = new List<FileInfo>();

            //写一个通用类,比较两个序列,返回匹配结果.
            //Compare<T1,T2>  T1和T2需要实现他们两者比较的接口
            foreach (Product p in products)
            {
                bool productHasImage = false;
                Console.WriteLine("productModel:" + p.ModelNumber);
                foreach (FileInfo image in images)
                {
                    string imageName = StringHelper.ReplaceSpace(Path.GetFileNameWithoutExtension(image.Name));
                    Console.Write("imageName:" + imageName);
                    if (imageName
                        .Equals(StringHelper.ReplaceSpace(p.ModelNumber), StringComparison.OrdinalIgnoreCase))
                    {
                        productsHasPicture.Add(p);
                        imagesHasProduct.Add(image);
                        productHasImage = true;
                        break;
                    }
                }
                if (!productHasImage)
                {
                    productsNotHasPicture.Add(p);
                }
            }
            foreach (FileInfo f in images)
            {
                bool imageHasProduct = false;
                foreach (FileInfo f2 in imagesHasProduct)
                {
                    if (f.Name.Equals(f2.Name))
                    {
                        imageHasProduct = true;
                        break;
                    }
                }
                if (!imageHasProduct)
                {
                    imagesHasNotProduct.Add(f);
                }
            }

        }

        /*
         导入结构:
         * 总目录-|
         *        -供应商-|
         *                -图片文件夹
         *                -产品数据.xls
         *
         * 结果目录:
         *  总目录- 
         *        --合格数据
         *        --不合格数据
         *               --供应商
         *                   --没有产品信息的图片
         *                   --没有图片的产品.xls
         */
        /// <summary>
        /// 将结果保存到磁盘:
        /// </summary>
        /// <param name="productHasImages"></param>
        public void HandlerCheckResult(IList<Product> productHasImages, IList<Product> productNotHasImages,
            IList<FileInfo> imagesHasProduct, IList<FileInfo> imagesNotHasProduct,
            string outputFolder)
        {
            DirectoryInfo dirRoot = new DirectoryInfo(outputFolder);
            DirectoryInfo dirQuanlified = IOHelper.EnsureDirectory(outputFolder + "合格数据\\");
            DirectoryInfo dirNotQuanlified = IOHelper.EnsureDirectory(outputFolder + "不合格数据\\");
            string supplierName=string.Empty;
            TransferInDatatable transfer = new TransferInDatatable();
            //合格数据和图片
            //合格数据根目录
             DirectoryInfo dirSupplierQuanlified =null;
             DirectoryInfo dirSupplierQuanlifiedImages = null;

            if(productHasImages.Count>0)
            {
                supplierName = productHasImages[0].SupplierName;
            }
            else if (productNotHasImages.Count > 0)
            {
                supplierName = productNotHasImages[0].SupplierName;
            }
            else
            {
                throw new Exception("错误无法获取供应商信息.");
            }
            supplierName= StringHelper.ReplaceInvalidChaInFileName(supplierName, string.Empty);
            dirSupplierQuanlified = IOHelper.EnsureDirectory(dirQuanlified.FullName + supplierName + "\\");

            dirSupplierQuanlifiedImages = IOHelper.EnsureDirectory(dirSupplierQuanlified.FullName + supplierName + "\\");
            foreach (Product product in productHasImages)
            {

                try
                {
                    FileInfo imageFile = imagesHasProduct.Single(x => StringHelper.ReplaceSpace(Path.GetFileNameWithoutExtension(x.Name))
                      .Equals(StringHelper.ReplaceSpace(product.ModelNumber), StringComparison.OrdinalIgnoreCase));
                    File.Copy(imageFile.FullName, dirSupplierQuanlified.FullName + supplierName + "\\" + imageFile.Name, true);
                }
                catch (Exception ex)
                {
                    throw new Exception("图片复制出错:" +dirSupplierQuanlified.FullName+"---"+ product.ModelNumber + "---" + ex.Message);
                }
            }
             
           
            DataTable dtProductsHasImage=ObjectConvertor.ToDataTable<Product>(productHasImages);
            transfer.CreateXslFromDataTable(dtProductsHasImage, 1, dirSupplierQuanlified.FullName+ "\\" + supplierName + ".xls");
            //不合格数据和图片
            string dirSupplierNotQuanlified = dirNotQuanlified.FullName + supplierName + "\\";

            string dirSupplierNotQuanlifiedImages = dirSupplierNotQuanlified + "多余图片_" + supplierName + "\\";
            IOHelper.EnsureDirectory(dirSupplierNotQuanlified);
            IOHelper.EnsureDirectory(dirSupplierNotQuanlifiedImages);
            foreach (FileInfo file in imagesNotHasProduct)
            {
                file.CopyTo(dirSupplierNotQuanlifiedImages + file.Name, true);
            }
            DataTable dtProductsNotHasImage = ObjectConvertor.ToDataTable<Product>(productNotHasImages);
            transfer.CreateXslFromDataTable(dtProductsNotHasImage, 1, dirSupplierNotQuanlified + "没有图片的数据_" + supplierName + ".xls");
        }
    }
}
