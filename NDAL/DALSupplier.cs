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
                .Where(x => x.Code == o.Code)
                .List();
            //如果只有一個與此相同,則update.
            if (q.Count > 0)
            {
                if (q.Count == 1)
                {
                    NModel.Supplier old = q[0];
                    old.EnglishName = o.EnglishName;
                    old.Name = o.Name;
                    old.ContactPerson = o.ContactPerson;
                    old.Address = o.Address;

                    base.Update(old);
                }
                else
                {
                    string errmsg = "未保存,已存在同名或者代码相同的供应商:"
                        + o.Name + "-" + o.Name + "-" + o.Code;
                    NLibrary.NLogger.Logger.Error(errmsg
                        );
                    throw new Exception(errmsg);
                
                }
                
            }
            else
            {
                base.Save(o);
            }
        }
        public virtual NModel.Supplier GetOneByName(string supplierName)
        {
            NHibernate.IQueryOver<NModel.Supplier> iqueryOver = session.QueryOver<NModel.Supplier>()
                .Where(x => (supplierName!=string.Empty)&&(x.EnglishName==supplierName|| x.Name == supplierName));
            string query =string.Format( "select s  from Supplier s where s.EnglishName='{0}' or s.Name='{0}'",supplierName);
            //GetOneByQuery(query);
            return GetOneByQuery(query);
        }
        public NModel.Supplier GetOneByName(string chinesename, string englishName)
        {
            NHibernate.IQueryOver<NModel.Supplier> iqueryOver = 
                session.QueryOver<NModel.Supplier>()
                .Where(x =>( !string.IsNullOrEmpty(chinesename)&& x.Name == chinesename)
                          ||(!string.IsNullOrEmpty(englishName)&&x.EnglishName==englishName)  );
            return GetOneByQuery(iqueryOver);
        }

        public IList<NModel.Supplier> Search(string supplierName, int pageIndex, int pageSize, out int recordCount)
        {
            string query = "select s from Supplier s  where 1=1 ";
            if (!string.IsNullOrEmpty(supplierName))
            {
                query += "  and s.Name like '%" + supplierName + "%' or s.EnglishName like '%"+supplierName+"%'";

            }
            return GetList(query,pageIndex,pageSize, out recordCount);
        }

    }
}
