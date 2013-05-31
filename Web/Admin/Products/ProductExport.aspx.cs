using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NBiz;
using NModel;
using NLibrary;
public partial class Admin_Products_ProductExport : System.Web.UI.Page
{
    ProductImagesExport imageExporter = new ProductImagesExport();
    BizProduct bizProduct = new BizProduct();
    IList<Product> productToExport;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private IList<Product> ProductsWithEnglish
    {
        get {
            if (productToExport == null)
            {
                productToExport = bizProduct.GetProducts_English();
            }
                return productToExport;
        }
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {

     
    
        ExcelExport export = new ExcelExport("产品资料" + DateTime.Now.ToString("yyyyMMdd-HHmmss"));
        export.ExportProductExcel(ProductsWithEnglish);
       
        
       


       
    }

  
    protected void btnExportImage_Click(object sender, EventArgs e)
    {

        NLogger.Logger.Debug("--开始导出图片--产品数量"+ProductsWithEnglish.Count);

        imageExporter.Export(ProductsWithEnglish, Server.MapPath("/productImagesExport/") + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "\\",

      Server.MapPath("/ProductImages/original/"), NModel.Enums.ImageOutPutStratage.Category_NTsCode);

        lblMsg.Text = "操作完成. 产品图片已保存于 \\192.168.1.44\\导出图片\\ ";
        NLogger.Logger.Debug("--导出结束--");
    }
}
