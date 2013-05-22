using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NModel
{
   public class NTSMember
    {
       public NTSMember()
       {
          LastLogin= RegistDate = DateTime.Now;
          LoginCount = 0;
       }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Password { get; set; }
        public virtual string Openid { get; set; }
        public virtual string RealName { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Address { get; set; }
        public virtual string IdCard { get; set; }
        public virtual string Email { get; set; }
        //注册时间
        public virtual DateTime RegistDate { get; set; }
        //登录次数
        public virtual int LoginCount { get; set; }
        //最后一次登录时间
        public virtual DateTime LastLogin { get; set; }
    }
}
