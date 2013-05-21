using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NLibrary;
namespace NBiz
{
    public class StringPopulateFactory
    {
        public static IStringPopulate CreateInstance(string type)
        {
            switch (type)
            {
                case "sm":
                    return new StringPopulateSupplierAndModelNumber();

                default: throw new Exception("No Type Defined");
            }
        }
    }
    /// <summary>
    /// 供应商名称  型号
    /// 供应商名称  型号
    /// </summary>
    public class StringPopulateSupplierAndModelNumber : IStringPopulate
    {
        public Product Populate(string strProperties,BizProduct bizProduct)
        {
            string[] pros =strProperties.Split(new string[]{ "---"}, StringSplitOptions.None);
            if (pros.Length != 2)
            {
                throw new Exception("格式有误:" + strProperties);
            }

            Product p = bizProduct.GetOneBySupplierNameModelNumber(pros[0].Trim(), pros[1].Trim());
            return p;

        }
    }
    public interface IStringPopulate
    {

        Product Populate(string strProperties,BizProduct bizProduct);
    }


}
