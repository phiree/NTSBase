using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NModel
{
   public class ImportLog
    {
       public virtual Guid Id { get; set; }
       public virtual string ImportedFileName { get; set; }
       /// <summary>
       /// 该表格完成时间
       /// </summary>
       public virtual DateTime FinishTime { get; set; }
       /// <summary>
       ///导入时间
       /// </summary>
       public virtual DateTime ImportTime { get; set; }
       /// <summary>
       /// 导入详情
       /// </summary>
       public virtual string ImportResult { get; set; }
       /// <summary>
       /// 该文件来自哪里(部门 或者 员工,木卫六 或者 奥特星云)
       /// </summary>
       public virtual string From { get; set; }
       public IList<Product> ImportedItems { get; set; }
       /// <summary>
       /// 操作员名称
       /// </summary>
       public string ImportMember { get; set; }

       public ImportLog()
       {
           ImportedItems = new List<Product>();
       }
    }
}
