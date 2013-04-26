<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Products_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<fieldset>
 <legend>搜索</legend>
 <div>
 <span>供应商名称:</span> <asp:TextBox runat="server" ID="tbxSupplierName"></asp:TextBox><asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="搜索"/>
 </div>
 </fieldset>
<uc:AspNetPager runat="server" ID="AspNetPager1"   CloneFrom="pager" ></uc:AspNetPager>
<asp:GridView  AutoGenerateColumns="false" runat="server" ID="dgProduct" 
        onrowdatabound="dgProduct_RowDataBound" RowStyle-Height="60" >
<Columns>
<asp:TemplateField HeaderText="图片">
  <ItemTemplate>
    <asp:Repeater runat="server" ID="rptImages">
    <ItemTemplate>
   <%-- <img style="width:200px" src='/ProductImages/<%# Container.DataItem.ToString()%>'  alt=""/>--%>
  <img src="/ImageHandler.ashx?imagename=<%# Container.DataItem.ToString()%>&width=50&height=50&tt=2" />
    </ItemTemplate>
    </asp:Repeater>
  </ItemTemplate>
</asp:TemplateField>
<asp:BoundField   HeaderText="名称" DataField="Name"/>
<asp:BoundField   HeaderText="NTS编码" DataField="NTSCode"/>
<asp:BoundField   HeaderText="供应商名称" DataField="SupplierName"/>

</Columns>
<EmptyDataTemplate>
<div class="notice">
 没有相关信息
</div>
</EmptyDataTemplate>
</asp:GridView>
<uc:AspNetPager runat="server" ID="pager"  UrlPaging="true" 
        CustomInfoHTML="Total:%RecordCount% Page %CurrentPageIndex% of %PageCount%" 
        EnableTheming="True" ShowCustomInfoSection="Left" ShowNavigationToolTip="True"></uc:AspNetPager>

</asp:Content>

