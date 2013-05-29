using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NModel;
using NDAL;
using NLibrary;
namespace NBiz
{
    /// <summary>
    /// 复制产品对应的图片
    /// 如果是为已经导入的产品复制图片则:  图片和产品的关联关系: 图片名称==产品型号,图片所在文件夹名称=供应商名称.
    /// 如果是同时导入excel文件里的产品 和 相关图片, 则 只需要对比 型号
    /// </summary>
    public class ProductImageImporter
    {
         DALSupplier dalSupplier;
         DALProduct dalProduct;
        public DALSupplier DalSupplier
        {
            get
            {
                if (dalSupplier == null) dalSupplier = new DALSupplier();
                return dalSupplier;
            }
            set { dalSupplier = value; }
        }
        public DALProduct DalProduct
        {
            get
            {
                if (dalProduct == null) dalProduct = new DALProduct();
                return dalProduct;
            }
            set { dalProduct = value; }
        }
        /// <summary>
        /// 扫描指定文件夹,根绝文件夹和图片名称提取图片信息
        /// <param name="targetPath">拷贝的目标地址,应该为网站的虚拟目录的物理路径</param>
        /// </summary>
        public IList<ImageInfo> ImportImage(string folderPath, string targetPath, out string message)
        {
            StringBuilder sb = new StringBuilder();
            IList<ImageInfo> images = new List<ImageInfo>();
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            DirectoryInfo[] supplierDirs = dir.GetDirectories();

            //foreach (DirectoryInfo dirSupplier in supplierDirs)
            //{
            //    string supplierNameOfFolder = dirSupplier.Name;
            //    Supplier s = dalSupplier.GetOneByName(supplierNameOfFolder);
            //    if (s == null)
            //    {
            //        string errmsg = "没有找到与文件夹同名的供应商名称,请核查." + supplierNameOfFolder;
            //        NLibrary.NLogger.Logger.Debug(errmsg);
            //        sb.AppendLine(errmsg);
            //        continue;
            //    }
            FileInfo[] imageFiles = dir.GetImageFiles().ToArray();
            if (imageFiles.Length == 0)
            {
                string errmsg = "文件夹内没有图片:" + dir.FullName;
                NLibrary.NLogger.Logger.Debug(errmsg);
                sb.AppendLine(errmsg);

            }
            //本次操作中出现过的供应商
            IList<Supplier> supplierList = new List<Supplier>();
            foreach (FileInfo imageFile in imageFiles)
            {

                string supplierNameOfFolder = imageFile.Directory.Name;
                Supplier s = supplierList.SingleOrDefault(x => x.Name == supplierNameOfFolder || x.EnglishName == supplierNameOfFolder);
                if (s == null)
                {
                    s = DalSupplier.GetOneByName(supplierNameOfFolder);
                    if (s == null)
                    {
                        string errmsg = "没有找到与文件夹同名的供应商名称,请核查." + supplierNameOfFolder;
                        NLibrary.NLogger.Logger.Debug(errmsg);
                        sb.AppendLine(errmsg);
                        continue;
                    }
                    if (!supplierList.Contains(s))
                    {
                        supplierList.Add(s);
                    }
                }
                IList<Product> ProductsOfSupplier = DalProduct.GetListBySupplierCode(s.Code);

                if (ProductsOfSupplier.Count == 0)
                {
                    string errmsg = "没有属于该供应商的产品,请导入对应的报价单 或者 核实文件夹名称与供应商名称的一致性." + supplierNameOfFolder;
                    NLibrary.NLogger.Logger.Debug(errmsg);
                    sb.AppendLine(errmsg);
                    continue;
                }

                //读取型号名称
                string modelNumber = Path.GetFileNameWithoutExtension(imageFile.Name).Replace("＄","$");

                Product p = null;//=  dalProduct.GetOneByModelNumberAndSupplier(modelNumber, dirSupplier.Name);
                IList<Product> productSupplierAndModel = ProductsOfSupplier.Where(x =>  x.ModelNumber.Trim().Replace("\n","") == modelNumber.Trim()).ToList();
                if (productSupplierAndModel.Count == 0)
                {

                    string errmsg = "该图片没有对应的产品信息:" + supplierNameOfFolder + "," + imageFile.Name;
                    NLibrary.NLogger.Logger.Debug(errmsg);
                    sb.AppendLine(errmsg);
                    //    NLibrary.NLogger.Logger.Debug("没有找到对应的图片.供应商:" + dirSupplier.FullName+",型号:"+modelNumber);
                    continue;
                }
                else if (productSupplierAndModel.Count > 1)
                {
                    string errmsg = "该图片对应两条产品信息:" + supplierNameOfFolder + "," + imageFile.Name;
                    NLibrary.NLogger.Logger.Debug(errmsg);
                    sb.AppendLine(errmsg);
                }
                //拷贝图片 到 对应文件夹
                p = productSupplierAndModel[0];
                string newImageName = (p.Name + p.SupplierName + modelNumber).GetHashCode().ToString() + imageFile.Extension;
                System.IO.File.Copy(imageFile.FullName, targetPath + "\\" + newImageName, true);

                if (!p.ProductImageUrls.Contains(newImageName))
                {
                    p.ProductImageUrls.Add(newImageName);

                    DalProduct.Update(p);
                }
                ImageInfo ii = new ImageInfo();
                ii.ImagePath = imageFile.FullName;
                ii.ModelNumber = modelNumber;
                ii.SupplierName = s.Name;
                images.Add(ii);
            }
            message = sb.ToString();
            return images;
        }


    }



    public class ImageInfo
    {
        public string ImagePath { get; set; }
        public string SupplierName { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string ModelNumber { get; set; }

    }
}
