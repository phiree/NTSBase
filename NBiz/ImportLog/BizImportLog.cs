using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using System.IO;
namespace NBiz
{
    /// <summary>
    /// 导入日志  记录导入行为的信息
    /// </summary>
    public class BizImportLog:BLLBase<ImportOperationLog>
    {
        BizProduct bizProduct = new BizProduct();
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="finishTime"></param>
        public void Import( string fileName,IList<Product> importedProducts , DateTime finishTime, string from, string memberName,string importResult)
        {
            ImportOperationLog log = new ImportOperationLog();
            log.FinishTime = finishTime;
            log.FileFrom = from;
            log.ImportTime = DateTime.Now;
            log.ImportedFileName = fileName;
          //  log.ImportedItems = importedProducts;
            log.ImportMember = memberName;
            log.ImportResult = importResult;
            Save(log);
           //data
        }
   
        
    }
}
