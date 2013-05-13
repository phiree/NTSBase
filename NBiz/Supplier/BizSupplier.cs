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
       public void ImportSupplierFromExcel(System.IO.Stream stream,out string errmsg)
       {
           IDataTableConverter<Supplier> supplierReader = new SupplierDataConverter();
           ImportToDatabaseFromExcel<Supplier> importor = new ImportToDatabaseFromExcel<Supplier>(supplierReader, this);
           importor.Import(stream,out errmsg);
       }
       public IList<Supplier> ReadSupplierListFromExcel(System.IO.Stream stream, out string errmsg)
       {
           IDataTableConverter<Supplier> supplierReader = new SupplierDataConverter();
           ImportToDatabaseFromExcel<Supplier> importor = new ImportToDatabaseFromExcel<Supplier>(supplierReader, this);
          return importor.ReadList(stream, out errmsg);
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
       
    }
}
