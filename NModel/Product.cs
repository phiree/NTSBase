using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel.Enums;
namespace NModel
{
    public class Product
    {
        public Product()
        {
            State = ProductState.Normal;
            Id = Guid.NewGuid();
            CreateTime = LastUpdateTime = DateTime.Now;
            ProductImageUrls = new List<string>();
        }
        private IList<MultiLanguageItem> ValuesOfMultiLanguage { get; set; }


        public virtual Guid Id { get; set; }
        public virtual string NTSCode { get; set; }
        public virtual string Name { get; set; }
        public virtual string EnglishName { get; set; }
        /// <summary>
        /// 相應語種的值
        /// </summary>
        /// <param name="lt">語種枚舉</param>
        /// <returns></returns>
        public virtual string GetName(LanguageType lt)
        {
            IList<MultiLanguageItem> items = ValuesOfMultiLanguage.Where(x => x.ClassType == ClassType.Product
                 && x.ItemId == this.Id.ToString() && x.Language == lt && x.PropertyType == PropertyType.ProductName).ToList();
            if (items.Count == 1)
            {
                return items[0].ItemValue;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 分类编码,对应excel
        /// </summary>
        public virtual string CategoryCode { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public virtual string ModelNumber { get; set; }
        /// <summary>
        /// 移除特殊字符之后的型号
        /// </summary>
        //public virtual string ModelNumber_Code
        //{
        //    get
        //    {
        //        return System.Text.RegularExpressions.Regex.Replace(ModelNumber, "[\\/:*?\"<>|]", "$");
        //    }
        //}
        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual string SupplierName { get; set; }
        /// <summary>
        /// 供应商代码
        /// </summary>
        public virtual string SupplierCode { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public virtual string Unit { get; set; }
        /// <summary>
        /// 规格参数
        /// </summary>
        public virtual string ProductParameters { get; set; }
        /// <summary>
        /// 产地
        /// </summary>
        public virtual string PlaceOfOrigin { get; set; }
        /// <summary>
        /// 交货地
        /// </summary>
        public virtual string PlaceOfDelivery { get; set; }
        /// <summary>
        /// 出厂价
        /// </summary>
        public virtual string PriceOfFactory { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public virtual decimal TaxRate { get; set; }
        /// <summary>
        /// 最小起定量
        /// </summary>
        public virtual decimal OrderAmountMin { get; set; }
        /// <summary>
        /// 生产周期(天)
        /// </summary>
        public virtual decimal ProductionCycle { get; set; }
        /// <summary>
        /// chanpi描述
        /// </summary>
        public virtual string ProductDescription { get; set; }
        public virtual string Memo { get; set; }
        /// <summary>
        /// 产品状态
        /// </summary>
        public virtual Enums.ProductState State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public virtual DateTime LastUpdateTime { get; set; }

        public virtual IList<string> ProductImageUrls { get; set; }



    }

}
