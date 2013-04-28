using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NDAL;
using NLibrary;
namespace NBiz
{
    public class BizProduct : BLLBase<NModel.Product>
    {
        DALProduct dalProduct = new DALProduct();
        FormatSerialNoUnit serialNoUnit = new FormatSerialNoUnit(new DALFormatSerialNo());
        DALSupplier dalSupplier = new DALSupplier();

        public override void SaveList(IList<Product> list)
        {
            IList<Product> listToBeSaved = new List<Product>();
            //排除已有产品 之前是在dal层实现,应该转移到bll层, 因为nts编码生成也与此相关.
            foreach (Product o in list)
            {
                //如果有suppliercode
                if (string.IsNullOrEmpty(o.SupplierCode))
                {
                    Supplier supplier = dalSupplier.GetOneByName(o.SupplierName);
                    if (supplier == null)
                    {
                        string errMsg = "未保存.供应商不存在:" + o.SupplierName + o.Name + "-" + "-" + o.ModelNumber;
                        NLibrary.NLogger.Logger.Debug(errMsg);
                        continue;
                    }
                    o.SupplierCode = supplier.Code;
                }
                
                var p = dalProduct.GetOneByModelNumberAndSupplier(o.ModelNumber, o.SupplierCode);

                if (p != null)
                {
                    string errMsg = "未保存.已存在相同供应商和型号的产品:" + o.Name + "-" + o.SupplierName + "-" + o.ModelNumber;
                    NLibrary.NLogger.Logger.Debug(errMsg);
                    continue;

                }

                o.NTSCode = serialNoUnit.GetFormatedSerialNo(o.CategoryCode + "." + o.SupplierCode);
                listToBeSaved.Add(o);


            }
            dalProduct.SaveList(listToBeSaved);
            serialNoUnit.Save();

        }
        public void ImportProductFromExcel(System.IO.Stream stream)
        {
            IExcelReader<Product> productReader = new ProductExcelReader();
            ImportToDatabaseFromExcel<Product> importor = new ImportToDatabaseFromExcel<Product>(productReader, this);
            importor.Import(stream);
        }
        public IList<Product> Search(string supplierName, string model, bool hasPhoto, int pageSize, int pageIndex, out int totalRecord)
        {
            return dalProduct.Search(supplierName, model, hasPhoto, pageSize, pageIndex, out totalRecord);
        }
        public IList<Product> GetListBySupplierName(string supplierName)
        {
            return dalProduct.GetListBySupplier(supplierName);
        }


    }
}
