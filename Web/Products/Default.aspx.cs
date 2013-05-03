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
            LoadParameters();
            BindProduct();
            
        }
    }
    //搜索关键字回显
    private void LoadParameters()
    {
        string supplierName =Server.UrlDecode( Request["sname"]);
        tbxSupplierName.Text =  supplierName;
        tbxModel.Text = Server.UrlDecode(Request["model"]);
        bool hasPhoto = false;
        bool.TryParse(Request["hasPhoto"],out hasPhoto);
        cbxHasPhoto.Checked = hasPhoto;
        tbxCode.Text =Server.UrlDecode( Request["categoryCode"]);
        tbxName.Text = Request["name"];
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
        string targetUrl =string.Format( "Default.aspx?sname={0}&model={1}&hasphoto={2}&name={3}&categorycode={4}"
            , Server.UrlEncode(tbxSupplierName.Text)
            ,Server.UrlDecode(tbxModel.Text)
            ,cbxHasPhoto.Checked
            ,tbxName.Text.Trim()
            ,tbxCode.Text.Trim()
            );
        Response.Redirect(targetUrl, true);
       // BindProduct();
    }
    private void BindProduct()
    {
        int pageIndex = GetPageIndex();
        int totalRecords;
        var product = bizProduct.Search(tbxSupplierName.Text.Trim()
            ,tbxModel.Text.Trim()
            ,cbxHasPhoto.Checked
            ,tbxName.Text.Trim()
            ,tbxCode.Text.Trim()
            , pager.PageSize
            , pageIndex, out totalRecords)
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