<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeFile="CategoryImport.aspx.cs" Inherits="Admin_Category_CategoryImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h3>选择分类列表:</h3>
    <p>
        <asp:FileUpload runat="server" ID="fuCategory" />
        <asp:Button runat="server" ID="btnImport" OnClick="btnImport_Click" Text="导入" />
    </p>
   
        <div  runat="server" ID="lblMsg">
    
        </div>
    
</asp:Content>

