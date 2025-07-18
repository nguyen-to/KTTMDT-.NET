<%@ Page Title="" Language="C#" MasterPageFile="~/ProjectMaster.Master" AutoEventWireup="true" CodeBehind="Carts.aspx.cs" Inherits="ProjectGroup10.Carts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Css/carts.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-md-8">
                    <h1 class="page-title">Giỏ hàng của bạn</h1>
                    <p class="text-muted">Quản lý các sản phẩm bạn muốn mua</p>
                </div>
                <div class="col-md-4 text-end">
                    <a href="Default.aspx" class="btn-back">
                        <i class="fas fa-arrow-left"></i> Tiếp tục mua sắm
                    </a>
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

        <!-- Cart Items -->
        <div class="cart-container">
            <div class="cart-header">
                <h2 class="cart-title">Sản phẩm trong giỏ hàng</h2>
                <p class="text-muted">Bạn có <asp:Label ID="lblCartCount" runat="server" Text="0"></asp:Label> sản phẩm trong giỏ hàng</p>
            </div>

            <!-- Cart Items List -->
            <asp:Panel ID="pnlCartItems" runat="server">
                <asp:Repeater ID="rptCartItems" runat="server" OnItemCommand="rptCartItems_ItemCommand">
                    <ItemTemplate>
                        <div class="cart-item">
                            <asp:Image ID="imgProduct" runat="server" 
                                CssClass="product-image"
                                ImageUrl='<%# Eval("imageUrl") %>' 
                                AlternateText='<%# Eval("productName") %>' />
                            
                            <div class="product-info">
                                <div class="product-name"><%# Eval("productName") %></div>
                                <div class="product-price"><%# String.Format("{0:N0}", Eval("price")) %> VNĐ</div>
                                
                                <div class="quantity-controls">
                                    <asp:Button ID="btnDecrease" runat="server" 
                                        CssClass="btn-quantity" 
                                        Text="-" 
                                        CommandName="UpdateQuantity" 
                                        CommandArgument='<%# Eval("cartId") + ",-1" %>' />
                                    
                                    <span class="quantity-display"><%# Eval("quantity") %></span>
                                    
                                    <asp:Button ID="btnIncrease" runat="server" 
                                        CssClass="btn-quantity" 
                                        Text="+" 
                                        CommandName="UpdateQuantity" 
                                        CommandArgument='<%# Eval("cartId") + ",1" %>' />
                                </div>
                                
                                <div class="total-price">
                                    Tổng: <%# String.Format("{0:N0}", Convert.ToInt32(Eval("quantity")) * Convert.ToDecimal(Eval("price"))) %> VNĐ
                                </div>
                            </div>
                            
                            <div class="cart-actions">
                                <asp:Button ID="btnBuyNow" runat="server" 
                                    CssClass="btn-buy" 
                                    Text="Mua hàng" 
                                    CommandName="BuyNow" 
                                    CommandArgument='<%# Eval("productId") + "," + Eval("quantity") + "," + Eval("cartId") %>' />
                                
                                <asp:Button ID="btnRemove" runat="server" 
                                    CssClass="btn-remove" 
                                    Text="Xóa" 
                                    CommandName="Remove" 
                                    CommandArgument='<%# Eval("cartId") %>'
                                    />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>

            <!-- Empty Cart Message -->
            <asp:Panel ID="pnlEmptyCart" runat="server" Visible="false">
                <div class="empty-cart">
                    <i class="fas fa-shopping-cart"></i>
                    <h3>Giỏ hàng trống</h3>
                    <p>Bạn chưa có sản phẩm nào trong giỏ hàng.</p>
                    <a href="Default.aspx" class="btn-back">Bắt đầu mua sắm</a>
                </div>
            </asp:Panel>

            <!-- Cart Summary -->
            <asp:Panel ID="pnlCartSummary" runat="server" Visible="false">
                <div class="cart-summary">
                    <div class="summary-row">
                        <span>Tổng số sản phẩm:</span>
                        <span><asp:Label ID="lblTotalItems" runat="server"></asp:Label></span>
                    </div>
                    <div class="summary-row">
                        <span>Tổng số lượng:</span>
                        <span><asp:Label ID="lblTotalQuantity" runat="server"></asp:Label></span>
                    </div>
                    <div class="summary-row summary-total">
                        <span>Tổng tiền:</span>
                        <span><asp:Label ID="lblTotalAmount" runat="server"></asp:Label> VNĐ</span>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
