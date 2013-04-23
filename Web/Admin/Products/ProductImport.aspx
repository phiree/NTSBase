<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeFile="ProductImport.aspx.cs" Inherits="Admin_Products_ProductImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
execl路径:<asp:TextBox runat="server" ID="tbxProductExcel"></asp:TextBox>
<asp:Button runat="server" ID="btnImport"  OnClick="btnImport_Click" Text="导入"/>
</asp:Content>

