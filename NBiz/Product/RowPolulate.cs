using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using System.Data;
using NLibrary;
using System.Text.RegularExpressions;
namespace NBiz
{
    public class RowPopulateFactory
    {
        public static IRowPopulate CreatePopulator(DataTable dt, string type)
        {
            switch (type.ToLower())
            {
                case "epr": return new RowPolulateErp();
                case "baojia": return new RowPolulateBaojiandan();
                case "bianmabiao": return new RowPopulateCodeTable();
                default: throw new Exception("请指定一种类型");
            }
        }
        public static IRowPopulate CreatePopulator(DataTable dt)
        {
            if (dt.Columns.Contains("编码日期"))
            {
                return new RowPopulateCodeTable();
            }

            if (dt.Columns.Count > 25)
                return new RowPolulateErp();
            else return new RowPolulateBaojiandan();
        }
    }
    public class RowPopulateCodeTable : IRowPopulate
    {

        public Product PopulateFromRow(DataRow row)
        {
            Product p = new Product();
            p.SupplierName = row["供应商名称"].ToString();
            p.ModelNumber = row["产品型号"].ToString();
            p.NTSCode = row["总编码"].ToString();
            return p;
        }

        public Product PopulateFromRowWithLanguage(DataRow row, NModel.Enums.LanguageType lang)
        {
            throw new NotImplementedException();
        }
    }
    public class RowPolulateBaojiandan : IRowPopulate
    {
        public Product PopulateFromRow(DataRow row)
        {
            StringBuilder sb = new StringBuilder();
            Product p = new Product();
            p.PlaceOfOrigin = row["产地"].ToString();
            p.PlaceOfDelivery = row["交货地"].ToString();
            p.ImageState = row["图片"].ToString();
            //p.Memo = row["备注"].ToString();
            p.PriceDate = row["报价日期"].ToString();
            p.PriceValidPeriod = row["报价有效期"].ToString();
            p.Name = row["产品名称"].ToString();
            //分类编码:非空,格式正确
            string categoryCode = StringHelper.ReplaceSpace(row["分类编码"].ToString());
            if (string.IsNullOrEmpty(categoryCode))
            {
                string errmsg = string.Format("分类编码不能为空.可能原因:1)分类编码为空 2)有空白行.供应商名称:{0}", p.SupplierName);
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
            p.SupplierName = StringHelper.ReplaceInvalidChaInFileName(row["供应商名称"].ToString());
            p.ProductDescription = row["产品描述"].ToString();
            p.MoneyType = row["币别"].ToString();
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


        public Product PopulateFromRowWithLanguage(DataRow row, NModel.Enums.LanguageType lang)
        {
            throw new NotImplementedException();
        }
    }
    public class RowPolulateErp : IRowPopulate
    {

        public Product PopulateFromRow(DataRow row)
        {
            Product p = new Product();
            p.PlaceOfOrigin = row["备注"].ToString();
            // p.PlaceOfDelivery = row["交货地"].ToString();
            p.Name = row["名称"].ToString();

            string categoryCode = StringHelper.ReplaceSpace(row["代码"].ToString());

            if (string.IsNullOrEmpty(p.PlaceOfOrigin) && string.IsNullOrEmpty(p.Name)
                && string.IsNullOrEmpty(categoryCode))
            {
                throw new Exception("未导入.请确认非内容单元格已彻底清除.");
            }
            //erp的代码包含了供应商和流水号,应该去除
            if (string.IsNullOrEmpty(categoryCode))
            {
                string errmsg = string.Format("分类编码不能为空.名称:{0}", p.Name);
                NLibrary.NLogger.Logger.Error(errmsg);
                throw new Exception(errmsg);
            }

            p.CategoryCode = categoryCode.Substring(0, 6);
            //产品型号:
            string modelNumber = row["产品型号"].ToString();
            p.ModelNumber = modelNumber;
            p.ProductParameters = row["规格型号"].ToString();
            p.Unit = row["计量单位组_FName"].ToString();
            p.SupplierName = row["来源_FName"].ToString();
            p.SupplierCode = row["来源_FNumber"].ToString();
            p.ProductDescription = row["描述/卖点"].ToString();
            //nts编码
            //已删除,该类不负责nts编码的创建

            //税率
            string strRate = row["税率(%)"].ToString();
            strRate = strRate.Replace("%", "");
            if (!string.IsNullOrEmpty(strRate))
            {
                p.TaxRate = Convert.ToDecimal(strRate);
            }

            //出厂价
            decimal price = 0;
            string strFactoryPrice = row["含税出厂价"].ToString();
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
            string productionCycle = row["固定提前期"].ToString();
            int 生产周期 = 0;
            if (!string.IsNullOrEmpty(productionCycle))
            {
                生产周期 = int.Parse(Regex.Replace(productionCycle, @"[^\d.]", ""));
            }
            p.ProductionCycle = 生产周期;
            //最小订货量
            string strMinOrderAmount = row["最小订货量"].ToString();
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


        public Product PopulateFromRowWithLanguage(DataRow row, NModel.Enums.LanguageType lang)
        {
            throw new NotImplementedException();
        }
    }
    public interface IRowPopulate
    {
        Product PopulateFromRow(DataRow row);
        Product PopulateFromRowWithLanguage(DataRow row, NModel.Enums.LanguageType lang);
    }
}
