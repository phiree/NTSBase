using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NModel
{
    public class Product
    {
        public  Product()
        {
            State = ProductState.Normal;
            Id = Guid.NewGuid();
            CreateTime = LastUpdateTime = DateTime.Now;
        }
        public virtual Guid Id { get; set; }
        public virtual string NTSCode { get; set; }
        public virtual string Name { get; set; }
        /// <summary>
        /// 分类编码,对应excel
        /// </summary>
        public virtual string CategoryCode { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public virtual string ModelNumber { get; set; }
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
        public virtual decimal OrderAmountMin{ get; set; }
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
        public virtual ProductState State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public virtual DateTime LastUpdateTime { get; set; }
   
    }
    public enum ProductState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 过期
        /// </summary>
        Expired,
        /// <summary>
        /// 禁用
        /// </summary>
        Disabled
    }
}
