<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Admin_OperationLog_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:GridView runat="server" ID="gv" AutoGenerateColumns="false">
        <Columns>
         <asp:BoundField DataField="FinishTime" HeaderText="数据完成日期" />
            <asp:BoundField DataField="ImportTime" HeaderText="导入日期" />
            <asp:BoundField DataField="FileFrom" HeaderText="来源" />
            <asp:BoundField DataField="ImportMember" HeaderText="操作员" />
            <asp:TemplateField HeaderText="导入数量">
                <ItemTemplate>
                    <%#((NModel.ImportOperationLog)Container.DataItem).ImportedItems.Count %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="导入日志">
                <ItemTemplate>
                    <textarea  rows="8" style="height:auto" disabled="disabled">
  <%#Eval("ImportResult") %>
  </textarea>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
