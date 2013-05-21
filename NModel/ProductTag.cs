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
       public virtual IList<ProductTag_Product> Product_Tags { get; set; }
       public ProductTag()
       {
           Product_Tags = new List<ProductTag_Product>();
       }
       public virtual void AddProduct_Tag(Product p)
       {
           ProductTag_Product ptp = new ProductTag_Product();
           ptp.Tag = this;
           ptp.Product = p;
           Product_Tags.Add(ptp);
       }
    }
}
