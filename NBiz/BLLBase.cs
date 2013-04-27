using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDAL;
namespace NBiz
{
    public class BLLBase<T>
    {
        //todo: bll factory using cache.
        DalBase<T> dalBase;
        public DalBase<T> DalBase
        {
            get
            {
                if (dalBase == null)
                    dalBase = new DalBase<T>();
                return dalBase;
            }
            set
            {
                dalBase = value;
            }
        }
        public T GetOne(object id)
        {
            return DalBase.GetOne(id);
        }
        public T GetOneByQuery(string query)
        {
            return DalBase.GetOneByQuery(query);
        }

        public void Delete(T t)
        {
            DalBase.Delete(t);
        }
        public void SaveOrUpdate(T t)
        {
            DalBase.SaveOrUpdate(t);
        }
        public void Save(T t)
        {
            DalBase.Save(t);
        }
        public virtual void SaveList(IList<T> list) {
            DalBase.SaveList(list);
        }
        public IList<T> GetAll<T>() where T : class
        {
            return DalBase.GetAll<T>();
        }
        protected IList<T> GetList(string where)
        {
            return DalBase.GetList(where);
        }
        protected IList<T> GetList(string where, int pageIndex, int pageSize, out int totalRecord)
        {
            return DalBase.GetList(where, pageIndex, pageSize, out totalRecord);
        }
    }
    //to be used for bll factory
    public enum BLLName
    {
        BLLQZSpringPartner
    }
}
