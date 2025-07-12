<%@ Page Title="" Language="C#" MasterPageFile="~/ProjectMaster.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProjectGroup10.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Css/default.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div class="page-header">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-md-8">
                    <h1 class="page-title">Danh sách sản phẩm</h1>
                    <p class="text-muted">Khám phá các sản phẩm nội thất chất lượng cao</p>
                </div>
                <div class="col-md-4 text-end">
                    <asp:Button ID="btnViewOrders" runat="server" 
                        CssClass="btn btn-orders" 
                        Text="Sản phẩm đã mua" 
                        OnClick="btnViewOrders_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="container">
        <!-- Alert Messages -->
        <asp:Panel ID="pnlMessage" runat="server" Visible="false">
            <div id="divAlert" runat="server" class="alert" role="alert">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
        </asp:Panel>

        <!-- Products Grid -->
        <div class="product-grid">
            <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
                <ItemTemplate>
                    <div class="product-card">
                        <div class="product-image">
                           <asp:Image ID="Image1" runat="server" Width="200px"
                             ImageUrl='<%# ResolveUrl("~/Images/" + Eval("imageUrl")) %>' />

                        </div>
                        <div class="product-name"><%# Eval("productName") %></div>
                        <div class="product-description"><%# Eval("description") %></div>
                        <div class="product-price"><%# String.Format("{0:N0}", Eval("price")) %> VNĐ</div>
                        <asp:Button ID="btnBuy" runat="server" 
                            CssClass="btn btn-buy" 
                            Text="Mua hàng" 
                            CommandName="BuyProduct" 
                            CommandArgument='<%# Eval("productId") %>'
                             />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- No Products Message -->
        <asp:Panel ID="pnlNoProducts" runat="server" Visible="false">
            <div class="no-products">
                <i class="fas fa-box-open"></i>
                <h3>Không có sản phẩm nào</h3>
                <p>Hiện tại chưa có sản phẩm nào trong cửa hàng.</p>
            </div>
        </asp:Panel>
    </div>

</asp:Content>
