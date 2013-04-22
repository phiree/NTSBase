using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;

using System.IO;
namespace ETBiz
{
    /// <summary>
    /// excel转换: 改变列的名称/增加修改列
    /// </summary>
    public class Transferer
    {
        public void Transfer()
        {
            string oldExcelPath = string.Empty;

            /*erp标准格式*/
            string xslmodelPath = string.Empty;
            FileStream fsModel = new FileStream(xslmodelPath, FileMode.Open);
            HSSFWorkbook xslmodel = new HSSFWorkbook(fsModel);
            var modelSheet = xslmodel.GetSheetAt(0);
            /*现有的格式*/
            using (FileStream fs = new FileStream(oldExcelPath, FileMode.Open))
            {
                HSSFWorkbook oldbook = new HSSFWorkbook(fs);
                var firstSheet = oldbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = firstSheet.GetRowEnumerator();
                //遍历每一行
                while (rows.MoveNext())
                {
                    HSSFRow row = (HSSFRow)rows.Current;
                    //TODO::Create DataTable row

                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        HSSFCell cell = (HSSFCell)row.GetCell(i);
                        FillIntoModel((HSSFSheet)modelSheet, cell);
                    }
                }
            }

        }
        //将现有字段值填入新格式的xsl中
        private void FillIntoModel(HSSFSheet modelSheet, HSSFCell cell)
        {
            int rowIndex = cell.RowIndex;

        }
        public void CreateNewExcelFromXSL()
        {


        }
    }
}
