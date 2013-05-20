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
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void BuildProductList()
    {
        IList<NModel.Product> productList = bizProduct.GetListByProvidedModelNumberSupplierNameList(tbxProductList.Text);


       
        
    }
    


}