<%@ Page Title="" Language="C#" MasterPageFile="~/ProjectMaster.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="ProjectGroup10.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Css/dashboard.css") %>" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="dashboard-header">
        <div class="container">
            <h1 class="dashboard-title">Admin Dashboard</h1>
            <p class="dashboard-subtitle">Quản lý đơn hàng và theo dõi hoạt động bán hàng</p>

        </div>
        <asp:Button ID="btnLogout" runat="server" 
            CssClass="btn btn-logout" 
            Text="Đăng xuất" 
            OnClick="btnLogout_Click" 
        />
    </div>

    <div class="container">
        <!-- Statistics Cards -->
        <div class="stats-cards">
            <div class="stat-card">
                <div class="stat-icon orders">
                    <i class="fas fa-shopping-cart"></i>
                </div>
                <div class="stat-number">
                    <asp:Label ID="lblTotalOrders" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Tổng đơn hàng</div>
            </div>
            
            <div class="stat-card">
                <div class="stat-icon pending">
                    <i class="fas fa-clock"></i>
                </div>
                <div class="stat-number">
                    <asp:Label ID="lblPendingOrders" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Đang chờ xử lý</div>
            </div>
            
            <div class="stat-card">
                <div class="stat-icon completed">
                    <i class="fas fa-check-circle"></i>
                </div>
                <div class="stat-number">
                    <asp:Label ID="lblCompletedOrders" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Đã hoàn thành</div>
            </div>
            
            <div class="stat-card">
                <div class="stat-icon revenue">
                    <i class="fas fa-dollar-sign"></i>
                </div>
                <div class="stat-number">
                    <asp:Label ID="lblTotalRevenue" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Doanh thu (VNĐ)</div>
            </div>
        </div>

        <!-- Alert Messages -->
        <asp:Panel ID="pnlMessage" runat="server" Visible="false">
            <div id="divAlert" runat="server" class="alert" role="alert">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
        </asp:Panel>

        <!-- Orders Section -->
        <div class="orders-section">
            <div class="section-header">
                <h2 class="section-title">Danh sách đơn hàng</h2>
                <div class="filter-controls">
                    <div class="filter-group">
                        <label>Trạng thái:</label>
                        <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="filter-select">
                            <asp:ListItem Text="Tất cả" Value=""></asp:ListItem>
                            <asp:ListItem Text="Đang chờ" Value="pending"></asp:ListItem>
                            <asp:ListItem Text="Đang xử lý" Value="processing"></asp:ListItem>
                            <asp:ListItem Text="Hoàn thành" Value="completed"></asp:ListItem>
                            <asp:ListItem Text="Đã hủy" Value="cancelled"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="filter-group">
                        <label>Từ ngày:</label>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="filter-select" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="filter-group">
                        <label>Đến ngày:</label>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="filter-select" TextMode="Date"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnFilter" runat="server" CssClass="btn-filter" Text="Lọc" OnClick="btnFilter_Click" />
                    <asp:Button ID="Button1" runat="server" CssClass="btn-filter them" Text="Thêm Sản Phẩm" OnClick="btnAddProduct_Click" />
                    <asp:Button ID="Button2" runat="server" CssClass="btn-filter quanly" Text="Quản Lý Sản Phẩm" OnClick="btnManageProduct_Click" />

                </div>
            </div>

            <!-- Orders Table -->
            <asp:Panel ID="pnlOrdersTable" runat="server">
                <table class="orders-table">
                    <thead>
                        <tr>
                            <th>Mã đơn hàng</th>
                            <th>Khách hàng</th>
                            <th>Sản phẩm</th>
                            <th>Ngày đặt</th>
                            <th>Số lượng</th>
                            <th>Tổng tiền</th>
                            <th>Trạng thái</th>
                            <th>Thao tác</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand" OnItemDataBound="rptOrders_ItemDataBound" >
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <div class="order-id">#<%# Eval("orderId") %></div>
                                    </td>
                                    <td>
                                        <div class="customer-info"><%# Eval("customerName") %></div>
                                        <div class="text-muted"><%# Eval("customerEmail") %></div>
                                    </td>
                                    <td>
                                        <div class="product-name"><%# Eval("productName") %></div>
                                    </td>
                                    <td>
                                        <div class="order-date"><%# Convert.ToDateTime(Eval("orderDate")).ToString("dd/MM/yyyy") %></div>
                                        <div class="text-muted"><%# Convert.ToDateTime(Eval("orderDate")).ToString("HH:mm") %></div>
                                    </td>
                                    <td><%# Eval("quantity") %></td>
                                    <td>
                                        <div class="order-amount"><%# String.Format("{0:N0}", Eval("totalAmount")) %> VNĐ</div>
                                    </td>
                                    <td>
                                        <span class="status-badge status-<%# Eval("status").ToString().ToLower() %>">
                                            <%# Eval("status").ToString() %>
                                        </span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-dropdown">
                                            <asp:ListItem Text="Đang chờ" Value="pending"></asp:ListItem>
                                            <asp:ListItem Text="Đang xử lý" Value="processing"></asp:ListItem>
                                            <asp:ListItem Text="Hoàn thành" Value="completed"></asp:ListItem>
                                            <asp:ListItem Text="Đã hủy" Value="cancelled"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Button ID="btnUpdateStatus" runat="server" 
                                            CssClass="btn-update" 
                                            Text="Cập nhật" 
                                            CommandName="UpdateStatus" 
                                            CommandArgument='<%# Eval("orderId") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </asp:Panel>

            <!-- No Orders Message -->
            <asp:Panel ID="pnlNoOrders" runat="server" Visible="false">
                <div class="no-orders">
                    <i class="fas fa-inbox"></i>
                    <h3>Không có đơn hàng nào</h3>
                    <p>Chưa có đơn hàng nào phù hợp với bộ lọc đã chọn.</p>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
