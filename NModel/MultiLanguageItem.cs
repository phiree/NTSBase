using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel.Enums;
namespace NModel
{
    /// <summary>
    /// 拥有多语言值的条目
    /// </summary>
    public class MultiLanguageItem
    {
        public virtual Guid Id { get; set; }
        public virtual ClassType ClassType { get; set; }
        public virtual PropertyType PropertyType { get; set; }
        public virtual LanguageType Language { get; set; }
        //对应条目的ID.如果 classtype为 product,则 该值则为 product.Id
        public virtual string ItemId { get; set; }
        public virtual string ItemValue { get; set; }

    }

    
}
