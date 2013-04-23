using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NDAL;
using System.IO;
using NLibrary;
namespace NBiz
{
    /// <summary>
    /// 将图片拷贝到目标位置
    /// </summary>
   public  class ImageCopy
    {
       public string TargetPath { get; set; }
       /// <summary>
       ///合法结构:
       /// --所选路径
       ///   |--供应商1
       ///      |--型号1.jpg
       ///   |--供应商2
       ///      |--型号3.jpg
       ///   
       /// </summary>
       public string SourcePath { get; set; }

       IList<Product> allProduct;
       DalBase<Product> dalProduct = new DalBase<Product>();
       public ImageCopy()
       {
           allProduct = dalProduct.GetAll<Product>();
       }
       
       public void Copy()
       {

           DirectoryInfo dirSource = new DirectoryInfo(SourcePath);

           foreach (DirectoryInfo dirSupplier in dirSource.GetDirectories())
           {
               foreach (FileInfo fiModelNumber in dirSupplier.GetFiles())
               {
                   
                   //文件夹名称==供应商, 图片名称(+序列号)==移除特殊字符之后的型号
                   var selectedPList= allProduct.Where(x => x.SupplierName == dirSupplier.Name
                         && fiModelNumber.Name.Contains(System.Text.RegularExpressions.Regex.Replace(x.ModelNumber, "[\\/:*?\"<>|]", "$")))
                         .ToList()
                         ;

                   if (selectedPList.Count==0)
                 {
                     NLogger.Logger.Debug("图片未能对应已有产品:" + fiModelNumber.FullName);
                 }
                 else
                 {
                     Product selectedP = selectedPList[0];
                     string targetFileName=TargetPath + "\\"+dirSupplier.Name+"\\" + selectedP.NTSCode + "\\" + fiModelNumber.Name;
                     IOHelper.EnsureFileDirectory(targetFileName);

                     try
                     {
                         File.Copy(fiModelNumber.FullName, targetFileName,true);
                     }
                     catch (Exception ex)
                     {
                         NLogger.Logger.Error("Exception when Copy File." + fiModelNumber.FullName + "\r\n ex:" + ex.Message);
                     }
                 }
               }
           }
       }
      

    }
}
