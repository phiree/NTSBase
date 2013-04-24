using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NDAL;
namespace NBiz
{
   public class BizSupplier:BLLBase<NModel.Supplier>
    {
       public void ImportSupplierFromExcel(System.IO.Stream stream)
       {
           IExcelReader<Supplier> supplierReader = new SupplierExcelReader();
           DalBase<Supplier> dalSupplier = new DALSupplier();
           ImportToDatabaseFromExcel<Supplier> importor = new ImportToDatabaseFromExcel<Supplier>(supplierReader, dalSupplier);
           importor.Import(stream);
       }
    }
}
