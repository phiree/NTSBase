<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true"
    CodeFile="ProductTag.aspx.cs" Inherits="Products_ProductTag" %>

<%@ Register Src="~/Products/ascxProductList.ascx" TagPrefix="pro" TagName="list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style type="text/css">
#main_leftmenu li
{
     float:left;
     padding-left:10px;
     list-style-type:none;
   width:170px;
    }
    #main_leftmenu li a
    {
          font-size:larger; 
        }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="main_leftmenu">
        <div>标签:</div>
        <asp:Repeater runat="server" ID="rptTags">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li>[<%#((DateTime)Eval("CreateTime")).ToString("MM月dd日HH点") %>]<a href='ProductTag.aspx?tagid=<%#Eval("Id") %>'>
                    <%#Eval("TagName")%></a>(<%#((IList<NModel.ProductTag_Product>)Eval("Product_Tags")).Count %>)</li>
            </ItemTemplate>
            <FooterTemplate>
                </ul></FooterTemplate>
        </asp:Repeater> <div class="clear"></div>
        <div>分类:</div>
        <asp:Repeater runat="server" ID="rptCate" OnItemDataBound="rptCate_ItemDataBound">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li><a runat="server" id="hrefCateAmount">
                    <%#Eval("Cate") %>
                </a>(<%#((IList<NModel.Product>)Eval("Products")).Count %>) </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul></FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="clear"></div>
    <div style="margin-top:14px;">
        产品列表:</div>
    <pro:list runat="server" ID="ascxProList" />
</asp:Content>
