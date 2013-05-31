using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NLibrary;
using NBiz;
using System.Data;
using NPOI.HSSF.UserModel;
namespace NBiz
{
    /// <summary>
    /// 产品导出
    /// </summary>
    public class ProductImagesExport
    {
        BizCategory bizCate = new BizCategory();
        public void Export(IList<Product> products, string rootPathExport,string rootPathOriginal,  NModel.Enums.ImageOutPutStratage stratage)
        {

            List<ImageExportModel> images = new List<ImageExportModel>();
            foreach (Product p in products)
            {
                if (p.ProductImageUrls.Count == 0) return;
                Stack<string> pathStacks = p.BuildImageOutputName(stratage);
                if (pathStacks.Count == 0) return;
                string pathFromStack = string.Empty;//根据stack中的节点值 构建路径.
                switch (stratage)
                {
                    case NModel.Enums.ImageOutPutStratage.Category_NTsCode:
                        //获取分类的名称

                        pathFromStack = bizCate.GetCateName(pathStacks.Pop());

                        break;
                    case NModel.Enums.ImageOutPutStratage.SupplierName_ModelNumber:
                        pathFromStack = pathStacks.Pop();
                        break;
                }
                string imageFileNew = pathStacks.Pop();
                string fullPath = rootPathExport + pathFromStack + "\\" + imageFileNew;
                ImageExportModel iem =
                    new ImageExportModel 
                    {
                        ImageName =rootPathOriginal+ p.ProductImageUrls[0]
                        , TargetImageFullName = fullPath };
                images.Add(iem);
            }
            NLibrary.NLogger.Logger.Debug("待拷贝图片数量" + images.Count);
            foreach (ImageExportModel iem in images)
            {
                IOHelper.EnsureFileDirectory(iem.TargetImageFullName);
                System.IO.File.Copy(iem.ImageName, iem.TargetImageFullName, true);
            }
            
        }

        
    }
    public class ImageExportModel
    {
        public string ImageName { get; set; }
        public string TargetImageFullName { get; set; }
    }
}
