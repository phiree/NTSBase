using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NDAL;
namespace NBiz
{
   public class BizProduct:BLLBase<NModel.Product>
   {
       DALProduct dalProduct = new DALProduct();
          
       public void ImportProductFromExcel(System.IO.Stream stream)
       {
           IExcelReader<Product> productReader = new ProductExcelReader();
           ImportToDatabaseFromExcel<Product> importor = new ImportToDatabaseFromExcel<Product>(productReader, dalProduct);
           importor.Import(stream);
       }
       public IList<Product> Search(string supplierName,string model,bool hasPhoto, int pageSize, int pageIndex, out int totalRecord)
       {
           return dalProduct.Search(supplierName,model,hasPhoto, pageSize, pageIndex, out totalRecord);
       }
       public IList<Product> GetListBySupplierName(string supplierName)
       {
           return dalProduct.GetListBySupplier(supplierName);
       }
    }
}
