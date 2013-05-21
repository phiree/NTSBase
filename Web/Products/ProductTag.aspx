<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true"
    CodeFile="ProductTag.aspx.cs" Inherits="Products_ProductTag" %>

<%@ Register Src="~/Products/ascxProductList.ascx" TagPrefix="pro" TagName="list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <div>
            标签:</div>
        <asp:Repeater runat="server" ID="rptTags">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li><a href='ProductTag.aspx?tagid=<%#Eval("Id") %>'>
                    <%#Eval("TagName")%></a></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul></FooterTemplate>
        </asp:Repeater>
    </div>
    <div>
        <div>
            分类:</div>
        <asp:Repeater runat="server" ID="rptCate" OnItemDataBound="rptCate_ItemDataBound">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li><a  runat="server" id="hrefCateAmount">
                    <%#Eval("Cate") %> </a>(<%#((IList<NModel.Product>)Eval("Products")).Count %>)
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul></FooterTemplate>
        </asp:Repeater>
    </div>
    <div>产品列表:</div>
    <pro:list runat="server" ID="ascxProList" />
    <div>
    </div>
</asp:Content>
