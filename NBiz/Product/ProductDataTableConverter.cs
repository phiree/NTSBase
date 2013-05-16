using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using System.Data;
using System.Text.RegularExpressions;
using NLibrary;

namespace NBiz
{
    /// <summary>
    /// 应该是 DataTable转成 IList<T>
    /// </summary>
   
    public class ProductDataTableConverter : IDataTableConverter<Product>
    {
        //如果出错,则抛出异常.
        public IList<Product> Convert(DataTable dt)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            IRowPopulate irp = RowPopulateFactory.CreatePopulator(dt);
            foreach (DataColumn col in dt.Columns)
            {
                ColumnNameMatch(dt, col.ColumnName);
            }
            List<Product> productList = new List<Product>();
            string supplierName = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
               
                    Product p = irp.PopulateFromRow(row);
                    supplierName = p.SupplierName;
                    if (productList.Where(x => x.SupplierName == p.SupplierName&&p.ModelNumber==x.ModelNumber).ToArray().Length > 0)
                    {
                        throw new Exception("错误: 供应商名称 和 型号均相同:"+p.SupplierName+","+p.ModelNumber);
                    }
                    productList.Add(p);
               
            }
          
            return productList;
        }
        /// <summary>
        /// 列名容错
        /// </summary>
        private void ColumnNameMatch(DataTable dt, string columnName)
        {
            Dictionary<string, string> columnsEasyToSpellWrong = new Dictionary<string, string>();
            columnsEasyToSpellWrong.Add("生产周期", ".*生产周期.*");
            columnsEasyToSpellWrong.Add("最小起订量", ".*最小起订量.*");
            columnsEasyToSpellWrong.Add("规格参数", ".*规格.*参数.*");
            columnsEasyToSpellWrong.Add("产地", "产地|开票地");
            //英文列名匹配
            //  columnsEasyToSpellWrong.Add("生产周期", ".*生产周期.*");
            // {"*生产周期*","*最小起定量*" };
            foreach (KeyValuePair<string, string> columnNamePatern in columnsEasyToSpellWrong)
            {
                if (Regex.IsMatch(columnName, columnNamePatern.Value))
                {
                    dt.Columns[columnName].ColumnName = columnNamePatern.Key;
                }
            }
        }

    }
}
