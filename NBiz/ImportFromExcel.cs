using System;
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
using NLibrary;
namespace NBiz
{    /// <summary>
    /// 读取excel,保存到数据库,泛型类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImportToDatabaseFromExcel<T>
    {
        IDataTableConverter<T> datatableConverter;
        BLLBase<T> bll;
        public ImportToDatabaseFromExcel(IDataTableConverter<T> datatableConverter, BLLBase<T> bll)
        {
            this.datatableConverter = datatableConverter;
            this.bll = bll;
        }
        public IList<T> ReadList(Stream stream, out string msg)
        {

            ReadExcelToDataTable excelToDatatableReader = new ReadExcelToDataTable(stream);
            DataTable dt = excelToDatatableReader.Read(out msg);
            IList<T> list = datatableConverter.Convert(dt);
            return list;

        }
        public IList<T> ReadListWithAllPictures(Stream stream, out string msg, out IList allPictures)
        {

            ReadExcelToDataTable excelToDatatableReader = new ReadExcelToDataTable(stream, true, false, 1);
            DataTable dt = excelToDatatableReader.Read(out msg);
            allPictures = excelToDatatableReader.AllPictures;

            IList<T> list = datatableConverter.Convert(dt);
            return list;

        }
        public IList<T> ImportXslData(Stream stream, out string importMsg)
        {
            string excelReadMsg, dataSaveMsg;
            IList<T> list = ReadList(stream, out excelReadMsg);
            IList<T> savedList = bll.SaveList(list, out dataSaveMsg);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--------Excel文件读取----------");
            sb.AppendLine(excelReadMsg);
            sb.AppendLine("--------数据保存----------");
            sb.AppendLine(dataSaveMsg);
            sb.AppendLine("-----Finished----------");
            importMsg = sb.ToString();
            return savedList;
        }
    }




}
