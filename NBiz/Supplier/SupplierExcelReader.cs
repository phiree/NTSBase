using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NModel;
namespace NBiz
{
    public class SupplierDataConverter:IDataTableConverter<NModel.Supplier>
    {
        public IList<NModel.Supplier> Convert(DataTable dt)
        {
           
            List<Supplier> SupplierList = new List<Supplier>();
            foreach (DataRow row in dt.Rows)
            {
                Supplier p = new Supplier();
                p.Name = row["供应商名称"].ToString();
                p.Code = row["供应商编码"].ToString();
                p.EnglishName = row["供应商英文名称"].ToString();
                //p.NTSCode=BuildNtsCode(p.CategoryCode,suppl
                SupplierList.Add(p);
            }


            return SupplierList;
        }
    }
}
