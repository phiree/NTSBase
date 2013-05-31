using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel.Enums;
using System.ComponentModel;
using System.IO;
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
        //en,zh,fr,de等 多语言实现: 同一个表,语言类型字段分辨~
        public virtual string Language { get; set; }
        public virtual Guid Id { get; set; }
        [Description("NTS编码")]
        public virtual string NTSCode { get; set; }
        public virtual string EnglishName { get; set; }
        /// <summary>
        /// 供应商代码
        /// </summary>
        /// 
        public virtual string SupplierCode { get; set; }
        [Description("图片")]
        public virtual string ImageState { get; set; }
        [Description("报价日期")]
        public virtual string PriceDate { get; set; }
        [Description("报价有效期")]
        public virtual string PriceValidPeriod { get; set; }

        [Description("供应商名称")]
        public virtual string SupplierName { get; set; }
        [Description("产品型号")]
        public virtual string ModelNumber { get; set; }
        [Description("产品名称")]
        public virtual string Name { get; set; }
        [Description("单位")]
        public virtual string Unit { get; set; }
        [Description("分类编码")]
        public virtual string CategoryCode { get; set; }
        [Description("规格参数")]
        public virtual string ProductParameters { get; set; }

        [Description("产地")]
        public virtual string PlaceOfOrigin { get; set; }
        [Description("交货地")]
        public virtual string PlaceOfDelivery { get; set; }


        [Description("出厂价")]
        public virtual string PriceOfFactory { get; set; }
        [Description("币别")]
        public virtual string MoneyType { get; set; }
        [Description("税率")]
        public virtual decimal TaxRate { get; set; }
        [Description("最小起定量")]
        public virtual decimal OrderAmountMin { get; set; }
        [Description("生产周期")]
        public virtual decimal ProductionCycle { get; set; }

        [Description("产品描述")]
        public virtual string ProductDescription { get; set; }
        [Description("备注")]
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
        /// <summary>
        /// 币种
        /// </summary>

        public virtual IList<string> ProductImageUrls { get; set; }

        /// <summary>
        /// 导入日志
        /// </summary>
        public virtual ImportOperationLog ImportOperationLog { get; set; }
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
        /// 为图片生成对应名称
        /// </summary>
        /// <param name="extensionWithDot">后缀名(带.)</param>
        /// <returns></returns>
        public virtual string BuildImageNameNoExtension(string extensionWithDot)
        {
            return (Name + SupplierName + ModelNumber).GetHashCode().ToString() + extensionWithDot;
        }
        public virtual void CopyFrom(Product newProduct)
        {
            this.CategoryCode = newProduct.CategoryCode;
            this.LastUpdateTime = DateTime.Now;
            this.EnglishName = newProduct.EnglishName;
            this.ImageState = newProduct.ImageState;
            this.Memo = newProduct.Memo;
            this.ModelNumber = newProduct.ModelNumber;
            this.MoneyType = newProduct.MoneyType;
            this.Name = newProduct.Name;
            this.OrderAmountMin = newProduct.OrderAmountMin;
            this.PlaceOfDelivery = newProduct.PlaceOfDelivery;
            this.PlaceOfOrigin = newProduct.PlaceOfOrigin;
            this.PriceDate = newProduct.PriceDate;
            this.PriceOfFactory = newProduct.PriceOfFactory;
            this.PriceValidPeriod = newProduct.PriceValidPeriod;
            this.ProductDescription = newProduct.ProductDescription;
            this.ProductionCycle = newProduct.ProductionCycle;
            this.ProductParameters = newProduct.ProductParameters;
            this.State = newProduct.State;
            this.SupplierCode = newProduct.SupplierCode;
            this.SupplierName = newProduct.SupplierName;
            this.TaxRate = newProduct.TaxRate;
            this.Unit = newProduct.Unit;
            this.ValuesOfMultiLanguage = newProduct.ValuesOfMultiLanguage;

        }

        /// <summary>
        /// 为导出的图片生成路径(目前只支持一张图片)
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="imageOutPutStratage"></param>
        /// <returns> 
        ///  key:原图片名称
        ///  value: 每一级文件夹名称.+ 目标文件名称
        /// </returns>
        public virtual Stack<string> BuildImageOutputName(Enums.ImageOutPutStratage imageOutPutStratage)
        {
            Stack<string> imagesToExport = new Stack<string>();
            string imageUrl = string.Empty;
            int imageCount = ProductImageUrls.Count;
            if (imageCount == 0) return imagesToExport;

            imageUrl = ProductImageUrls[0];

            string imageExtension = Path.GetExtension(imageUrl);

            string targetFileName = string.Empty;
            switch (imageOutPutStratage)
            {
                case ImageOutPutStratage.Category_NTsCode:
                    string nameee = NTSCode;
                    if (string.IsNullOrEmpty(NTSCode))
                        nameee = Guid.NewGuid().ToString();
                    imagesToExport.Push(nameee + imageExtension);//文件名称
                    imagesToExport.Push(CategoryCode);

                    break;
                case ImageOutPutStratage.SupplierName_ModelNumber:
                    imagesToExport.Push(ModelNumber + imageExtension);//文件名称
                    imagesToExport.Push(SupplierName);
                    break;
                default: throw new Exception("No Such Stratage");
            }
            return imagesToExport;

        }
    }

}
