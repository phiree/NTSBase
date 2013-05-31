<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true"
    CodeFile="ProductExport.aspx.cs" Inherits="Admin_Products_ProductExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageDesc" runat="Server">
    导出所有产品的英文资料
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>英文产品资料</legend>
        <div>
            <uc:ButtonExt runat="server" ID="btnExportExcel" Text="导出Excel" OnClick="btnExportExcel_Click" />
            <uc:ButtonExt runat="server" ID="btnExportImage" Text="导出图片" OnClick="btnExportImage_Click" />
        </div>
    </fieldset>
    <asp:Label runat="server" CssClass="info" ID="lblMsg"></asp:Label>
</asp:Content>
