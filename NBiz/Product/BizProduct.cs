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
        public string ImportMsg { get; set; }
        public override void SaveList(IList<Product> list)
        {
            StringBuilder sbMsg = new StringBuilder();
            IList<Product> listToBeSaved = new List<Product>();
            sbMsg.AppendLine("-----开始导入.待导入产品数量:" + list.Count + "<br/>");
            //排除已有产品 之前是在dal层实现,应该转移到bll层, 因为nts编码生成也与此相关.
            //已经提取出来的supplier直接获取 不再从数据源提取
            List<Supplier> latestSuppliers=new List<Supplier>();
            
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
                        sbMsg.AppendLine(errMsg + "<br/>");
                        continue;
                    }
                    o.SupplierCode = supplier.Code;
                }
                try
                {
                    var p = dalProduct.GetOneByModelNumberAndSupplier(o.ModelNumber, o.SupplierCode);

                    if (p != null)
                    {
                        string errMsg = "未保存.已存在相同供应商和型号的产品:" + o.Name + "-" + o.SupplierName + "-" + o.ModelNumber;
                        NLibrary.NLogger.Logger.Debug(errMsg);
                        sbMsg.AppendLine(errMsg + "<br/>");
                        continue;

                    }
                }
                catch (Exception ex)
                {
                    NLibrary.NLogger.Logger.Debug(ex.Message);
                    sbMsg.AppendLine(ex.Message + "<br/>");
                    continue;
                }
                o.NTSCode = serialNoUnit.GetFormatedSerialNo(o.CategoryCode + "." + o.SupplierCode);
                listToBeSaved.Add(o);


            }
            sbMsg.AppendLine("----可导入/待导入数量:"+listToBeSaved.Count+"/"+list.Count+"<br/>");
            dalProduct.SaveList(listToBeSaved);
            serialNoUnit.Save();
            sbMsg.AppendLine("----导入完成----:" + listToBeSaved.Count + "<br/>");
            ImportMsg= sbMsg.ToString();

        }
        public void ImportProductFromExcel(System.IO.Stream stream)
        {
            IExcelReader<Product> productReader = new ProductExcelReader();
            ImportToDatabaseFromExcel<Product> importor = new ImportToDatabaseFromExcel<Product>(productReader, this);
            importor.Import(stream);
        }
        public IList<Product> Search(string supplierName, string model, bool hasPhoto, 
            string name,string categorycode,
            int pageSize, int pageIndex, out int totalRecord)
        {
            return dalProduct.Search(supplierName, model, hasPhoto,
                name,categorycode,
                pageSize, pageIndex, out totalRecord);
        }
       


    }
}
