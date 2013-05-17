using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace NModel.Mapping
{
    public class ProductMap:ClassMap<Product>
    {
        public ProductMap()
        {
            Id(x=>x.Id);
            Map(x => x.CategoryCode);
            Map(x => x.CreateTime);
            Map(x => x.EnglishName);
            Map(x => x.LastUpdateTime);
            Map(x => x.Memo);
            Map(x => x.ModelNumber).UniqueKey("UN_Product");
            Map(x => x.Name);
            Map(x => x.NTSCode).Unique();
            Map(x => x.OrderAmountMin);
            Map(x => x.PlaceOfDelivery);
            Map(x => x.PlaceOfOrigin);
            Map(x => x.PriceOfFactory);
            Map(x => x.ProductDescription);
            HasMany(x => x.ProductImageUrls).Element("productimageurls").Cascade.AllDeleteOrphan();
            Map(x => x.ProductionCycle);
            Map(x => x.ProductParameters);
            Map(x => x.State).CustomType<int>();
            Map(x => x.SupplierCode).UniqueKey("UN_Product");
            Map(x => x.SupplierName);
            Map(x => x.TaxRate);
            Map(x => x.Unit);
            Map(x => x.PriceDate);
            Map(x => x.PriceValidPeriod);
            Map(x => x.MoneyType);
            Map(x => x.ImageState);
            References<ImportOperationLog>(x => x.ImportOperationLog);
        }
    }
}
