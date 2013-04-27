using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
namespace NDAL
{
    public class DALProduct : DalBase<NModel.Product>
    {
        public override void SaveList(IList<Product> list)
        {


            base.SaveList(list);
        }
        public override void Save(NModel.Product o)
        {
            var q = session.QueryOver<Product>().Where(x => x.Name == o.Name)
                .And(x => x.ModelNumber == o.ModelNumber)
                .And(x => x.SupplierName == o.SupplierName)
                .And(x => x.CategoryCode == o.CategoryCode)
                .List();
            if (q.Count > 0)
            {
                string errMsg = "未保存.已存在相同供应商和型号的产品:" + o.Name + "-" + o.SupplierName + "-" + o.ModelNumber;
                NLibrary.NLogger.Logger.Debug(errMsg);
                throw new Exception(errMsg);
            }
            else
            {
                base.Save(o);
            }
        }
        public Product GetOneByModelNumberAndSupplier(string modelNumber, string supplierName)
        {
            NHibernate.IQueryOver<Product> iqueryover = session.QueryOver<Product>().Where(x => x.SupplierName == supplierName)
                .And(x => x.ModelNumber == modelNumber);
            //string query = string.Format("from Product p where p.SupplierName='{0}' and p.ModelNumber='{1}'",
            //    supplierName,modelNumber);
            return GetOneByQuery(iqueryover);
        }
        

        /// <summary>
        /// 通用搜索.
        /// </summary>
        /// <param name="supplierName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public IList<Product> Search(string supplierName, string model, bool hasphoto, int pageSize, int pageIndex, out int totalRecord)
        {

            string query = "select p from Product p  where 1=1 ";
            if (!string.IsNullOrEmpty(supplierName))
            {
                query += " and p.SupplierName like '%" + supplierName + "%'";
            }
            if (!string.IsNullOrEmpty(model))
            {
                query += "and p.ModelNumber like '%" + model + "%'";
            }
            if (hasphoto)
            {
                query += "and p.ProductImageUrls.size>0";
            }
            //if (supplierCodes.Length > 0)
            //{
            //    foreach (string su in supplierCodes)
            //    {
            //        whereSupplier += "'" + su + "',";
            //    }
            //    if (!string.IsNullOrEmpty(whereSupplier))
            //    {
            //        whereSupplier = whereSupplier.TrimEnd(',');
            //    }
            //    query += " and p.SupplierCode in (" + whereSupplier + ")";

            //}

            return GetList(query, pageIndex, pageSize, out totalRecord);
        }

        /// <summary>
        /// 精确获取供应商的产品
        /// </summary>
        /// <param name="supplierName"></param>
        /// <returns></returns>
        public IList<Product> GetListBySupplier(string supplierName)
        {
            NHibernate.IQueryOver<Product, Product> queryover = session.QueryOver<Product>().Where(x => x.SupplierName == supplierName);
            return GetList(queryover);
        }
    }
}
