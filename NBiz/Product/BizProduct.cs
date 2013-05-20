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
        public DALSupplier DalSupplier
        {
            get
            {
                if (_dalSupplier == null)
                {
                    _dalSupplier = new DALSupplier();
                }
                return _dalSupplier;

            }
            set
            {
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
            var listToBeSaved = CheckDB(list, out invalidItems);

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
        ///   该产品是否已经存在.
        /// </summary>
        /// <param name="list">待检查列表</param>
        /// <param name="invalidItems">不合格数据</param>
        /// <param name="outErrMsg">错误信息</param>
        /// <returns>合格数据,可以直接导入</returns>
        public IList<Product> CheckDB(IList<Product> list, out IList<Product> existedItems)
        {
            existedItems = new List<Product>();
            IList<Product> ValidItems = new List<Product>();//没有重复的产品

            foreach (Product o in list)
            {
                var p = ((DALProduct)dalProduct).GetOneByModelNumberAndSupplierCode(o.ModelNumber, o.SupplierCode);

                if (p != null)
                {
                    existedItems.Add(o);
                    continue;
                }
                ValidItems.Add(o);
            }
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


        public IList<Product> GetListByProvidedModelNumberSupplierNameList(string providedList)
        {
            throw new NotImplementedException();
        }



    }
}
