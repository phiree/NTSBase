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
       DALSupplier dalSupplier = new DALSupplier();
      public void ImportSupplierFromExcel(System.IO.Stream stream)
       {
           IExcelReader<Supplier> supplierReader = new SupplierExcelReader();
           ImportToDatabaseFromExcel<Supplier> importor = new ImportToDatabaseFromExcel<Supplier>(supplierReader, this);
           importor.Import(stream);
       }
      public override void SaveList(IList<Supplier> list)
      {
          dalSupplier.SaveList(list);
      }
       public IList<Supplier> GetListAllPaged(int pageIndex, int pageSize, out int totalRerord)
       {
           return dalSupplier.GetList("select s from Supplier s", pageIndex, pageSize, out totalRerord);
       }
       public IList<Supplier> Search(string name,int pageIndex,int pageSize,out int recordCount)
       {
           return dalSupplier.Search(name, pageIndex, pageSize, out recordCount);
       }
       public Supplier GetListBySupplierName(string supplierName, string englishName)
       {
           return dalSupplier.GetOneByName(supplierName, englishName);
       }
      
    }
}
