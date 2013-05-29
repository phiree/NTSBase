<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="ProductDetail.aspx.cs" Inherits="Products_ProductDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <asp:DetailsView  ID="dv" AutoGenerateRows="false" runat="server" 
        ondatabound="dv_DataBound" onitemcreated="dv_ItemCreated" 
        onload="dv_Load"   >
 <Fields>
 <asp:BoundField  ItemStyle-Font-Size="Medium"  ItemStyle-Font-Bold="true"  HeaderText="名称" DataField="Name"/>
<asp:BoundField   HeaderText="型号" DataField="ModelNumber"/>
 <asp:BoundField ItemStyle-Font-Bold="true"  HeaderText="出厂价" DataField="PriceOfFactory"/>
<asp:BoundField   HeaderText="NTS编码" DataField="NTSCode"/>
<asp:BoundField   HeaderText="分类编码" DataField="CategoryCode"/>
<asp:BoundField   HeaderText="产品型号" DataField="ModelNumber"/>
<asp:BoundField   HeaderText="单位" DataField="Unit"/>
<asp:BoundField   HeaderText="产地" DataField="PlaceOfOrigin"/>
<asp:BoundField   HeaderText="发货地" DataField="PlaceOfDelivery"/>
<asp:BoundField   HeaderText="税率" DataField="TaxRate"/>
<asp:BoundField   HeaderText="起定量" DataField="OrderAmountMin"/>
<asp:BoundField   HeaderText="生产周期" DataField="ProductionCycle"/>

<asp:BoundField   HeaderText="最后修改日期" DataField="LastUpdateTime"/>
<asp:BoundField   HeaderText="规格参数" DataField="ProductParameters"/>
<asp:BoundField   HeaderText="产品描述" DataField="ProductDescription"/>
<asp:TemplateField HeaderText="图片">
  <ItemTemplate>
    <asp:Repeater runat="server" ID="rptImages">
    <ItemTemplate>
   <%-- <img style="width:200px" src='/ProductImages/<%# Container.DataItem.ToString()%>'  alt=""/>--%>
  <!--"/ProductImages/<%# Container.DataItem.ToString()%> "
   "/ImageHandler.ashx?imagename=<%# Container.DataItem.ToString()%>&width=50&height=50&tt=2" 
   "/ProductImages/thumbnails/1742381668_100-100.JPG"
   -->
  <a href=

 "/ProductImages/original/<%# Container.DataItem.ToString()%>" 
 title="点击查看原图" target="_blank">
  <img src= "/ImageHandler.ashx?imagename=<%# Container.DataItem.ToString()%>&width=500&height=500&tt=2"/>
 </a>
    </ItemTemplate>
    </asp:Repeater>
  </ItemTemplate>
</asp:TemplateField>

 </Fields>
 </asp:DetailsView>
</asp:Content>

