using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NModel
{
    //产品标签-
   public class ProductTag
    {
       public virtual Guid Id { get; set; }
       public virtual string TagName { get; set; }
       public virtual string Description { get; set; }
       public virtual DateTime CreateTime { get; set; }
    }
}
