﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using NModel;
using NBiz;
public partial class Products_ProductTag : System.Web.UI.Page
{
    BizProductTag bizTag = new BizProductTag();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindTag();

        }
        string tagId = Request["tagid"];
        if (string.IsNullOrEmpty(tagId))
        {
            return;
        }


        ProductTag pt = bizTag.GetOne(new Guid(tagId));
        if (pt == null) return;
        string cateCode = Request["cate"];
        
        var productListAllCates=pt.Product_Tags.Select(x=>x.Product).ToList();
        BindCategory(productListAllCates);
        IList<Product> productListCated = productListAllCates;
        if (!string.IsNullOrEmpty(cateCode))
        {
            productListCated = productListAllCates.Where(x => x.CategoryCode == cateCode).ToList();
        }
        ascxProList.ProductList = productListCated;
       
    }

    private void BindProduct()
    {
        ascxProList.ProductList = new List<Product>();
        ascxProList.RecordCount = 20;

    }
    BizCategory bizCate = new BizCategory();
    private void BindCategory(IList<Product> productList)
    {
       // IList<Category> allCates = bizCate.GetAll<Category>();

        var g = from p in productList
                
                group p by p.CategoryCode into cate
                select new { Cate = cate.Key, Products = cate };

        rptCate.DataSource = g.OrderBy(x=>x.Cate);
        rptCate.DataBind();
    }
    
    protected void rptCate_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HtmlAnchor href = e.Item.FindControl("hrefCateAmount") as HtmlAnchor;
            dynamic data = e.Item.DataItem;
            if (string.IsNullOrEmpty(Request["cate"]))
            {
                href.HRef = Request.Url + "&cate=" + data.Cate;
            }
            else
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("cate", data.Cate);
                string url = Request.Url.AbsolutePath;
                string updatedQueryString = "?" + nameValues.ToString();
                href.HRef = url + updatedQueryString;
            }
            href.InnerText = bizCate.GetCateName(data.Cate);
          
           
        }
    }
    private void BindTag()
    {
        rptTags.DataSource = bizTag.GetAll<ProductTag>();
        rptTags.DataBind();
    }
}