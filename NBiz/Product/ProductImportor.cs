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



        public IList<Product> Result_ProductsSuccessImported { private set; get; }
   
        public ProductImportor() {
            Result_ProductsSuccessImported = new List<Product>();
        }
        public string WebProductImagesPath { get; set; }
        public ProductImportor(bool checkWithDb)
            : this()
        {
            CheckWithDatabase = checkWithDb;
        }

        StringBuilder sbMsg = new StringBuilder();
        StringBuilder sbMsgForLog = new StringBuilder();
        public string ImportMsg
        {
            get
            {
                return sbMsg.ToString();
            }
        }
        /// <summary>
        /// 保存到操作记录里的记录., 不记录具体产品信息,只记录数量 控制字符串长度
        /// </summary>
        public string ImportMsgForLog
        {
            get { return sbMsgForLog.ToString(); }
        }
        public void Import(DirectoryInfo[] originalFolders, string outFolder

            )
        {

            sbMsg.AppendLine("---------开始导入--------");
            foreach (DirectoryInfo dir in originalFolders)
            {
                sbMsg.AppendLine("供应商-导入开始:" + dir.Name);
                CheckAndSaveSingleFold(dir.FullName, outFolder);
                sbMsg.AppendLine("");

            }
            sbMsg.AppendLine("----------导入完成--------");
            NLogger.Logger.Debug(sbMsg.ToString());

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
        /// 处理单个文件夹 检查 和 保存 
        /// </summary>
        /// <param name="folderPath">包含 excel和图片  文件夹路径</param>
        /// <param name="outSavePath"> 最终结果 保存路径. 合格数据, 多余图片,多余excel数据,数据库内已存在数据 </param>
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
            string saveMsg;
            BizProduct.SaveList(productsHasPicture,out saveMsg );
            sbMsg.AppendLine(productsHasPicture.Count + ":已导入");
            //结果保存到文件夹
            DateTime beginSaveResultToDisk = DateTime.Now;
            string supplierName = string.Empty;
            if (productsExistedInDB.Count > 0) supplierName = productsExistedInDB[0].SupplierName;
            else if (productsHasPicture.Count > 0) supplierName = productsHasPicture[0].SupplierName;
            else if (productsNotHasPicture.Count > 0) supplierName = productsNotHasPicture[0].SupplierName;
            else
            {
                return;
            }
            supplierName = StringHelper.ReplaceInvalidChaInFileName(supplierName, string.Empty);
            HandlerCheckResult(supplierName,
                productsHasPicture
                , productsNotHasPicture
                , productsExistedInDB
                , imagesHasProduct
                , imagesHasNotProduct
                , outSavePath
                );
            //  sbMsg.AppendLine(productsHasPicture.Count+":有效图片数量" );
            //
            if (productsHasPicture.Count > 0)
            {
                Result_ProductsSuccessImported = Result_ProductsSuccessImported.Concat(productsHasPicture).ToList();
            }
            sbMsgForLog.AppendLine(productsHasPicture.Count+"-成功导入数量:");
            sbMsgForLog.AppendLine(productsNotHasPicture.Count+"-没有图片的产品数量");
            sbMsgForLog.AppendLine(productsExistedInDB.Count+"-数据库里已存在的产品数量");
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
            sbMsg.AppendLine(products.Count + ":待导入");
        
            sbMsgForLog.AppendLine(products.Count+"-待导入产品数量-----"+dir.Name);
            //排除数据库内重复的数据
            IList<Product> validItems = products;
            productsExistedInDB = new List<Product>();
            if (CheckWithDatabase)
            {
                DateTime beginCheckDbExists = DateTime.Now;
              
                validItems = BizProduct.CheckDB(
                    products, out productsExistedInDB);
                Console.WriteLine("Time Cost CheckDB:" + (DateTime.Now - beginCheckDbExists).TotalSeconds);
                sbMsg.AppendLine(productsExistedInDB.Count + ":已存在");
                foreach (Product pi in productsExistedInDB)
                {
                    sbMsg.AppendLine("     名称:" + pi.Name + "型号:" + pi.ModelNumber + "供应商名称:" + pi.SupplierName);
                }
            }
            //
            DateTime beginCheckImage = DateTime.Now;
            CheckProductImages(products, folderPath, out productsHasPicture
           , out productsNotHasPicture
           , out  imagesHasProduct
           , out  imagesHasNotProduct);
            Console.WriteLine("Time Cost CheckImage:" + (DateTime.Now - beginCheckImage).TotalSeconds);
            sbMsg.AppendLine(productsNotHasPicture.Count + ":无图片");
            foreach (Product pnopic in productsNotHasPicture)
            {
                sbMsg.AppendLine("     名称:" + pnopic.Name + "型号:" + pnopic.ModelNumber + "供应商名称:" + pnopic.SupplierName);

            }

        }

        public void CheckProductImages(IList<Product> productsNotExistInDb, string ImageFolder,
           out IList<Product> productsHasPicture
           , out IList<Product> productsNotHasPicture
           , out IList<FileInfo> imagesHasProduct
           , out IList<FileInfo> imagesHasNotProduct)
        {
            DirectoryInfo dir = new DirectoryInfo(ImageFolder);
            FileInfo[] images = dir.GetImageFiles(SearchOption.AllDirectories).ToArray<FileInfo>();// dirImage.GetFiles();

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
                        string newImageName = (p.Name + p.SupplierName + p.ModelNumber).GetHashCode().ToString() + image.Extension;
                        p.ProductImageUrls.Add(newImageName);
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
        public void HandlerCheckResult(string supplierName, IList<Product> productHasImages, IList<Product> productNotHasImages,
            IList<Product> productsExistedInDb,
            IList<FileInfo> imagesHasProduct, IList<FileInfo> imagesNotHasProduct,
            string outputFolder)
        {
            DirectoryInfo dirRoot = new DirectoryInfo(outputFolder);
            TransferInDatatable transfer = new TransferInDatatable();
            //如果没有合格数据 则不需要创建
            if (productHasImages.Count > 0)
            {
                DirectoryInfo dirQuanlified = IOHelper.EnsureDirectory(outputFolder + "合格数据\\");
                DirectoryInfo dirSupplierQuanlified = IOHelper.EnsureDirectory(dirQuanlified.FullName + supplierName + "\\");
                DirectoryInfo dirSupplierQuanlifiedImages = IOHelper.EnsureDirectory(dirSupplierQuanlified.FullName + supplierName + "\\");
                foreach (Product product in productHasImages)
                {
                    try
                    {
                        FileInfo imageFile = imagesHasProduct.Single(x => StringHelper.ReplaceSpace(Path.GetFileNameWithoutExtension(x.Name))
                          .Equals(StringHelper.ReplaceSpace(product.ModelNumber), StringComparison.OrdinalIgnoreCase));
                        File.Copy(imageFile.FullName, dirSupplierQuanlified.FullName + supplierName + "\\" + imageFile.Name, true);
                        //同时拷贝到网站图片路径
                        if (!string.IsNullOrEmpty(WebProductImagesPath) && outputFolder != WebProductImagesPath)
                        {
                            string newImageName = (product.Name + product.SupplierName + product.ModelNumber).GetHashCode().ToString() + imageFile.Extension;

                            File.Copy(imageFile.FullName, WebProductImagesPath + newImageName, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("图片复制出错:" + dirSupplierQuanlified.FullName + "---" + product.ModelNumber + "---" + ex.Message);
                    }
                }


                DataTable dtProductsHasImage = ObjectConvertor.ToDataTable<Product>(productHasImages);
                transfer.CreateXslFromDataTable(dtProductsHasImage, 1, dirSupplierQuanlified.FullName + "\\" + supplierName + ".xls");
            }

            //没有图片的产品
            string dirPathNotQuanlified = outputFolder + "不合格数据\\";
            string dirPathSupplierNotQuanlified = dirPathNotQuanlified + supplierName + "\\";
            if (productNotHasImages.Count > 0)
            {
                DirectoryInfo dirSupplierNotQuanlified = IOHelper.EnsureDirectory(dirPathSupplierNotQuanlified);
                DataTable dtProductsNotHasImage = ObjectConvertor.ToDataTable<Product>(productNotHasImages);
                transfer.CreateXslFromDataTable(dtProductsNotHasImage, 1, dirSupplierNotQuanlified + "没有图片的数据_" + supplierName + ".xls");

            }
            //没有产品的图片
            if (imagesNotHasProduct.Count > 0)
            {

                string dirPathSupplierNotQuanlifiedImages = dirPathSupplierNotQuanlified + "多余图片_" + supplierName + "\\";
                DirectoryInfo dirSupplierNotQuanlifiedImages = IOHelper.EnsureDirectory(dirPathSupplierNotQuanlifiedImages);
               foreach (FileInfo file in imagesNotHasProduct)
                {
                    file.CopyTo(dirSupplierNotQuanlifiedImages + file.Name, true);
                }
            }
            //多余的图片

            //重复数据
            if (productsExistedInDb.Count > 0)
            {
                string dirPathSupplierRepeated = dirPathSupplierNotQuanlified + "数据库内已存在的数据_" + supplierName + "\\";
                DirectoryInfo dirSupplierRepeated = IOHelper.EnsureDirectory(dirPathSupplierRepeated);
            
                DataTable dtProductsRepeated = ObjectConvertor.ToDataTable<Product>(productsExistedInDb);
                transfer.CreateXslFromDataTable(dtProductsRepeated, 1, dirSupplierRepeated.FullName+"\\" + supplierName + ".xls");
            }
        }

    }
}
