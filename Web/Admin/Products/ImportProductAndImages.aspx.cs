using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using NBiz;
using NModel;
public partial class Admin_Products_ImportProductAndImages : System.Web.UI.Page
{
    string ProductsData_ToImport, ProductsData_Imported;

    protected void Page_Load(object sender, EventArgs e)
    {
        ProductsData_ToImport = Server.MapPath("/ProductsData_ToImport/");
        ProductsData_Imported = Server.MapPath("/ProductsData_Imported/");
        if (!IsPostBack)
        {
            LoadDirTree();
        }
    }

    private void LoadDirTree()
    {
        DirectoryInfo dir = new DirectoryInfo(ProductsData_ToImport);
        DirectoryInfo[] dirChildren = dir.GetDirectories();
        foreach (DirectoryInfo c in dirChildren)
        {
            TreeNode node = new TreeNode(c.Name);
            tr.Nodes.Add(node);
        }
    }

    private DirectoryInfo[] GetImportedDir()
    {
        IList<DirectoryInfo> dir = new List<DirectoryInfo>();
        foreach (TreeNode node in tr.Nodes)
        {
            if (node.Checked)
            {
                DirectoryInfo cdir = new DirectoryInfo(ProductsData_ToImport + @"\" + node.Text);
                dir.Add(cdir);
            }
        }
        return dir.ToArray();
    }
    BizImportLog bizImportlog = new BizImportLog();
    protected void btnImport_Click(object sender, EventArgs e)
    {
        DateTime finishTime=Convert.ToDateTime(tbxFinishTime.Text.Trim());
        ImportOperationLog il = new ImportOperationLog();
        string fileNames=string.Empty;
        foreach(DirectoryInfo dir in GetImportedDir())
        {
         fileNames+=dir.FullName+"__";
        }
        

        try
        {
            

            ProductImportor importor = new ProductImportor(true);
            //importor.CheckWithDatabase = true;
            importor.WebProductImagesPath = Server.MapPath("/ProductImages/original/");
            importor.Import(GetImportedDir(), ProductsData_Imported);
            tbxMsg.CssClass = "success";
            tbxMsg.Text = importor.ImportMsg;

            //保存日志
            bizImportlog.Import(fileNames, importor.Result_ProductsSuccessImported, finishTime, tbxFrom.Text
                , "数据部",importor.ImportMsgForLog);
        }
        catch (Exception ex)
        {
            tbxMsg.CssClass = "error";
            tbxMsg.Text = ex.Message;
        }
    }
}