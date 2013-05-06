using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace NModel.Mapping
{
    public class CategoryMap:ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x=>x.Id);
          
            Map(x => x.Memo);
            Map(x => x.Code).UniqueKey("UN_CateCode");

            Map(x => x.EnglishName);
            Map(x => x.ParentCode).UniqueKey("UN_CateCode");
            Map(x => x.Name);

        }
    }
}
