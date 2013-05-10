using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using System.IO;
namespace NBiz
{
    /// <summary>
    /// 导入日志
    /// </summary>
    public class BizImportLog
    {
        BizProduct bizProduct = new BizProduct();
        IExcelReader<Product> productReader = new ProductExcelReader();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="finishTime"></param>
        public void Import(Stream stream, string fileName, DateTime finishTime)
        {
            ImportLog log = new ImportLog();
            IList<Product> productList = productReader.Read(stream);
            
            
           
           //data

        }
    }
}
