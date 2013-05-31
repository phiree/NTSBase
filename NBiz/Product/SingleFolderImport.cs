using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using System.IO;
using NModel.Enums;
using System.Data;
using NLibrary;
namespace NBiz
{
    /// <summary>
    /// 处理单个供应商文件夹
    /// 包含一个 excel表格 和 图片,图片文件的物理结构不限.
    /// </summary>
    public class SingleFolderImport
    {
        
        public IList<Product> ProductsPassedDBCheck{ get; private set; }
        public IList<Product> ProductsHasImage { get; private set; }
        public IList<Product> ProductsNotHasImage { get; private set; }
        public IList<Product> ProductsExistedInDB { get; set; }
        public IList<FileInfo> ImagesHasProduct { get; private set; }
        public IList<FileInfo> ImagesNotHasProduct { get; private set; }
        /// <summary>
        /// 包含Excel和图片的 文件夹
        /// </summary>
        public string FolderPath { private get; set; }
        /// <summary>
        /// 是否查询数据库(检查供应商是否存在, 该产品是否已经存在
        /// </summary>
        public bool NeedCheckWithDB { private get; set; }

        public OperationWhenExists OperationWhenExists { private get; set; }
        /// <summary>
        /// 图片另存路径
        /// </summary>
        public string ImageSaveAsPath { private get; set; }
        /// <summary>
        /// 导入过程中的信息
        /// </summary>
        public string ImportMsg { get; private set; }

        private StringBuilder sbImportMsg = new StringBuilder();

        private DirectoryInfo RootDir = null;
        public SingleFolderImport(string folderpath)
        {
            FolderPath = folderpath;
            RootDir = new DirectoryInfo(folderpath);
            ProductsExistedInDB = new List<Product>();
            ProductsHasImage = new List<Product>();
            ProductsNotHasImage = new List<Product>();
            ImagesHasProduct = new List<FileInfo>();
            ImagesNotHasProduct = new List<FileInfo>();
        }
        /// <summary>
        /// 导入总方法
        /// </summary>
        public void Import(BizProduct bizProduct, BizSupplier bizSupplier,IFormatSerialNoPersistent formatSerialnoPersisitent)
        {
            //1 读取Excel列表
            IList<Product> ProductsInExcel = ReadProductsFromExcel(CheckExcelFile());
            //2 检查图片是否存在
            CheckProductImages(ProductsInExcel);
            //检查数据库
            if (NeedCheckWithDB)
            {
                //1 检查 供应商是否存在
                IList<string> supplierNameList = GetSupplierNameList(ProductsInExcel);
                IList<string> supplierNameList_NotExists;
                IList<Supplier> supplierList = bizSupplier.GetListByNameList(supplierNameList, out supplierNameList_NotExists);

                if (supplierNameList_NotExists.Count > 0)
                {
                    foreach (string supplierName in supplierNameList_NotExists)
                    {
                        sbImportMsg.AppendLine("不存在该供应商:" + supplierName);
                    }
                    return;
                }
                else
                { 
                    
                }
                //2 检查数据是否已经导入  && 更新产品的供应商信息
                IList<Product> productsExisted;
                ProductsPassedDBCheck = bizProduct.CheckDB(ProductsHasImage, out productsExisted);
                ProductsExistedInDB = productsExisted;
                foreach (Product productExist in ProductsExistedInDB)
                {
                    sbImportMsg.AppendLine("已存在该产品.供应商/型号:"+productExist.SupplierName+"/"+productExist.ModelNumber);
                }
            }
            //数据保存到数据库-- 分配NTS编码
            FormatSerialNoUnit serialNoMgr=new FormatSerialNoUnit(formatSerialnoPersisitent);

            foreach(Product p in ProductsPassedDBCheck)
            {
                 p.NTSCode=serialNoMgr.GetFormatedSerialNo(p.CategoryCode+"."+p.SupplierCode );
            }
            
            bizProduct.SaveList(ProductsPassedDBCheck);
            serialNoMgr.Save();

            //图片复制至web的虚拟路径,  不合格数据拷贝

        }
      
        private Stream CheckExcelFile()
        {
            DirectoryInfo dir = new DirectoryInfo(FolderPath);
            FileInfo[] excelFiles = dir.GetFiles("*.xls", SearchOption.TopDirectoryOnly);
            if (excelFiles.Length != 1)
            {
                throw new Exception("错误,文件夹 " + FolderPath + " 应该有且仅有一个excel文件");
            }
            FileInfo excelFile = excelFiles[0];
            Stream stream = new FileStream(excelFile.FullName, FileMode.Open);
            sbImportMsg.AppendLine("开始导入:" + excelFile.Name);
            return stream;
        }
        private IList<Product> ReadProductsFromExcel(Stream stream)
        {
            IDataTableConverter<Product> productReader = new ProductDataTableConverter();
            string errMsg;
            DataTable dt = new NLibrary.ReadExcelToDataTable(stream).Read(out errMsg);
            IList<Product> products = productReader.Convert(dt);
            return products;
        }

