using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NModel
{
   public class Supplier
    {
       public virtual Guid Id { get; set; }
       /// <summary>
       /// 名称
       /// </summary>
       public virtual string Name { get; set; }
       public virtual string EnglishName { get; set; }
       /// <summary>
       /// 代码
       /// </summary>
       public virtual string Code { get; set; }
       /// <summary>
       /// 地址
       /// </summary>
       public virtual string Address { get; set; }
       /// <summary>
       /// 联系人
       /// </summary>
       public virtual string ContactPerson{ get; set; }
       /// <summary>
       /// 电话号码
       /// </summary>
       public virtual string Phone{ get; set; }

      
    }
}
