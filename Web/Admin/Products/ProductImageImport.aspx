<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true"
    CodeFile="ProductImageImport.aspx.cs" Inherits="Admin_Products_ProductImageImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
<legend>待导入图片</legend>
<div style="float:left">
<asp:TreeView runat="server" ID="tr"  ShowExpandCollapse="false"></asp:TreeView>
</div>
</fieldset>


<uc:ButtonExt runat="server" id="btnImport"  OnClick="btnImport_Click" Text="开始导入" />
<div>
<asp:TextBox Width="100%" CssClass="success"  Enabled="false" runat="server" ID="tbxMsg"  TextMode="MultiLine" Rows="40"></asp:TextBox>
</div>
</asp:Content>
