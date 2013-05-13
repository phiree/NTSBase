using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NModel;
using NBiz;
public partial class Admin_Supplier_SupplierImport : System.Web.UI.Page
{
    BizSupplier bizSupplier = new BizSupplier();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.InnerHtml = string.Empty;
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            string errMsg;
            bizSupplier.ImportSupplierFromExcel(fuSupplier.PostedFile.InputStream,out errMsg);
            lblMsg.Attributes["class"] = "success";
            lblMsg.InnerHtml = errMsg;
        }
        catch (Exception ex)
        {
            lblMsg.Attributes["class"] = "error";
            lblMsg.InnerHtml = ex.Message;
        }


    }
}