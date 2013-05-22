using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
namespace NModel.Mapping
{
    public class NTSMemberMap:ClassMap<NTSMember>
    {
        public NTSMemberMap()
        {
            Id(x=>x.Id);
          
            Map(x => x.Address);
            Map(x => x.Email);
            Map(x => x.IdCard);
            Map(x => x.LastLogin);
            Map(x => x.LoginCount);
            Map(x => x.Name).Unique();
            Map(x => x.Openid);
            Map(x => x.Password);
            Map(x => x.Phone);
            Map(x => x.RealName);
            Map(x => x.RegistDate);
        }
    }
}
