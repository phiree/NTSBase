using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDAL
{
  public  class DALSupplier:DalBase<NModel.Supplier>
    {
      public override void Save(NModel.Supplier o)
      {
          var q = session.QueryOver<NModel.Supplier>()
              .Where(x => x.Name == o.Name||x.Code==o.Code)
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
    }
}
