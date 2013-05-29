<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true"
    CodeFile="ProductImport.aspx.cs" Inherits="Admin_Products_ProductImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ContentPlaceHolderID="pageDesc" runat="server">
将Excel表单导入系统.支持两种格式: <a href="/Content/files/产品导入Excel示例_报价单.xls">报价单</a> 和 
<a href="/Content/files/产品导入Excel示例_ERP导入格式表.xls">
   ERP数据表</a>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3>选择Excel表单:</h3>
    <p>
        <asp:FileUpload runat="server" ID="fuProduct" />
        <asp:Button runat="server" ID="btnImport" OnClick="btnImport_Click" Text="导入" />
    </p>
    <p>
        <div runat="server" ID="lblMsg">
    
        </div>
    </p>
</asp:Content>
