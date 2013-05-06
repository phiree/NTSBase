using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace NModel.Mapping
{
    public class FormatSerialNoMap : ClassMap<FormatSerialNo>
    {
        public FormatSerialNoMap()
        {
            Id(x=>x.Id);
          
            Map(x => x.SerialKey).Unique();
            Map(x => x.SerialNo);
        }
    }
}
