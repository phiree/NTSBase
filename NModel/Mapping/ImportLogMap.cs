using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace NModel.Mapping
{
    public class ImportLogMap:ClassMap<ImportLog>
    {
        public ImportLogMap()
        {
            Id(x=>x.Id);
            Map(x => x.FinishTime);
            Map(x => x.From);
            Map(x => x.ImportedFileName);
            HasMany(x => x.ImportedItems);
            Map(x => x.ImportTime);
            Map(x => x.Log);
            Map(x => x.Operator);
            
         
        }
    }
}
