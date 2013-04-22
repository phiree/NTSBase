using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using System.IO;
using NPOI;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
namespace NBiz
{


    public class ImportFromExcel
    {

        public void Save()
        {

        }

        public IList<Product> ReadProductFromExcel(string xslPath)
        {
            DataTable dt = new TransferInDatatable().CreateFromXsl(xslPath);

            List<Product> productList=new List<Product>();
            foreach (DataRow row in dt.Rows)
            {
                Product p = new Product();

                decimal price = 0;
                string strFactoryPrice = row["出厂价"].ToString();
                if (!string.IsNullOrEmpty(strFactoryPrice))
                {
                    price = decimal.Parse(Regex.Replace(strFactoryPrice, @"[^\d.]", ""));
                }
                string productionCycle = row["生产周期"].ToString();
                
                int 生产周期 = 0;

                if (!string.IsNullOrEmpty(productionCycle))
                {
                    生产周期 = int.Parse(Regex.Replace(productionCycle, @"[^\d.]", ""));
                }
                string strMinOrderAmount = row["最小起订量"].ToString();
                int 最小订货量 = 0;

                if (!string.IsNullOrEmpty(strMinOrderAmount))
                {
                    最小订货量 = int.Parse(Regex.Replace(strMinOrderAmount, @"[^\d.]", ""));
                }

                p.  ModelNumber = row["产品型号"].ToString();
                p.PlaceOfOrigin = row["产地"].ToString();
                p. PlaceOfDelivery = row["交货地"].ToString();
                p.OrderAmountMin = 最小订货量;
                p.Name = row["产品名称"].ToString();
                p.CategoryCode = row["分类编码"].ToString();
                p.PriceOfFactory = strFactoryPrice;
                p.ProductParameters = row["规格/参数"].ToString();
                p.TaxRate = Convert.ToDecimal(row["税率"]);
                p.ProductionCycle = 生产周期;
                p.Unit = row["单位"].ToString();
                p.SupplierName = row["供应商名称"].ToString();
                p.ProductDescription = row["产品描述"].ToString();
                productList.Add(p);
            }


            return productList;
        }

        SerialNumberManager serialNumberManager = new SerialNumberManager();
        public string BuildNtsCode(string catelogCode, string suppierCode, bool isTest)
        {
            string baseNumber = catelogCode + "." + suppierCode;
            //获取当前分类和当前供应商的最大编码
            int serialNumber = serialNumberManager.GetSerialNo(baseNumber, isTest);
            //新编码
            string newSerialNumber = "0000" + serialNumber;
            newSerialNumber = newSerialNumber.Substring(newSerialNumber.Length - 5, 5);
            string ntsCode = baseNumber + newSerialNumber;
            return ntsCode;
        }

    }
}
