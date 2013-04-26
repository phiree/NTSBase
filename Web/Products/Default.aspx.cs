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
    BizProduct bizProduct = new NBiz.BizProduct();
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindProduct();
        }
    }
    private int GetPageIndex()
    {
        int pageIndex=0;
        string paramPageIndex = Request[pager.UrlPageIndexName];
        if (!int.TryParse(paramPageIndex, out pageIndex))
        {
            pageIndex = 0;
        }
        return pageIndex;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindProduct();
    }
    private void BindProduct()
    {
        int pageIndex = GetPageIndex();
        int totalRecords;
        var product = bizProduct.Search(tbxSupplierName.Text.Trim(), pager.PageSize, pageIndex, out totalRecords)
            .OrderBy(x=>x.CategoryCode);  // bizProduct.GetAll<NModel.Product>();
        pager.RecordCount = totalRecords;
        dgProduct.DataSource = product;
        dgProduct.DataBind();
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
}