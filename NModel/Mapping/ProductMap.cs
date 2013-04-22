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
        }
    }
}
