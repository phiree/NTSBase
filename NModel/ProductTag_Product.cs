using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NModel
{
    //tag和产品的关联类
    public class ProductTag_Product
    {
        public virtual Guid Id { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductTag Tag { get; set; }
    }
}
