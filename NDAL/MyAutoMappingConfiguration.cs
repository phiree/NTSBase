﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;

namespace NDAL
{
    public class MyAutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        
        public override bool ShouldMap(Type type)
        {
            return type.Namespace == "NModel";
            
        }
    }
}
