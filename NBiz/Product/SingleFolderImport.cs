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
    /// 处理单个文件夹
    /// </summary>
    public class SingleFolderImport
    {
        //
        public IList<Product> ProductsPassedDBCheck{ get; private set; }
        public IList<Product> ProductsHasImage { get; private set; }
        public IList<Product> ProductsNotHasImage { get; private set; }
        public IList<Product> ProductsExistedInDB { get; set; }
        public IList<FileInfo> ImagesHasProduct { get; private set; }
        public IList<FileInfo> ImagesNotHasProduct { get; private set; }
        /// <summary>
        /// 文件夹路径
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
                //2 检查数据是否已经导入
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

           bizProduct.SaveList(

        }
        public void ImportWithDBCheck(BizProduct bizProduct, BizSupplier bizSupplier)
        {

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
    }
}
