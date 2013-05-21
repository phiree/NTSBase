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
            var q = session.QueryOver<Category>().Where(x => x.Name == o.Name)
                   .And(x => x.ParentCode == o.ParentCode)
                   .And(x => x.Code == o.Code)
                   .List();
            if (q.Count > 0)
            {
                string errMsg = "未保存,请修改后重新导入.已存在相同名称/大类编码/分类编码的类别:" + o.Name + "-" + o.ParentCode + "-" + o.Code;
                NLibrary.NLogger.Logger.Debug(errMsg);
                throw new Exception(errMsg);
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
