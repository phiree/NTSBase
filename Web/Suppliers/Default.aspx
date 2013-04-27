<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Suppliers_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
 <fieldset>
 <legend>搜索</legend>
 <div>
 <span>供应商名称:</span> <asp:TextBox runat="server" ID="tbxName"></asp:TextBox><asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="搜索"/>
 </div>
 </fieldset>
</div>
<uc:AspNetPager runat="server" ID="AspNetPager1"   CloneFrom="pager" ></uc:AspNetPager>
<asp:DataGrid runat="server" ID="dgSupplier"></asp:DataGrid>
<uc:AspNetPager runat="server" ID="pager"  UrlPaging="true"  CssClass="paginator"
        CustomInfoHTML="Total:%RecordCount% Page %CurrentPageIndex% of %PageCount%" 
        EnableTheming="True" ShowCustomInfoSection="Left" ShowNavigationToolTip="True"></uc:AspNetPager>

</asp:Content>

