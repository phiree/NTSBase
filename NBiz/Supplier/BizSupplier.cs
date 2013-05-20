using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NDAL;
namespace NBiz
{
    public class BizSupplier : BLLBase<NModel.Supplier>
    {
        DALSupplier dalSupplier = new DALSupplier();
        public void ImportSupplierFromExcel(System.IO.Stream stream, out string errmsg)
        {
            IDataTableConverter<Supplier> supplierReader = new SupplierDataConverter();
            ImportToDatabaseFromExcel<Supplier> importor = new ImportToDatabaseFromExcel<Supplier>(supplierReader, this);
            importor.ImportXslData(stream, out errmsg);
        }
        public IList<Supplier> ReadSupplierListFromExcel(System.IO.Stream stream, out string errmsg)
        {
            IDataTableConverter<Supplier> supplierReader = new SupplierDataConverter();
            ImportToDatabaseFromExcel<Supplier> importor = new ImportToDatabaseFromExcel<Supplier>(supplierReader, this);
            return importor.ReadList(stream, out errmsg);
        }

        public Supplier GetByCode(string supplierCode)
        {
            string query = "from Supplier s where s.Code='" + supplierCode + "'";
            return GetOneByQuery(query);
        }
        public Supplier GetByName(string supplierName)
        {
            string query = "from Supplier s where s.Name='" + supplierName + "'";
            return GetOneByQuery(query);
        }
        public IList<Supplier> GetListByNameList(IList<string> supplierNameList,out IList<string>  supplierNameListNotExists )
        {
            supplierNameListNotExists = new List<string>();
            IList<Supplier> existsSupplierList = new List<Supplier>();
            foreach (string supplierCode in supplierNameList)
            {
                //如果返回为空
                Supplier supplier = GetByCode(supplierCode);
                if (supplier != null)
                {
                    existsSupplierList.Add(supplier);
                }
                else
                {
                    supplierNameListNotExists.Add(supplierCode);
                }
                
            }
            return existsSupplierList;
        }
        public override IList<Supplier> SaveList(IList<Supplier> list, out string errMsg)
        {
            errMsg = string.Empty;
            dalSupplier.SaveList(list);
            return list;
        }
        public IList<Supplier> GetListAllPaged(int pageIndex, int pageSize, out int totalRerord)
        {
            return dalSupplier.GetList("select s from Supplier s", pageIndex, pageSize, out totalRerord);
        }
        public IList<Supplier> Search(string name, int pageIndex, int pageSize, out int recordCount)
        {
            return dalSupplier.Search(name, pageIndex, pageSize, out recordCount);
        }

    }
}
