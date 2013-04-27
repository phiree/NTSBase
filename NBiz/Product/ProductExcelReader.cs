using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using System.Data;
using System.Text.RegularExpressions;
using NLibrary;
namespace NBiz
{


    public class ProductExcelReader : IExcelReader<Product>
    {
        BLLBase<Supplier> bllSupplier = new BLLBase<Supplier>();
        SerialNumberManager snm = new SerialNumberManager();
        StringBuilder sbReadMsg = new StringBuilder();
     
        public IList<Product> Read(System.IO.Stream stream)
        {
            DataTable dt = new TransferInDatatable().CreateFromXsl(stream);
           
            foreach (DataColumn col in dt.Columns)
            {
                ColumnNameMatch(dt, col.ColumnName);
            }
            List<Product> productList = new List<Product>();
            foreach (DataRow row in dt.Rows)
            {
                Product p = PopulateRowToProduct(row);
                productList.Add(p);
            }
            
            return productList;
        }

        private Product PopulateRowToProduct(DataRow row)
        {

            Product p = new Product();
            p.PlaceOfOrigin = row["产地"].ToString();
            p.PlaceOfDelivery = row["交货地"].ToString();
            p.Name = row["产品名称"].ToString();
            //分类编码:非空,格式正确
            string categoryCode = StringHelper.ReplaceSpace(row["分类编码"].ToString());
            if (string.IsNullOrEmpty(categoryCode))
            {
                string errmsg = string.Format("分类编码不能为空.名称:{0}", p.Name);
                NLibrary.NLogger.Logger.Error(errmsg);
                throw new Exception(errmsg);
            }
            string categoryCodePatern = @"\d{2}\.\d{3}";
            if (!Regex.IsMatch(categoryCode, categoryCodePatern))
            {
                string errmsg = string.Format("分类编码格式有误.名称:{0},编码:{1}", p.Name, categoryCode);
                NLibrary.NLogger.Logger.Error(errmsg);
                throw new Exception(errmsg);
            }
            p.CategoryCode = categoryCode;
            //产品型号:
            string modelNumber = row["产品型号"].ToString();
            p.ModelNumber = modelNumber;
            p.ProductParameters = row["规格参数"].ToString();
            p.Unit = row["单位"].ToString();
            p.SupplierName = row["供应商名称"].ToString();
            p.ProductDescription = row["产品描述"].ToString();
            //nts编码
            //已删除,该类不负责nts编码的创建
            
            //税率
            string strRate = row["税率"].ToString();
            strRate = strRate.Replace("%", "");
            if (!string.IsNullOrEmpty(strRate))
            {
                p.TaxRate = Convert.ToDecimal(strRate);
            }

            //出厂价
            decimal price = 0;
            string strFactoryPrice = row["出厂价"].ToString();
            if (!string.IsNullOrEmpty(strFactoryPrice))
            {
                try
                {
                    price = decimal.Parse(Regex.Replace(strFactoryPrice, @"[^\d.]", ""));
                }
                catch
                {
                    throw new Exception(string.Format("出厂价数据格式有误.供应商:{0},产品型号:{1}",
                                    p.SupplierName, p.ModelNumber
                            ));
                }
            }
            p.PriceOfFactory = strFactoryPrice;
            //生产周期
            string productionCycle = row["生产周期"].ToString();
            int 生产周期 = 0;
            if (!string.IsNullOrEmpty(productionCycle))
            {
                生产周期 = int.Parse(Regex.Replace(productionCycle, @"[^\d.]", ""));
            }
            p.ProductionCycle = 生产周期;
            //最小订货量
            string strMinOrderAmount = row["最小起订量"].ToString();
            int 最小订货量 = 0;
            if (!string.IsNullOrEmpty(strMinOrderAmount))
            {
                if (!int.TryParse(Regex.Replace(strMinOrderAmount, @"[^\d.]", ""), out 最小订货量))
                {
                    NLibrary.NLogger.Logger.Debug(
                        string.Format(@"最小起定量数据格式异常,已设置为0.供应商:{0},产品型号:{1}"
                       , p.SupplierName, p.ModelNumber
                        ));
                }
            }
            p.OrderAmountMin = 最小订货量;
            return p;
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
