<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true"
    CodeFile="ProductImageImport.aspx.cs" Inherits="Admin_Products_ProductImageImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <p>
        前提条件: 1) 相关产品资料已经导入 2) 产品图片已按照规定结构上传至服务器的指定位置. 规定结构:供应商/产品型号.jpg.指定位置:192.168.1.44/VirtualPath/NtsBase/ProductImages/
    </p>
   <br />
   <asp:RadioButtonList runat="server" ID="rbl">
   
   </asp:RadioButtonList>
    <asp:Button runat="server" ID="btnImportImages" Text="开始导入" />
</asp:Content>
