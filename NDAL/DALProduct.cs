using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
namespace NDAL
{
    public class DALProduct:DalBase<NModel.Product>
    {
        public override void Save(NModel.Product o)
        {
            var q = session.QueryOver<Product>().Where(x => x.Name == o.Name)
                .And(x => x.ModelNumber == o.ModelNumber)
                .And(x => x.SupplierName == o.SupplierName)
                .And(x => x.CategoryCode == o.CategoryCode)
                .List();
            if (q.Count > 0)
            {
                NLibrary.NLogger.Logger.Debug("未保存,已存在该产品:" + o.Name + "-" + o.SupplierName + "-" + o.ModelNumber
                    );
            }
            else
            {
                base.Save(o);
            }
        }
    }
}
