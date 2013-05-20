using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace NModel.Mapping
{
    public class ProductTag_ProductMap : ClassMap<ProductTag_Product>
    {
        public ProductTag_ProductMap()
        {
            Id(x=>x.Id);
            References<Product>(x => x.Product);
            References<ProductTag>(x => x.Tag);
        }
    }
}
