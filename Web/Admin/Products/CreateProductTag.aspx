<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeFile="CreateProductTag.aspx.cs" Inherits="Admin_Products_CreateProductTag" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>创建产品清单</h1>
<table>
<tr><td>清单名称:</td><td><asp:TextBox runat="server" ID="tbxTagName"></asp:TextBox></td></tr>
<tr><td>描述:</td><td><asp:TextBox runat="server" ID="tbxDescription" TextMode="MultiLine"></asp:TextBox></td></tr>
<tr><td>产品列表:</td><td>
    <asp:TextBox runat="server" ID="tbxProductList" TextMode="MultiLine"></asp:TextBox>
</td></tr>
<tr>
<td></td>
<td><uc:ButtonExt runat="server" ID="btnSaveTag" OnClick="btnSaveTag_Click" Text="保存" />
<asp:Label runat="server" ID="lblMsg"></asp:Label>
 </td>
</tr>
<tr>
<td colspan="2">
<asp:TextBox  runat="server" ID="tbxMsg" TextMode="MultiLine"></asp:TextBox>
</td>
</tr>
</table>
</asp:Content>

