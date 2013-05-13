using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Hql;
using System.Web.Security;

namespace NDAL
{
    public class DalBase<T>
    {

        protected ISession session = new HybridSessionBuilder().GetSession();
        public void Delete(T o)
        {
            session.Delete(o);
            session.Flush();
        }
        public virtual void Save(T o)
        {
            session.Save(o);
            session.Flush();
        }
        
        public virtual void SaveList(IList<T> list)
        {
            
            foreach (T t in list)
            {

                Save(t);
                
            }
           
        }
        public void Update(T o)
        {
            session.Update(o);
            session.Flush();
        }
        public void SaveOrUpdate(T o)
        {

            session.SaveOrUpdate(o);
            session.Flush();

        }
        public T GetOne(object id)
        {
            return session.Get<T>(id);
        }
        protected T GetOneByQuery(IQueryOver<T> queryOver)
        {
            return GetOneByQuery(queryOver.List());
        }
        public T GetOneByQuery(string query)
        {
            return GetOneByQuery(GetList(query));
        }
        private T GetOneByQuery(IList<T> listT)
        {

            if (listT.Count == 1)
            {
                return listT[0];
            }
            else if (listT.Count == 0)
            {
                return default(T);
            }
            else
            {
                string errmsg = "错误:GetOnByQuery应该只能返回一个值.现在有" + listT.Count + "个值返回.";

                NLibrary.NLogger.Logger.Error(errmsg);
                //return listT[0];
                throw new Exception(errmsg);
            }
        }

        public IList<T> GetAll<T>() where T : class
        {
            session.Clear();
            return session.QueryOver<T>().List();
        }

        public IList<T> GetList(string query)
        {
            int totalRecords;
            return GetList(query, 0, 99999, out totalRecords);
        }
        protected IList<T> GetList(IQueryOver<T, T> queryOver)
        {
            return queryOver.List();
        }

        public IList<T> GetList(string query, int pageIndex, int pageSize, out int totalRecords)
        {
            IQuery qry = session.CreateQuery(query);
            var itemList = qry.Future<T>().ToList();
            totalRecords = itemList.Count;
            var returnList = itemList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return returnList;
        }


    }
}
