using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Globalization;
using System.Text.RegularExpressions;
using NLibrary;
using NPOI.SS.UserModel;
namespace NLibrary
{
    /// <summary>
    /// 通过表格形式的Excel流生成DataTable.
    ///  第一行有效数据应该是 列名 类型.
    ///  
    /// </summary>
    public class ReadExcelToDataTable
    {
        /// <summary>
        /// 是否需要获取excel文件内的所有图片
        /// </summary>
        public bool NeedGetAllPictures { get; set; }
        /// <summary>
        /// 只创建表结构,不填充数据
        /// </summary>
        public bool OnlyCreateSchemar { get; set; }
        /// <summary>
        /// 从此行开始读取.
        /// </summary>
        public int RowNumberBegin { get; set; }
        /// <summary>
        /// 该文件内所有的图片
        /// </summary>
        public IList AllPictures { get; private set; }
        /// <summary>
        /// worksheet index, start with 0
        /// </summary>
        public int SheetIndex { get; set; }

        Stream InputStream { get; set; } //excel 流
        
        
        public ReadExcelToDataTable(Stream inputStream)
        {
            InputStream = inputStream;
            NeedGetAllPictures = false;
            OnlyCreateSchemar = false;
            RowNumberBegin = 1;
            SheetIndex = 0;
        }
        public ReadExcelToDataTable(Stream inputStream,bool needAllPictures
            ,bool onlyCreateSchemar,int rowNumberBegin):this(inputStream)
        {
            NeedGetAllPictures = needAllPictures;
            OnlyCreateSchemar = onlyCreateSchemar;
            RowNumberBegin = rowNumberBegin;
        }
        public DataTable Read(out string errMsg)
        {
            StringBuilder sbErrMsg = new StringBuilder();
            HSSFWorkbook book = new HSSFWorkbook(InputStream);
            if (NeedGetAllPictures)
            {
                 AllPictures = book.GetAllPictures();
            }
            var sheet = book.GetSheetAt(SheetIndex);
            DataTable dt = new DataTable();
            //起始行单元格内的值作为datatable的列名.
            var row = sheet.GetRow(RowNumberBegin);
            for (int i = 0; i < row.LastCellNum; i++)
            {
                var columnName = row.GetCell(i);
                //空白列导致出错
                string strColName = string.Empty;
                if (columnName == null)
                {
                    strColName = Guid.NewGuid().ToString();
                }
                else
                {
                    strColName = columnName.ToString();
                }
                DataColumn col = new DataColumn(strColName, typeof(String));
                dt.Columns.Add(col);
            }
            if (!OnlyCreateSchemar)
            {
                IEnumerator rowEnumer = sheet.GetRowEnumerator();
                while (rowEnumer.MoveNext())
                {

                    var currentRow = (HSSFRow)rowEnumer.Current;
                    if (currentRow.RowNum < RowNumberBegin + 1) continue;
                    //防止其遍历到没有数据的row
                    if (currentRow.LastCellNum < row.Cells.Count)
                    {
                        //    break;
                    }
                    //空白行判断:如果所有的cell都没有可见数据,则为空白行.
                    bool isEnd = true;
                    foreach (var cell in currentRow.Cells)
                    {
                        if (!string.IsNullOrEmpty(NLibrary.StringHelper.ReplaceSpace(cell.ToString())))
                        {
                            isEnd = false;
                            break;
                        }
                    }
                    if (isEnd) { 
                        
                        break; }

                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        var cell = currentRow.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            errMsg = sbErrMsg.ToString();
            return dt;
        }

       


    }
}
