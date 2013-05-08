using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NModel;
using NBiz;
public partial class Admin_Products_ProductImageImport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private void Import()
    {
        Organizer org = new Organizer();
        //org.Execute(Server.MapPath("/ProductImages/"),
    }
}