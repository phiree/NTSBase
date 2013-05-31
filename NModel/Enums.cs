using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NModel.Enums
{
    /// <summary>
    /// 产品状态
    /// </summary>
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

    /// <summary>
    /// 数据库内 已存在数据  操作
    /// 
    /// </summary>
    public enum OperationWhenExists
    {
        update,//更新数据
        skip,//跳过
        throwException//抛出错误
    }
    //图片导出的结构
    public enum ImageOutPutStratage
    { 
        SupplierName_ModelNumber,// 供应商/型号.扩展名
        Category_NTsCode,//分类/ntscode.扩展名
    }
}
