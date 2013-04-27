﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using System.IO;
using NPOI;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using NDAL;
namespace NBiz
{
    /// <summary>
    /// 读取excel,保存到数据库,泛型类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImportToDatabaseFromExcel<T>
    {
        IExcelReader<T> excelReader;
        DalBase<T> dal;
        public ImportToDatabaseFromExcel(IExcelReader<T> Reader,DalBase<T> dal)
        { 
          this.excelReader=Reader;
          this.dal = dal;
        }
        public void Import(Stream stream)
        {
            IList<T> list = excelReader.Read(stream);
           
            dal.SaveList(list);
        }
    }

    public interface IExcelReader<T>
    {
        IList<T> Read(Stream stream);
    }
}
