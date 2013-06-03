using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
namespace NDAL
{
    public class DALCategory : DalBase<NModel.Category>
    {
        public override void Save(NModel.Category o)
        {
            var q = session.QueryOver<Category>().Where(x => x.ParentCode == o.ParentCode)
                   .And(x => x.Code == o.Code)
                   .List();
            if (q.Count > 0)
            {
                Category cate = q[0];
                cate.Name = o.Name;
                cate.EnglishName = o.EnglishName;
                cate.Memo = o.Memo;
                if (q.Count > 1)
                {
                    NLibrary.NLogger.Logger.Debug("数据重复:"+ o.ParentCode+"."+o.Code);
                }
                base.Update(cate);
               
            }
            else
            {
                base.Save(o);
            }

        }

        public Category GetOneByCode(string code)
        {
            return GetOneByCodes(code, "0");
        }

        public Category GetOneByCodes(string code, string parentCode)
        {
            string query = "select c from Category c  where c.Code='" 
                + code + "' and c.ParentCode='"+parentCode+"'";
            return GetOneByQuery(query);
        }
    }
}
