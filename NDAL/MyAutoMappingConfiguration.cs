using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace NDAL
{
    public class MyAutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        
        public override bool ShouldMap(Type type)
        {
            return type.Namespace == "NModel";
            
        }
    }
    public class DefaultStringLengthConvention
  : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            instance.Length(2000);
        }
    }
}
