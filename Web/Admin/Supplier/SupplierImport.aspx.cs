using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Supplier_SupplierImport : System.Web.UI.Page
{
    NBiz.BizSupplier bizImport = new NBiz.BizSupplier();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.InnerHtml = string.Empty;
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            bizImport.ImportSupplierFromExcel(fuSupplier.PostedFile.InputStream);
            lblMsg.Attributes["class"] = "success";
            lblMsg.InnerHtml = "导入成功";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes["class"] = "error";
            lblMsg.InnerHtml = ex.Message;
        }


    }
}