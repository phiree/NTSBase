using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace NModel.Mapping
{
    public class ProductTagMap:ClassMap<ProductTag>
    {
        public ProductTagMap()
        {
            Id(x=>x.Id);
            Map(x => x.CreateTime);
            Map(x => x.Description);
            Map(x => x.TagName);
            
        }
    }
}
