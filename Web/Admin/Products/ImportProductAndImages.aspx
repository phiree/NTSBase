<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeFile="ImportProductAndImages.aspx.cs" Inherits="Admin_Products_ImportProductAndImages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<fieldset>
<legend>已上传的数据</legend>
<div style="float:left">
<asp:TreeView runat="server" ID="tr"  ShowExpandCollapse="false"  ShowCheckBoxes="All"></asp:TreeView>
</div>
</fieldset>
来源:<asp:TextBox runat="server" ID="tbxFrom"></asp:TextBox>
数据完成时间:
<asp:TextBox runat="server" ID="tbxFinishTime"></asp:TextBox>

<uc:ButtonExt runat="server" id="btnImport"  OnClick="btnImport_Click" Text="开始导入" />
<div>
<asp:TextBox Width="100%"  Enabled="false" runat="server" ID="tbxMsg"  TextMode="MultiLine" Rows="40"></asp:TextBox>
</div>
</asp:Content>

