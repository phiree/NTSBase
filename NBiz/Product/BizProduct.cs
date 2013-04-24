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
       public void ImportProductFromExcel(System.IO.Stream stream)
       {
           IExcelReader<Product> productReader = new ProductExcelReader();
           DalBase<Product> dalProduct = new DALProduct();
           ImportToDatabaseFromExcel<Product> importor = new ImportToDatabaseFromExcel<Product>(productReader, dalProduct);
           importor.Import(stream);
       }
    }
}
