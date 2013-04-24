<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true"
    CodeFile="ProductImport.aspx.cs" Inherits="Admin_Products_ProductImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <p>
        <span>execl路径:</span><asp:TextBox Width="600" runat="server" ID="tbxProductExcel"></asp:TextBox>

        <asp:FileUpload runat="server" ID="fuProduct" />
        <asp:Button runat="server" ID="btnImport" OnClick="btnImport_Click" Text="导入" />
    </p>
    <p>
        <div  runat="server" ID="lblMsg">
    
        </div>
    </p>
</asp:Content>
