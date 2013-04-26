using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDAL
{
    public class DALSupplier : DalBase<NModel.Supplier>
    {
        public override void Save(NModel.Supplier o)
        {
            var q = session.QueryOver<NModel.Supplier>()
                .Where(x => x.Name == o.Name || x.Code == o.Code)
                .List();
            if (q.Count > 0)
            {
                NLibrary.NLogger.Logger.Debug("未保存,已存在同名或者代码相同的供应商:" + o.Name + "-" + o.Name + "-" + o.Code
                    );
            }
            else
            {
                base.Save(o);
            }
        }
        public NModel.Supplier GetOneByName(string supplierName)
        {
            NHibernate.IQueryOver<NModel.Supplier> iqueryOver = session.QueryOver<NModel.Supplier>().Where(x => x.Name == supplierName);
            return GetOneByQuery(iqueryOver);
        }

        public IList<NModel.Supplier> Search(string supplierName, int pageIndex, int pageSize, out int recordCount)
        {
            string query = "select s from Supplier s  where 1=1 ";
            if (!string.IsNullOrEmpty(supplierName))
            {
                query += "  and s.Name like '%" + supplierName + "%'";

            }
            return GetList(query,pageIndex,pageSize, out recordCount);
        }

    }
}
