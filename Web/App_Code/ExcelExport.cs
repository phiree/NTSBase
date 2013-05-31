using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NModel;
using NBiz;
using NLibrary;
using System.Data;
using NPOI.HSSF.UserModel;
using System.IO;
/// <summary>
///ExcelExport 的摘要说明
/// </summary>
public class ExcelExport
{
    string saveFileName;
	public ExcelExport(string saveFileName)
	{
        this.saveFileName=saveFileName;
	}
    public void ExportProductExcel(IList<Product> products)
    {
        DataTable dt = ObjectConvertor.ToDataTable<Product>(products);
        ExportProductExcel(dt);
    }
    public void ExportProductExcel(DataTable dt)
    {
        TransferInDatatable tt = new TransferInDatatable();
        HSSFWorkbook book = tt.CreateXslWorkBookFromDataTable(dt);
        DownLoadXslFile(book);
    }
    private void DownLoadXslFile(HSSFWorkbook workbook)
    {
        // Save the Excel spreadsheet to a MemoryStream and return it to the client
        using (var exportData = new MemoryStream())
        {
            HttpResponse Response = HttpContext.Current.Response;
            workbook.Write(exportData);
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveFileName));
            //Response.Clear();
            Response.BinaryWrite(exportData.GetBuffer());
           // Response.End();
        }
    }
}