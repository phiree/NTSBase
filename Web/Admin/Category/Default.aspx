<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Category_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:GridView runat="server" ID="gv" AutoGenerateColumns="false">
<Columns><asp:TemplateField  HeaderText="序号">
<ItemTemplate>
<%# Container.DataItemIndex +1%>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField  HeaderText="名称" DataField="Name" />
<asp:BoundField  HeaderText="英文名称" DataField="EnglishName" />
<asp:BoundField  HeaderText="分类编码" DataField="Code" />
<asp:BoundField  HeaderText="父类编码" DataField="ParentCode" />
<asp:BoundField  HeaderText="备注" DataField="Memo" />

</Columns>
</asp:GridView>
</asp:Content>

