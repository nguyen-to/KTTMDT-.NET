<%@ Page Title="" Language="C#" MasterPageFile="~/ProjectMaster.Master" AutoEventWireup="true" CodeBehind="MyOrders.aspx.cs" Inherits="ProjectGroup10.MyOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Css/orders.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-md-8">
                    <h1 class="page-title">Sản phẩm đã mua</h1>
                    <p class="text-muted">Danh sách các sản phẩm bạn đã mua</p>
                </div>
                <div class="col-md-4 text-end">
                    <a href="Default.aspx" class="btn-back">
                        <i class="fas fa-arrow-left"></i> Quay lại
                    </a>
                </div>
            </div>
        </div>
    </div>

    <div class="container">
        <!-- Orders List -->
        <asp:Repeater ID="rptOrders" runat="server">
            <ItemTemplate>
                <div class="order-card">
                    <div class="order-header">
                        <div>
                            <div class="order-id">Đơn hàng #<%# Eval("orderId") %></div>
                            <div class="order-date"><%# Convert.ToDateTime(Eval("orderDate")).ToString("dd/MM/yyyy HH:mm") %></div>
                        </div>
                        <div class="order-status status-<%# Eval("status").ToString().ToLower() %>">
                            <%# Eval("status").ToString() %>
                        </div>
                    </div>
                    <div class="order-details">
                        <div>
                            <div class="product-name"><%# Eval("productName") %></div>
                        </div>
                        <div class="order-quantity">
                            Số lượng: <%# Eval("quantity") %>
                        </div>
                        <div class="order-price">
                            <%# String.Format("{0:N0}", Eval("price")) %> VNĐ
                        </div>
                        <div class="order-total">
                            Tổng: <%# String.Format("{0:N0}", Eval("totalAmount")) %> VNĐ
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <!-- No Orders Message -->
        <asp:Panel ID="pnlNoOrders" runat="server" Visible="false">
            <div class="no-orders">
                <i class="fas fa-shopping-cart"></i>
                <h3>Chưa có đơn hàng nào</h3>
                <p>Bạn chưa mua sản phẩm nào. Hãy khám phá các sản phẩm tuyệt vời của chúng tôi!</p>
                <a href="Default.aspx" class="btn btn-primary">Mua sắm ngay</a>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
