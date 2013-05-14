using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NDAL;
using NLibrary;
using System.Data;
namespace NBiz
{
    public class BizProduct : BLLBase<NModel.Product>
    {
        DALProduct dalProduct = new DALProduct();
        FormatSerialNoUnit serialNoUnit = new FormatSerialNoUnit(new DALFormatSerialNo());
        DALSupplier dalSupplier = new DALSupplier();
        public string ImportMsg { get; set; }
        /// <summary>
        /// 导入excel产品列表
        /// </summary>
        /// <param name="stream">excel流</param>
        /// <param name="errMsg"></param>
        public void ImportProductFromExcel(System.IO.Stream stream, out string errMsg)
        {
            IDataTableConverter<Product> productReader = new ProductDataTableConverter();
            ImportToDatabaseFromExcel<Product> importor = new ImportToDatabaseFromExcel<Product>(productReader, this);
            importor.ImportXslData(stream, out  errMsg);
        }
        /// <summary>
        /// 从excel文件中读取产品信息
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public IList<Product> ReadListFromExcel(System.IO.Stream stream, out string errMsg)
        {
            IDataTableConverter<Product> productReader = new ProductDataTableConverter();
            ImportToDatabaseFromExcel<Product> importor = new ImportToDatabaseFromExcel<Product>(productReader, this);
            return importor.ReadList(stream, out  errMsg);
        }
        /// <summary>
        /// 从excel文件中读取产品信息和内嵌图片
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="errMsg"></param>
        /// <param name="allPictures"></param>
        /// <returns></returns>
        public IList<Product> ReadListFromExcelWithAllPictures(System.IO.Stream stream, out string errMsg,out System.Collections.IList allPictures)
        {
            IDataTableConverter<Product> productReader = new ProductDataTableConverter();
            ImportToDatabaseFromExcel<Product> importor = new ImportToDatabaseFromExcel<Product>(productReader, this);
            return importor.ReadListWithAllPictures(stream, out  errMsg, out allPictures);
        }
        /// <summary>
        /// 保存产品列表至数据库
        /// </summary>
        /// <param name="list"></param>
        /// <param name="totalErrMsg"></param>
        /// <returns>成功保存的总数</returns>
        public override IList<Product> SaveList(IList<Product> list, out string totalErrMsg)
        {
            StringBuilder sbMsg = new StringBuilder();
            totalErrMsg = string.Empty;
            IList<Product> listToBeSaved = new List<Product>();
            sbMsg.AppendLine("-----开始保存.产品数量:" + list.Count + "<br/>");
            //排除已有产品 之前是在dal层实现,应该转移到bll层, 因为nts编码生成也与此相关.
            //已经提取出来的supplier直接获取 不再从数据源提取
            List<Supplier> latestSuppliers = new List<Supplier>();
            string errMsg;
            foreach (Product o in list)
            {
                if (string.IsNullOrEmpty(o.SupplierCode))
                {

                    Supplier supplier = dalSupplier.GetOneByName(o.SupplierName);
                    if (supplier == null)
                    {
                        errMsg = "未保存.供应商不存在:" + o.SupplierName + o.Name + "-" + "-" + o.ModelNumber;
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
                        errMsg = "未保存.已存在相同供应商和型号的产品:" + o.Name + "-" + o.SupplierName + "-" + o.ModelNumber;
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
                totalErrMsg = sbMsg.ToString();
                o.NTSCode = serialNoUnit.GetFormatedSerialNo(o.CategoryCode + "." + o.SupplierCode);
                listToBeSaved.Add(o);


            }
            sbMsg.AppendLine("---------可导入/待导入数量:" + listToBeSaved.Count + "/" + list.Count + "<br/>");
            dalProduct.SaveList(listToBeSaved);
          
            sbMsg.AppendLine("---------导入完成----:" + listToBeSaved.Count + "<br/>");
            serialNoUnit.Save();
            sbMsg.AppendLine("---------NTS编码已生成----:" + listToBeSaved.Count + "<br/>");
            sbMsg.AppendLine("--数据导入结束----:" + listToBeSaved.Count + "<br/>");
            
            ImportMsg = sbMsg.ToString();
            return listToBeSaved;

        }
      
        public IList<Product> Search(string supplierName, string model, bool hasPhoto,
            string name, string categorycode,
            int pageSize, int pageIndex, out int totalRecord)
        {
            return dalProduct.Search(supplierName, model, hasPhoto,
                name, categorycode,
                pageSize, pageIndex, out totalRecord);
        }

        /// <summary>
        /// excel和图片同时导入(使用客户端 或者, 先将资料上传至服务器)
        /// </summary>
        /// <param name="supplierFolderPath"></param>
        public void ImportPictures(IList<Product> savedList, string supplierFolderPath)
        { 
            
        }


    }
}
