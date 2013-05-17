using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OperationLog_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        gv.DataSource = new NBiz.BizImportLog().GetAll<NModel.ImportOperationLog>().OrderByDescending(x=>x.ImportTime);
        gv.DataBind();
    }
}