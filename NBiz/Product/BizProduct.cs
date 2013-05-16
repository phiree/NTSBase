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
        FormatSerialNoUnit serialNoUnit = new FormatSerialNoUnit(new DALFormatSerialNo());
        DALSupplier _dalSupplier;
       public  DALSupplier DalSupplier
        {
            get
            {
                if (_dalSupplier == null)
                {
                    _dalSupplier = new DALSupplier();
                }
                return _dalSupplier;

            }
            set {
                _dalSupplier = value;
            }
        }
        DalBase<Product> dalProduct = new DALProduct();
        
        
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
        public IList<Product> ReadListFromExcelWithAllPictures(System.IO.Stream stream, out string errMsg, out System.Collections.IList allPictures)
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
            totalErrMsg = string.Empty;
            StringBuilder sbMsg = new StringBuilder();


            sbMsg.AppendLine("-----开始保存.产品数量:" + list.Count + "<br/>");
            IList<Product> invalidItems = new List<Product>();
            var listToBeSaved = CheckItemsBeforeSave(list, out invalidItems, out totalErrMsg);

            //排除已有产品 之前是在dal层实现,应该转移到bll层, 因为nts编码生成也与此相关.
            //已经提取出来的supplier直接获取 不再从数据源提取


            sbMsg.AppendLine("---------可导入/待导入数量:" + listToBeSaved.Count + "/" + list.Count + "<br/>");
            ((DALProduct)dalProduct).SaveList(listToBeSaved);
            sbMsg.AppendLine("---------导入完成----:" + listToBeSaved.Count + "<br/>");
            serialNoUnit.Save();
            sbMsg.AppendLine("---------NTS编码已生成----:" + listToBeSaved.Count + "<br/>");
            sbMsg.AppendLine("--数据导入结束----:" + listToBeSaved.Count + "<br/>");
            ImportMsg = sbMsg.ToString();
            return listToBeSaved;

        }
        /// <summary>
        ///  检查产品
        /// 
        ///  该产品是否已经存在.
        /// </summary>
        /// <param name="list">待检查列表</param>
        /// <param name="invalidItems">不合格数据</param>
        /// <param name="outErrMsg">错误信息</param>
        /// <returns>合格数据,可以直接导入</returns>
        public IList<Product> CheckItemsBeforeSave(IList<Product> list, out IList<Product> invalidItems, out string outErrMsg)
        {
            invalidItems = new List<Product>();
            IList<Product> ValidItems = new List<Product>();
            string errMsg = string.Empty;
            StringBuilder sbMsg = new StringBuilder();
            List<Supplier> latestSuppliers = new List<Supplier>();
            foreach (Product o in list)
            {
                if (string.IsNullOrEmpty(o.SupplierCode))
                {
                    Supplier supplier = null;
                    IList<Supplier> suppliersInLatest = latestSuppliers.Where(x => x.Name.ToLower() == o.SupplierName || x.EnglishName.ToLower() == o.SupplierName).ToList();
                    if (suppliersInLatest.Count == 0)
                    {
                        supplier = DalSupplier.GetOneByName(o.SupplierName);
                    }
                    else if (suppliersInLatest.Count == 1)
                    {
                        supplier = suppliersInLatest[0];
                    }
                    else if (suppliersInLatest.Count > 1)
                    {
                        throw new Exception("已经添加多个供应商");
                    }

                    if (supplier == null)
                    {
                        errMsg = "未保存.供应商不存在:" + o.SupplierName + o.Name + "-" + "-" + o.ModelNumber;
                        NLibrary.NLogger.Logger.Debug(errMsg);
                        throw new Exception(errMsg);
                       
                    }
                    else
                    {
                        latestSuppliers.Add(supplier);
                    }
                    o.SupplierCode = supplier.Code;
                }
                try
                {
                    var p = ((DALProduct)DalBase).GetOneByModelNumberAndSupplier(o.ModelNumber, o.SupplierCode);

                    if (p != null)
                    {
                        errMsg = "未保存.已存在相同供应商和型号的产品:" + o.Name + "-" + o.SupplierName + "-" + o.ModelNumber;
                        NLibrary.NLogger.Logger.Debug(errMsg);
                        sbMsg.AppendLine(errMsg + "<br/>");
                        invalidItems.Add(o);
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
                ValidItems.Add(o);
            }
            outErrMsg = sbMsg.ToString();
            return ValidItems;
        }
       
        public IList<Product> Search(string supplierName, string model, bool hasPhoto,
            string name, string categorycode,
            int pageSize, int pageIndex, out int totalRecord)
        {
             return ((DALProduct)dalProduct).Search(supplierName, model, hasPhoto,
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
