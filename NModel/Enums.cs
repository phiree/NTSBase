using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NModel.Enums
{
    
    public enum ProductState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 过期
        /// </summary>
        Expired,
        /// <summary>
        /// 禁用
        /// </summary>
        Disabled
    }
    /*多語言版本的屬性值*/
    public enum ClassType
    {
        Product,
        Supplier
    }
    //
    public enum PropertyType
    {
        ProductName,
        ProductDescription,
        ProductParameters
    }
    public enum LanguageType
    {
        Chinese,
        English
    }
}
