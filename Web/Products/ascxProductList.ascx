<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ascxProductList.ascx.cs" Inherits="Products_ascxProductList" %>
<uc:AspNetPager runat="server" ID="AspNetPager1" CloneFrom="pager">
    </uc:AspNetPager>
    <asp:GridView AutoGenerateColumns="false" runat="server" ID="dgProduct" OnRowDataBound="dgProduct_RowDataBound"
        RowStyle-Height="60">
        <Columns>
           <asp:TemplateField>
              <ItemTemplate>
                    <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
           </asp:TemplateField>
            <asp:TemplateField HeaderText="图片">
                <ItemTemplate>
                    <asp:Repeater runat="server" ID="rptImages" OnItemDataBound="rptImages_ItemDataBound">
                 
                        <ItemTemplate>
                            <%-- <img style="width:200px" src='/ProductImages/<%# Container.DataItem.ToString()%>'  alt=""/>--%>
                            <!--"/ProductImages/<%# Container.DataItem.ToString()%> "
   "/ImageHandler.ashx?imagename=<%# Container.DataItem.ToString()%>&width=50&height=50&tt=2" 
   "/ProductImages/thumbnails/1742381668_100-100.JPG"
   -->
                            <a href="/ProductImages/original/<%# Container.DataItem.ToString()%>" title="点击查看原图"
                                target="_blank">
                                <img src="/ImageHandler.ashx?imagename=<%# Container.DataItem.ToString()%>&width=50&height=50&tt=2" />
                            </a>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Image runat="server" ID="imgNoPic" Visible="false" />
                        </FooterTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField HeaderText="名称" DataTextField="Name" Target="_blank" DataNavigateUrlFields="Id"
                DataNavigateUrlFormatString="/products/productdetail.aspx?id={0}" />
            <asp:BoundField HeaderText="型号" DataField="ModelNumber" />
            <asp:BoundField HeaderText="NTS编码" DataField="NTSCode" />
            <asp:BoundField HeaderText="供应商名称" DataField="SupplierName" />
        </Columns>
        <EmptyDataTemplate>
            <div class="notice">
                没有相关信息
            </div>
        </EmptyDataTemplate>
    </asp:GridView>
    <uc:AspNetPager runat="server" ID="pager" UrlPaging="true" CssClass="paginator" CustomInfoHTML="总计:&lt;b&gt;%RecordCount%&lt;/b&gt; 页码: %CurrentPageIndex% / %PageCount%"
        EnableTheming="True" ShowCustomInfoSection="Left" ShowNavigationToolTip="True"
        CustomInfoSectionWidth="" FirstPageText="第一页" LastPageText="最后一页" NextPageText="下一页"
        PrevPageText="上一页">
    </uc:AspNetPager>