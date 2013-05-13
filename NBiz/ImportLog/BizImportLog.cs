﻿using System;
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
    public class BizImportLog:BLLBase<ImportLog>
    {
        BizProduct bizProduct = new BizProduct();
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="finishTime"></param>
        public void Import(Stream stream, string fileName, DateTime finishTime, string from, string memberName,string importResult)
        {
           ImportLog log = new ImportLog();
            IDataTableConverter<Product> productReader = new ProductDataTableConverter();
            NBiz.ImportToDatabaseFromExcel<Product> importor = new ImportToDatabaseFromExcel<Product>(productReader, bizProduct);
            string errMsg;

            IList<Product> importedProducts = importor.ReadList(stream,out errMsg);

            log.FinishTime = finishTime;
            log.From = from;
            log.ImportedFileName = fileName;
            log.ImportedItems = importedProducts;
            log.ImportMember = memberName;
            log.ImportResult = importResult;

            Save(log);
           //data

        }
    }
}