using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NModel;
using NBiz;
using System.IO;
public partial class Admin_Products_ProductImageImport : System.Web.UI.Page
{
    string ProductImages_ToImport, ProductImages_OriginalPath;

    protected void Page_Load(object sender, EventArgs e)
    {
        ProductImages_ToImport = Server.MapPath("/ProductImages_ToImport/");
        ProductImages_OriginalPath = Server.MapPath("/ProductImages/Original/");
        if (!IsPostBack)
        {
            LoadDirTree();
        }
    }

    private void LoadDirTree()
    {
        DirectoryInfo dir = new DirectoryInfo(ProductImages_ToImport);
        DirectoryInfo[] dirChildren = dir.GetDirectories();
        foreach (DirectoryInfo c in dirChildren)
        {
            TreeNode node = new TreeNode(c.Name);
            tr.Nodes.Add(node);
        }
    }

   
    BizImportLog bizImportlog = new BizImportLog();
    protected void btnImport_Click(object sender, EventArgs e)
    {
         string msg = string.Empty;
      
        try
        {
            ProductImageImporter importor = new ProductImageImporter();
            //importor.CheckWithDatabase = true;
        IList<ImageInfo> imported=    importor.ImportImage(ProductImages_ToImport, ProductImages_OriginalPath, out msg);
        msg = "成功导入" + imported.Count + "张" + Environment.NewLine;
            tbxMsg.Text = msg;
        }
        catch (Exception ex)
        {
            tbxMsg.CssClass = "error";
            tbxMsg.Text = ex.Message;
            if (ex.InnerException != null)
            {
                tbxMsg.Text += Environment.NewLine + ex.InnerException.Message;
            }
        }
    }
}