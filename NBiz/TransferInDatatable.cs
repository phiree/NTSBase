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
namespace NBiz
{
    /// <summary>
    /// 将excel文件导入 datatable,再通过sql来转换
    /// </summary>
    public class TransferInDatatable
    {
        // SerialNumberManager serialNumberManager = new SerialNumberManager();

        DataSet ds = new DataSet();

        public void Transfer(string xslBaojiandan, bool needSaveSerialNo)
        {
            DataTable dt = JoinXslToDataTable(xslBaojiandan, !needSaveSerialNo);
            CreateXslFromDataTable(dt, needSaveSerialNo);
        }

        /// <summary>
        /// 根据产品报价单和供应商编码生成物料表
        /// </summary>
        /// <param name="xslPathBaojia">报价单</param>
        /// <param name="xslPathBianma">编码表</param>
        /// <param name="xslPathSupplier">供应商列表</param>
        /// <param name="xslPathErp">标准erp物料表</param>
        public DataTable JoinXslToDataTable(string xslPathBaojia, bool istest)
        {
            FileStream fsBaojia = new FileStream(xslPathBaojia, FileMode.Open);
            DataTable dtBaoJia = CreateFromXsl(fsBaojia);
            Console.WriteLine("报价单行数:" + dtBaoJia.Rows.Count);
            FileStream fsErp = new FileStream(GlobalVariables.ErpXslFileTemplate, FileMode.Open);

            DataTable dtErp = CreateFromXsl(fsErp);
            FileStream fsSupplier = new FileStream(GlobalVariables.XslSupplierList, FileMode.Open);

            DataTable dtSupplier = CreateFromXsl(fsSupplier);
            Console.WriteLine("供应商数量:" + dtSupplier.Rows.Count);

            DataSet ds = new DataSet();
            ds.Tables.Add(dtBaoJia);
            var results = from ttBaojia in dtBaoJia.AsEnumerable()
                          join ttSupplier in dtSupplier.AsEnumerable()
                          on ttBaojia.Field<string>("供应商名称") equals ttSupplier.Field<string>("供应商名称")
                          select new
                          {
                              分类编码 = ttBaojia.Field<string>("分类编码"),
                              名称 = ttBaojia.Field<string>("产品名称"),
                              明细 = "TRUE",
                              物料型号 = ttBaojia.Field<string>("产品型号"),
                              基本计量单位_FName = ttBaojia.Field<string>("单位"),
                              规格型号 = ttBaojia.Field<string>("规格/参数"),
                              备注 = ttBaojia.Field<string>("产地"),
                              出厂价 = ttBaojia.Field<string>("出厂价"),
                              税率 = ttBaojia.Field<string>("税率"),
                              最小订货量 = ttBaojia.Field<string>("最小起订量"),
                              固定提前期 = ttBaojia.Field<string>("生产周期"),
                              产品描述 = ttBaojia.Field<string>("产品描述"),
                              来源_FName = ttSupplier.Field<string>("供应商名称"),
                              来源_FNumber = ttSupplier.Field<string>("供应商编码")
                          };

            foreach (var r in results)
            {
                DataRow newRow = dtErp.NewRow();

                string[] cate = r.分类编码.Split('.');
                string categoryLevel1, categoryLevel2;
                if (cate.Length != 2)
                {
                    throw new Exception("分类编码格式有误:" + r.分类编码);
                }
                categoryLevel1 = cate[0];
                categoryLevel2 = cate[1];
                //   newRow["代码"] = serialNumberManager.GetFormatedSerialNo(r.分类编码, r.来源_FNumber, istest);
                newRow["备注"] = r.备注;
                newRow["描述/卖点"] = r.产品描述;

                newRow["规格型号"] = r.规格型号;
                Console.WriteLine(r.出厂价);

                decimal price = 0;
                if (!string.IsNullOrEmpty(r.出厂价))
                {
                    price = decimal.Parse(Regex.Replace(r.出厂价, @"[^\d.]", ""));
                }
                int 生产周期 = 0;

                if (!string.IsNullOrEmpty(r.固定提前期))
                {
                    生产周期 = int.Parse(Regex.Replace(r.固定提前期, @"[^\d.]", ""));
                }
                int 最小订货量 = 0;

                if (!string.IsNullOrEmpty(r.最小订货量))
                {
                    最小订货量 = int.Parse(Regex.Replace(r.最小订货量, @"[^\d.]", ""));
                }
                newRow["固定提前期"] = 生产周期;
                newRow["采购单价"] = price;// decimal.Parse(r.含税出厂价, NumberStyles.AllowCurrencySymbol | NumberStyles.Number); //r.含税出厂价;
                newRow["基本计量单位_FName"] = r.基本计量单位_FName;
                newRow["来源_FName"] = r.来源_FName;
                newRow["来源_FNumber"] = r.来源_FNumber;
                newRow["名称"] = r.名称;
                newRow["明细"] = r.明细;
                newRow["税率(%)"] = r.税率;
                newRow["物料型号"] = r.物料型号;
                newRow["最小订货量"] = 最小订货量;
                newRow["最大订货量"] = 99999;

                newRow["物料属性_FName"] = "外购";
                newRow["物料分类_FName"] = "主推商品";
                newRow["计量单位组_FName"] = "数量组";

                newRow["基本计量单位_FGroupName"] = "数量组";
                newRow["基本计量单位_FName"] = "个";

                newRow["采购计量单位_FName"] = "个";
                newRow["采购计量单位_FGroupName"] = "数量组";
                newRow["销售计量单位_FName"] = "个";
                newRow["销售计量单位_FGroupName"] = "数量组";
                newRow["生产计量单位_FName"] = "个";
                newRow["生产计量单位_FGroupName"] = "数量组";
                newRow["库存计量单位_FName"] = "个";
                newRow["库存计量单位_FGroupName"] = "数量组";

                newRow["辅助计量单位换算率"] = "0";
                newRow["默认仓库_FName"] = "一号仓";
                newRow["默认仓库_FNumber"] = "0001";
                newRow["默认仓位_FName"] = "*";
                newRow["默认仓位_FGroupName"] = "*";

                newRow["数量精度"] = "4";
                newRow["最低存量"] = "1";
                newRow["最高存量"] = "11000";
                newRow["安全库存数量"] = "2";
                newRow["使用状态_FName"] = "使用";
                newRow["是否为设备"] = "False";

                newRow["存货科目代码_FNumber"] = "1001";
                newRow["销售收入科目代码_FNumber"] = "1001";
                newRow["销售成本科目代码_FNumber"] = "1001";
                newRow["计划模式_FName"] = "MTS计划模式";
                newRow["计价方法_FName"] = "加权平均法";
                newRow["变动提前期批量"] = "1";
                newRow["看板容量"] = "1";
                newRow["单价精度"] = "2";
                newRow["变动提前期"] = "0";
                newRow["标准加工批量"] = "1";

                dtErp.Rows.Add(newRow);
            }
            return dtErp;

        }
        /// <summary>
        /// 为每个xsl生成数据表
        /// </summary>
        /// <param name="xslPath"></param>
        /// <returns></returns>
        public DataTable CreateFromXsl(Stream xslFileStream)
        {
            return CreateFromXsl(xslFileStream, false);
        }
        public DataTable CreateFromXsl(Stream xslFileStream, bool onlyCreateSchedule)
        {
            IList allPic = new List<HSSFPictureData>();
            return CreateFromXsl(xslFileStream, onlyCreateSchedule, out allPic);

        }
 
