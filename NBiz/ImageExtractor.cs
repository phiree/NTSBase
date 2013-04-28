using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Text.RegularExpressions;
namespace NBiz
{
    //提取excel里的图片,以指定列的值命名,保存到磁盘
   public class ImageExtractor
    {

       public string ExcelPath = string.Empty;
       /// <summary>
       /// 图片命名
       /// </summary>
       /// <param name="supplierName"></param>
       /// <param name="productType"></param>
       /// <param name="ntsCode"></param>
       public void Excute(string filePath,string savePath)
       {
          
           ProductExcelReader importer = new ProductExcelReader();
           System.Collections.IList allPictures;
           IList<Product> products = importer.Read(new System.IO.FileStream(filePath, System.IO.FileMode.Open), out allPictures);

           if (products.Count != allPictures.Count)
           {
               throw new Exception(string.Format( "提取失败:产品和图片的数量不相等.产品:{0},图片:{1}",products.Count,allPictures.Count));
           }
           for (int i = 0; i < products.Count; i++)
           {
               HSSFPictureData pic = (HSSFPictureData)allPictures[i];
               
               var modelNumber =NLibrary.StringHelper.ReplaceInvalidChaInFileName(products[i].ModelNumber,"$")+".jpg";
               string fileName=savePath+modelNumber;
               NLibrary.IOHelper.EnsureFileDirectory(fileName);
               File.WriteAllBytes(fileName, pic.Data);
                
           }
          
       }
    }
}
