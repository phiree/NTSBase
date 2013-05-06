using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace NModel.Mapping
{
    public class SupplierMap : ClassMap<Supplier>
    {
        public SupplierMap()
        {
            Id(x=>x.Id);
          
            Map(x => x.Address);
            Map(x => x.Code).Unique();
            Map(x => x.EnglishName);
            Map(x => x.ContactPerson);
            Map(x => x.Name);
            Map(x => x.Phone);

        }
    }
}
