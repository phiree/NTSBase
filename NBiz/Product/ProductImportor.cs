﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NModel;
using System.Data;
using NLibrary;
namespace NBiz
{
    //导入数据
    /// <summary>
    /// 检查数据有效性
    /// 1) excel 文件有孝心(格式,值的格式)
    /// 2) 图片有效性(是否与excel里的产品型号对应)
    /// 3) 结合数据库检查 数据有效性.
    ///将数据导入
    /// </summary>
    public class ProductImportor
    {
        BizProduct bizProduct;
        public BizProduct BizProduct
        {
            get
            {
                if (bizProduct == null)
                {
                    bizProduct = new BizProduct();
                }
                return bizProduct;
            }
            set
            {
                bizProduct = value;
            }
        }
        public bool CheckWithDatabase { get; set; }
        /// <summary>
        /// 由excel文件创建的流
        /// </summary>



        public IList<Product> Result_productsHasPicture { private set; get; }
        public IList<Product> Result_productsNotHasPicture { private set; get; }
        public IList<Product> Result_productsExistedInDB { private set; get; }
        public IList<FileInfo> Result_imagesHasProduct { private set; get; }
        public IList<FileInfo> Result_imagesHasNotProduct { private set; get; }
        public ProductImportor() { }
        public ProductImportor(bool checkWithDb):this()
        {
            CheckWithDatabase = checkWithDb;
        }

        StringBuilder sbMsg = new StringBuilder();
        public string ImportMsg
        {
            get
            {
                return sbMsg.ToString();
            }
        }
        public void Import(DirectoryInfo[] originalFolders, string outFolder

            )
        {

            sbMsg.AppendLine("---------开始导入--------");
            foreach (DirectoryInfo dir in originalFolders)
            {
                sbMsg.AppendLine("供应商开始:" + dir.Name);
                CheckAndSaveSingleFold(dir.FullName, outFolder);
                sbMsg.AppendLine("供应商结束:" + dir.Name);
            }
            sbMsg.AppendLine("----------导入完成--------");

        }
        public void Import(string originalFolder, string outFolder)
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

