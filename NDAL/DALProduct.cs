using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NLibrary;
namespace NDAL
{
    public class DALProduct : DalBase<NModel.Product>
    {

        public override void Save(NModel.Product o)
        {
            var q = session.QueryOver<Product>().Where(x => x.SupplierName == o.SupplierCode)
                .And(x => x.ModelNumber == o.ModelNumber)

                .List();

            if (q.Count > 0)
            {      //如果只有一个 则 更新
                if (q.Count == 1)
                {
                    o.CopyFrom(q[0]);
                    base.Update(o);
                }
                else
                {

                    string errMsg = "未保存.已存在相同供应商和型号的产品:" + o.Name + "-" + o.SupplierName + "-" + o.ModelNumber;
                    NLibrary.NLogger.Logger.Debug(errMsg);
                    throw new Exception(errMsg);
                }
            }
            else
            {
                base.Save(o);
            }
        }
        public virtual Product GetOneByModelNumberAndSupplierCode(string modelNumber, string supplierCode)
        {
            NHibernate.IQueryOver<Product> iqueryover = session.QueryOver<Product>().Where(x => x.SupplierCode == supplierCode)
                .And(x => x.ModelNumber == modelNumber);
            //string query = string.Format("from Product p where p.SupplierName='{0}' and p.ModelNumber='{1}'",
            //    supplierName,modelNumber);
            try
            {
                Product p = GetOneByQuery(iqueryover);

                return p;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "Data:modelnumber:" + modelNumber + "--suppliercode:" + supplierCode);
            }
            // return GetOneByQuery(iqueryover);
        }
        public virtual Product GetOneByModelNumberAndSupplierName(string modelNumber, string suppliername, string supplierEnglishName)
        {
            NHibernate.IQueryOver<Product> iqueryover = session.QueryOver<Product>().Where(x => x.ModelNumber == modelNumber)
                 .And(x => x.SupplierName == suppliername || x.SupplierName == supplierEnglishName);
            //  string query = string.Format("from Product p where p.SupplierName='{0}' and p.ModelNumber='{1}'",
            //    supplierName,modelNumber);
            try
            {
                Product p = GetOneByQuery(iqueryover);

                return p;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "Data:modelnumber:" + modelNumber + "--supplierName:" + suppliername);
            }
        }

        /// <summary>
        /// 通用搜索.
        /// </summary>
        /// <param name="supplierName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public IList<Product> Search(string supplierName, string model, bool? hasphoto,
            string name, string categorycode,
            string ntsCode,
            int pageSize, int pageIndex, out int totalRecord)
        {

            string query = "select p from Product p  where 1=1 ";
            if (!string.IsNullOrEmpty(ntsCode))
            {
                query += " and p.NTSCode like '%" + ntsCode + "%'";
            }
            if (!string.IsNullOrEmpty(supplierName))
            {
                query += " and p.SupplierName like '%" + supplierName + "%'";
            }
            if (!string.IsNullOrEmpty(model))
            {
                query += "and p.ModelNumber like '%" + model + "%'";
            }
            if ( hasphoto.HasValue)
            {
                if (hasphoto.Value == true)
                {
                    query += "and p.ProductImageUrls.size>0";
                }
                else
                {
                    query += "and p.ProductImageUrls.size=0";
                }
            }
            if (!string.IsNullOrEmpty(name))
            {
                query += string.Format(" and (p.Name like '%{0}%'  or p.EnglishName like '%{0}%')", name);
            }
            if (!string.IsNullOrEmpty(categorycode))
            {
                //02 或者 02.001
                query += string.Format(" and (p.CategoryCode='{0}' or substring(p.CategoryCode,1,2)='{0}')", categorycode);
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
        public IList<Product> GetListBySupplier(string supplierName, string englishName)
        {
            NHibernate.IQueryOver<Product, Product> queryover = session.QueryOver<Product>()
                .Where(x => x.SupplierName == supplierName || x.SupplierName == englishName)

                ;
            return GetList(queryover);
        }

        public virtual IList<Product> GetListBySupplierCode(string supplierCode)
        {
            NHibernate.IQueryOver<Product, Product> queryover = session.QueryOver<Product>()
                .Where(x => x.SupplierCode == supplierCode);
           //var list= session.QueryOver<Product>().Where(x => x.SupplierCode == supplierCode);

           //string qry = "select p from Product p where p.SupplierCode='" + supplierCode + "'";
           //return GetList(qry);

            return GetList(queryover);
        }

    }
}
