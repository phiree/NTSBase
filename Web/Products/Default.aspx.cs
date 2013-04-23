using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NBiz;
using NDAL;
public partial class Products_Default : System.Web.UI.Page
{
    BLLBase<NModel.Product> bizProduct = new NBiz.BizProduct();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindProduct();
        }
    }
    private void BindProduct()
    {
        var product = bizProduct.GetAll<NModel.Product>();
        dgProduct.DataSource = product;
        dgProduct.DataBind();
    }
}