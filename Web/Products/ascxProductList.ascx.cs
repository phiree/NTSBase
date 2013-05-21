using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NModel;

public partial class Products_ascxProductList : System.Web.UI.UserControl
{
    public IList<Product> ProductList { private get; set; }
    public int RecordCount { private get; set; }
    public int PageSize { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        pager.PageSize = PageSize;
        if (!IsPostBack)
        {
            pager.RecordCount = RecordCount;
            dgProduct.DataSource = ProductList;
            dgProduct.DataBind();
        }
    }
    protected void dgProduct_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            NModel.Product p = e.Row.DataItem as NModel.Product;
            Repeater rptImages = e.Row.FindControl("rptImages") as Repeater;
            rptImages.DataSource = p.ProductImageUrls;
            rptImages.DataBind();

        }
    }

    protected void rptImages_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            if (((Repeater)sender).Items.Count == 0)
            {
                Image img = e.Item.FindControl("imgNoPic") as Image;
                img.Visible = true;
                img.ImageUrl = "~/content/images/no-image.gif";
            }
        }
    }
}