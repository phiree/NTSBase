using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NBiz;
using NModel;
public partial class Admin_Products_CreateProductTag : System.Web.UI.Page
{
    BizProduct bizProduct = new BizProduct();
    BizProductTag bizTag = new BizProductTag();
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    private void SaveTag()
    {
        string msg;
        IList<NModel.Product> productList = bizProduct.GetListByProvidedModelNumberSupplierNameList(tbxProductList.Text,out msg);

        ProductTag tag = bizTag.Save(tbxTagName.Text, tbxDescription.Text, productList);
        lblMsg.Text = "导入产品数量:" + productList.Count;
        tbxMsg.Text = msg;
   }
    protected void btnSaveTag_Click(object sender, EventArgs e)
    {
        SaveTag();    
    }

    private void BuildProductList()
    {


    }



}