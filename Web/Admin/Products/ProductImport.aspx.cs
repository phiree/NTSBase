using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NBiz;
public partial class Admin_Products_ProductImport : System.Web.UI.Page
{
    NBiz.ImportFromExcel bizImport = new ImportFromExcel();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        bizImport.ImportToDatabase(tbxProductExcel.Text.Trim());

    }
}