                Import(subdirs, outFolder);

            }
            else
            {
                CheckAndSaveSingleFold(originalFolder, outFolder);
            }


        }

        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="outSavePath"></param>
        public void CheckAndSaveSingleFold(string folderPath, string outSavePath)
        {
            IList<Product> productsHasPicture, productsNotHasPicture, productsExistedInDB;

            IList<FileInfo> imagesHasProduct, imagesHasNotProduct;
            string folderName = folderPath;

            CheckSingleFolder(folderName,
                out productsHasPicture,
                out productsNotHasPicture,
                out productsExistedInDB,
                out imagesHasProduct
                , out imagesHasNotProduct);
     
            //将结果保存到数据库
            BizProduct.SaveList(productsHasPicture);
            //结果保存到文件夹
            DateTime beginSaveResultToDisk = DateTime.Now;
            HandlerCheckResult(
                productsHasPicture
                , productsNotHasPicture
                , productsExistedInDB
                , imagesHasProduct
                , imagesHasNotProduct

                , outSavePath);
            Console.WriteLine("Time Cost beginSaveResultToDisk:" + (DateTime.Now - beginSaveResultToDisk).TotalSeconds);
        }
        /// <summary>
        /// 文件结构检查
        ///  正确结构: Folder-|
        ///                   -xls文件.xls
        ///                   -图片文件夹
        ///  excel读取为              
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="productsHasPicture"></param>
        /// <param name="productsNotHasPicture"></param>
        /// <param name="imagesHasProduct"></param>
        /// <param name="imagesHasNotProduct"></param>
        public void CheckSingleFolder(string folderPath
           , out IList<Product> productsHasPicture
            , out IList<Product> productsNotHasPicture
        , out IList<Product> productsExistedInDB
            , out IList<FileInfo> imagesHasProduct
            , out IList<FileInfo> imagesHasNotProduct)
        {
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            FileInfo[] excelFiles = dir.GetFiles("*.xls", SearchOption.TopDirectoryOnly);
            if (excelFiles.Length != 1)
            {
                throw new Exception("错误,文件夹 " + folderPath + " 应该有且仅有一个excel文件");
            }
            FileInfo excelFile = excelFiles[0];
            Stream stream = new FileStream(excelFile.FullName, FileMode.Open);
            IDataTableConverter<Product> productReader = new ProductDataTableConverter();
            string errMsg;
            DataTable dt = new NLibrary.ReadExcelToDataTable(stream).Read(out errMsg);
            IList<Product> products = productReader.Convert(dt);
            //排除数据库内重复的数据
            IList<Product> validItems = products;
            productsExistedInDB = new List<Product>();
            if (CheckWithDatabase)
            {
                DateTime beginCheckDbExists = DateTime.Now;
                string repeatedErrMsg;
                validItems = BizProduct.CheckItemsBeforeSave(
                    products, out productsExistedInDB, out repeatedErrMsg);
                Console.WriteLine("Time Cost CheckDB:" + (DateTime.Now - beginCheckDbExists).TotalSeconds);
            }
            //
            DateTime beginCheckImage = DateTime.Now;
            CheckProductImages(validItems, folderPath, out productsHasPicture
           , out productsNotHasPicture
           , out  imagesHasProduct
           , out  imagesHasNotProduct);
            Console.WriteLine("Time Cost CheckImage:" + (DateTime.Now - beginCheckImage).TotalSeconds);



        }

        public void CheckProductImages(IList<Product> productsNotExistInDb, string ImageFolder,
           out IList<Product> productsHasPicture
           , out IList<Product> productsNotHasPicture
           , out IList<FileInfo> imagesHasProduct
           , out IList<FileInfo> imagesHasNotProduct)
        {
            DirectoryInfo dir = new DirectoryInfo(ImageFolder);
            FileInfo[] images = dir.GetImageFiles().ToArray<FileInfo>();// dirImage.GetFiles();

            productsHasPicture = new List<Product>();
            productsNotHasPicture = new List<Product>();

            imagesHasProduct = new List<FileInfo>();
            imagesHasNotProduct = new List<FileInfo>();

            //写一个通用类,比较两个序列,返回匹配结果.
            //Compare<T1,T2>  T1和T2需要实现他们两者比较的接口
            foreach (Product p in productsNotExistInDb)
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
            IList<Product> productsExistedInDb,
            IList<FileInfo> imagesHasProduct, IList<FileInfo> imagesNotHasProduct,
            string outputFolder)
        {
            DirectoryInfo dirRoot = new DirectoryInfo(outputFolder);
            DirectoryInfo dirQuanlified = IOHelper.EnsureDirectory(outputFolder + "合格数据\\");
            DirectoryInfo dirNotQuanlified = IOHelper.EnsureDirectory(outputFolder + "不合格数据\\");
            string supplierName = string.Empty;
            TransferInDatatable transfer = new TransferInDatatable();
            //合格数据和图片
            //合格数据根目录
            DirectoryInfo dirSupplierQuanlified = null;
            DirectoryInfo dirSupplierQuanlifiedImages = null;

            if (productHasImages.Count > 0)
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
            supplierName = StringHelper.ReplaceInvalidChaInFileName(supplierName, string.Empty);
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
                    throw new Exception("图片复制出错:" + dirSupplierQuanlified.FullName + "---" + product.ModelNumber + "---" + ex.Message);
                }
            }


            DataTable dtProductsHasImage = ObjectConvertor.ToDataTable<Product>(productHasImages);
            transfer.CreateXslFromDataTable(dtProductsHasImage, 1, dirSupplierQuanlified.FullName + "\\" + supplierName + ".xls");
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
            //重复数据
            DataTable dtProductsRepeated = ObjectConvertor.ToDataTable<Product>(productsExistedInDb);
            transfer.CreateXslFromDataTable(dtProductsRepeated, 1, dirSupplierNotQuanlified + "数据库内已存在的数据_" + supplierName + ".xls");

        }
    }
}
