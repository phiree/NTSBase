using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace NModel.Mapping
{
    public class ImportOperationLogMap:ClassMap<ImportOperationLog>
    {
        public ImportOperationLogMap()
        {
            Id(x=>x.Id);
            Map(x => x.FinishTime);
            Map(x => x.FileFrom);
            Map(x => x.ImportedFileName);
            HasMany(x => x.ImportedItems).Cascade.SaveUpdate();
            Map(x => x.ImportTime);
            Map(x => x.ImportResult).Length(4000);
            Map(x => x.ImportMember);
            
         
        }
    }
}
