using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NModel
{
   public class Category
    {
       public virtual Guid Id { get; set; }
       /// <summary>
       /// 父类id
       /// </summary>
       public virtual Guid ParentId { get; set; }
       public virtual string ParentCode { get; set; }
       /// <summary>
       /// 代码
       /// </summary>
       public virtual string Code { get; set; }
       public virtual string Name { get; set; }
       public virtual string EnglishName { get; set; }
       //备注
       public virtual string Memo { get; set; }
    }
}