        public DataTable CreateFromXsl(Stream xslFileStream, bool onlyCreateSchedule, out IList allPictures)
        {
            return CreateFromXsl(xslFileStream, 1, onlyCreateSchedule, out allPictures);
        }
        /// <summary>
        /// 读取excel内容,填入DataTable
        /// </summary>
        /// <param name="startRowIndex">起始行,从0开始.上面的row忽略.</param>
        /// <param name="xslFileStream">Excel文件流</param>
        /// <param name="onlyCreateSchedule">只创建结构,不填充数据.</param>
        /// <param name="allPictures">excel文件内的所图片</param>
        /// <returns></returns>
        public DataTable CreateFromXsl(Stream xslFileStream, int startRowIndex, bool onlyCreateSchedule, out IList allPictures)
        {

            HSSFWorkbook book = new HSSFWorkbook(xslFileStream);
            allPictures = book.GetAllPictures();

            var sheet = book.GetSheetAt(0);
            DataTable dt = new DataTable();
            //起始行单元格内的值作为datatable的列名.
            var row = sheet.GetRow(startRowIndex);
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
            if (!onlyCreateSchedule)
            {
                IEnumerator rowEnumer = sheet.GetRowEnumerator();
                while (rowEnumer.MoveNext())
                {

                    var currentRow = (HSSFRow)rowEnumer.Current;
                    if (currentRow.RowNum < startRowIndex + 1) continue;
                    //防止其遍历到没有数据的row
                    if (currentRow.LastCellNum < row.Cells.Count)
                    {
                        //    break;
                    }
                    //空白行判断
                    bool isEnd = true;
                    foreach (var cell in currentRow.Cells)
                    {
                        if (!string.IsNullOrEmpty( NLibrary.StringHelper.ReplaceSpace(cell.StringCellValue)))
                        {
                            isEnd = false;
                            break;
                        }
                    }
                    if (isEnd) break;                                                             

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

            return dt;
        }




        public void CreateXslFromDataTable(DataTable dt)
        {
            CreateXslFromDataTable(dt, false);
        }
        /// <summary>
        /// 将结果导出成erp标准xsl表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="xslPath"></param>
        /// <param name="outXslPath"></param>
        /// <param name="saveNtsNumber">将最大的nts编码保存到物理文件</param>
        public void CreateXslFromDataTable(DataTable dt, bool saveNtsNumber)
        {
            FileStream fs = new FileStream(GlobalVariables.ErpXslFileTemplate, FileMode.Open);
            HSSFWorkbook book = new HSSFWorkbook(fs);
            fs.Close();
            var sheet = book.GetSheetAt(0);
            var firstRow = sheet.GetRow(0);
            var columnNameOfXsl = firstRow.Cells;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var xslRow = sheet.CreateRow(i + 1);
                for (int j = 0; j < row.ItemArray.Length; j++)
                {
                    foreach (var cell in columnNameOfXsl)
                    {
                        if (cell.ToString() == dt.Columns[j].ColumnName)
                        {
                            xslRow.CreateCell(cell.ColumnIndex).SetCellValue(row.ItemArray[j].ToString());
                            break;
                        }
                    }
                    // 
                }

            }
            string outXlsFilePath = GlobalVariables.ErpXslFileOutTest;
            if (saveNtsNumber)
            {
                outXlsFilePath = GlobalVariables.ErpXslFileOut;
            }

            IOHelper.EnsureFileDirectory(outXlsFilePath);
            FileStream file = new FileStream(outXlsFilePath, FileMode.Create);

            book.Write(file);
            file.Close();
            //保存每个分类的最后编码
            if (saveNtsNumber)
            {
                // serialNumberManager.WriteSerialNumberFile();
            }
        }

        /// <summary>
        /// 根据datatable
        /// </summary>
        /// <param name="dataStartRowNumber">Excel文件顶部可能需要写入其他数据</param>
        /// <param name="dt"></param>
        /// <param name="xslTemplate"></param>
        /// <param name="saveNtsNumber"></param>
        /// <param name="savePath">保存位置</param>
        public void CreateXslFromDataTable(DataTable dt,int dataStartRowNumber,string savePath)
        {
            if(dataStartRowNumber<0)
                throw new Exception("dataStartRowNumber必须大于等于0");
            HSSFWorkbook book = new HSSFWorkbook();
            var sheet = book.CreateSheet("产品报价单");
            DataColumnCollection cols=dt.Columns;
            //创建表头
            for(int h=0;h<=dataStartRowNumber;h++)
            {
                IRow headrow = sheet.CreateRow(h);
                if (h == dataStartRowNumber)
                {
                    CreateCellForRow(headrow, cols, null, true);
                }
                else
                {
                    CreateCellForRow(headrow, cols, null, false);
                }

            }
            //填充内容
            for (int i=0;i<dt.Rows.Count;i++)
            {
                var dataRow=dt.Rows[i];
              var excelRow=   sheet.CreateRow(dataStartRowNumber+1 + i);
              CreateCellForRow(excelRow, cols, dataRow, false);
            }
            //物理保存
            IOHelper.EnsureFile(savePath);
            FileStream fsOut=new FileStream(savePath,FileMode.Create);
            book.Write(fsOut);
            fsOut.Close();
            
        }

        /// <summary>
        /// 根据datarow 创建 cells. 
        /// </summary>
        /// <param name="excelRow"></param>
        /// <param name="columns"></param>
        /// <param name="row">如果为null 则该行所有cell的值为空</param>
        /// <param name="isHead">如果是true 则创建表头.</param>
        private void CreateCellForRow(IRow excelRow, DataColumnCollection columns, DataRow row,bool isHead)
        {
            for (int i=0;i<columns.Count;i++)
            {
                var cell = excelRow.CreateCell(i);
                if (isHead)
                {
                    cell.SetCellValue(columns[i].ColumnName);
                }
                else
                {
                    string cellValue = string.Empty;
                    if (row != null) { cellValue = row[i].ToString(); }
                    cell.SetCellValue(cellValue);
                }
            }
        }
    }
}