        private IList<string> GetSupplierNameList(IList<Product> products)
        {
            IList<string> supplierNameList = new List<string>();
            foreach (Product p in products)
            {
                if (!supplierNameList.Contains(p.SupplierName))
                {
                    supplierNameList.Add(p.SupplierName);
                }
            }
            return supplierNameList;
        }

        //产品与excel的对应性.
        //assert:图片和excel在同一个文件夹里.
        private void CheckProductImages(IList<Product> productList)
        {
            FileInfo[] images = RootDir.GetImageFiles(SearchOption.AllDirectories).ToArray<FileInfo>();// dirImage.GetFiles();
            //写一个通用类,比较两个序列,返回匹配结果.
            //Compare<T1,T2>  T1和T2需要实现他们两者比较的接口
            foreach (Product p in productList)
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
                        ProductsHasImage.Add(p);
                        ImagesHasProduct.Add(image);
                        productHasImage = true;
                        break;
                    }
                }
                if (!productHasImage)
                {
                    ProductsNotHasImage.Add(p);
                }
            }
            foreach (FileInfo f in images)
            {
                bool imageHasProduct = false;
                foreach (FileInfo f2 in ImagesHasProduct)
                {
                    if (f.Name.Equals(f2.Name))
                    {
                        imageHasProduct = true;
                        break;
                    }
                }
                if (!imageHasProduct)
                {
                    ImagesNotHasProduct.Add(f);
                }
            }
        }

        /// <summary>
        /// 将结果保存到磁盘:
        /// </summary>
        /// <param name="productHasImages"></param>
        public void HandlerCheckResult(string supplierName, 
            string outputFolder, string WebProductImagesPath)
        {
            DirectoryInfo dirRoot = new DirectoryInfo(outputFolder);
            TransferInDatatable transfer = new TransferInDatatable();
            //如果没有合格数据 则不需要创建
            if (ProductsPassedDBCheck.Count > 0)
            {
                DirectoryInfo dirQuanlified = IOHelper.EnsureDirectory(outputFolder + "合格数据\\");
                DirectoryInfo dirSupplierQuanlified = IOHelper.EnsureDirectory(dirQuanlified.FullName + supplierName + "\\");
                DirectoryInfo dirSupplierQuanlifiedImages = IOHelper.EnsureDirectory(dirSupplierQuanlified.FullName + supplierName + "\\");
                foreach (Product product in ProductsPassedDBCheck)
                {
                    try
                    {
                        FileInfo imageFile = ImagesHasProduct.Single(x => StringHelper.ReplaceSpace(Path.GetFileNameWithoutExtension(x.Name))
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


                DataTable dtProductsHasImage = ObjectConvertor.ToDataTable<Product>(ProductsPassedDBCheck);
                transfer.CreateXslFromDataTable(dtProductsHasImage, 1, dirSupplierQuanlified.FullName + "\\" + supplierName + ".xls");
            }

            //没有图片的产品
            string dirPathNotQuanlified = outputFolder + "不合格数据\\";
            string dirPathSupplierNotQuanlified = dirPathNotQuanlified + supplierName + "\\";
            if (ProductsNotHasImage.Count > 0)
            {
                DirectoryInfo dirSupplierNotQuanlified = IOHelper.EnsureDirectory(dirPathSupplierNotQuanlified);
                DataTable dtProductsNotHasImage = ObjectConvertor.ToDataTable<Product>(ProductsNotHasImage);
                transfer.CreateXslFromDataTable(dtProductsNotHasImage, 1, dirSupplierNotQuanlified + "没有图片的数据_" + supplierName + ".xls");

            }
            //没有产品的图片
            if (ImagesNotHasProduct.Count > 0)
            {

                string dirPathSupplierNotQuanlifiedImages = dirPathSupplierNotQuanlified + "多余图片_" + supplierName + "\\";
                DirectoryInfo dirSupplierNotQuanlifiedImages = IOHelper.EnsureDirectory(dirPathSupplierNotQuanlifiedImages);
                foreach (FileInfo file in ImagesNotHasProduct)
                {
                    file.CopyTo(dirSupplierNotQuanlifiedImages + file.Name, true);
                }
            }
            //多余的图片

            //重复数据
            if (ProductsExistedInDB.Count > 0)
            {
                string dirPathSupplierRepeated = dirPathSupplierNotQuanlified + "数据库内已存在的数据_" + supplierName + "\\";
                DirectoryInfo dirSupplierRepeated = IOHelper.EnsureDirectory(dirPathSupplierRepeated);

                DataTable dtProductsRepeated = ObjectConvertor.ToDataTable<Product>(ProductsExistedInDB);
                transfer.CreateXslFromDataTable(dtProductsRepeated, 1, dirSupplierRepeated.FullName + "\\" + supplierName + ".xls");
            }
        }
    }
}
