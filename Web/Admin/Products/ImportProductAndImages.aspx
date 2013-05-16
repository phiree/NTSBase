<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeFile="ImportProductAndImages.aspx.cs" Inherits="Admin_Products_ImportProductAndImages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<fieldset>
<legend>已上传的数据</legend>
<div style="float:left">
<asp:TreeView runat="server" ID="tr" ShowLines="true"  ShowCheckBoxes="All"></asp:TreeView>
</div>
</fieldset>

<uc:ButtonExt runat="server" id="btnImport"  OnClick="btnImport_Click" Text="开始导入" />
<div>
<asp:TextBox runat="server" ID="tbxMsg"  TextMode="MultiLine" Rows="40"></asp:TextBox>
</div>
</asp:Content>

