using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NBiz;
using NModel;
public partial class Admin_Category_Default : System.Web.UI.Page
{
    BizCategory bizCategory = new BizCategory();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }
    private void Bind()
    {
        gv.DataSource = bizCategory.GetAll<NModel.Category>().OrderBy(x=>x.ParentCode).OrderBy(x=>x.Code);
        gv.DataBind();
    }
}