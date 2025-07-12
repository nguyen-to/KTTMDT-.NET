<%@ Page Title="" Language="C#" MasterPageFile="~/ProjectMaster.Master" AutoEventWireup="true" CodeBehind="PurchaseConfirm.aspx.cs" Inherits="ProjectGroup10.PurchaseConfirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Css/purchase.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="confirm-container">
            <div class="confirm-header">
                <i class="fas fa-shopping-cart"></i>
                <h2>Xác nhận mua hàng</h2>
                <p class="text-muted">Vui lòng kiểm tra thông tin đơn hàng</p>
            </div>

            <!-- Alert Messages -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <div id="divAlert" runat="server" class="alert" role="alert">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
            </asp:Panel>

            <!-- Product Information -->
            <div class="product-info">
                <div class="info-row">
                    <span class="info-label">Sản phẩm:</span>
                    <span class="info-value">
                        <asp:Label ID="lblProductName" runat="server"></asp:Label>
                    </span>
                </div>
                <div class="info-row">
                    <span class="info-label">Giá:</span>
                    <span class="info-value">
                        <asp:Label ID="lblPrice" runat="server"></asp:Label> VNĐ
                    </span>
                </div>
                <div class="info-row">
                    <span class="info-label">Số lượng:</span>
                    <div class="quantity-control">
                        <asp:Button ID="btnDecrease" runat="server" CssClass="btn-quantity" Text="-" OnClick="btnDecrease_Click" />
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="quantity-input" Text="1" TextMode="Number" AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged"></asp:TextBox>
                        <asp:Button ID="btnIncrease" runat="server" CssClass="btn-quantity" Text="+" OnClick="btnIncrease_Click" />
                    </div>
                </div>
                <div class="info-row">
                    <span class="info-label">Tổng tiền:</span>
                    <span class="info-value total-amount">
                        <asp:Label ID="lblTotalAmount" runat="server"></asp:Label> VNĐ
                    </span>
                </div>
            </div>

            <!-- Action Buttons -->
            <asp:Button ID="btnConfirmPurchase" runat="server" CssClass="btn btn-confirm" Text="Xác nhận mua hàng" OnClick="btnConfirmPurchase_Click" />
            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-cancel" Text="Hủy" OnClick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
