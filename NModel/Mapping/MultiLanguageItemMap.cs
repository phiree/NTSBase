using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace NModel.Mapping
{
    public class MultiLanguageItemMap : ClassMap<MultiLanguageItem>
    {
        public MultiLanguageItemMap()
        {
            Id(x => x.Id);

            Map(x => x.ClassType).CustomType<int>().UniqueKey("UN_MLItemValue");
            Map(x => x.ItemId).UniqueKey("UN_MLItemValue");
            Map(x => x.ItemValue);
            Map(x => x.Language).CustomType<int>(). UniqueKey("UN_MLItemValue");
            Map(x => x.PropertyType).CustomType<int>().UniqueKey("UN_MLItemValue");


        }
    }
}
