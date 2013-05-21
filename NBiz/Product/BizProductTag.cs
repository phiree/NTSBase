using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using System.Data;
using NLibrary;
using System.Text.RegularExpressions;
namespace NBiz
{
    public class BizProductTag:BLLBase<ProductTag>
    {

        public ProductTag Save(string name, string description,IList<Product> products)
        {
            ProductTag tag = new ProductTag();
            tag.CreateTime = DateTime.Now;
            tag.Description = description;
            tag.TagName = name;
            foreach (Product p in products)
            {
                tag.AddProduct_Tag(p);
            }
            Save(tag);
            return tag;
        }
    }
 
}
