<%@ Page Title="" Language="C#" MasterPageFile="~/ProjectMaster.Master" AutoEventWireup="true" CodeBehind="ManageProduct.aspx.cs" Inherits="ProjectGroup10.ManageProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Css/manageproduct.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div class="container">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="Dashboard.aspx">Dashboard</a></li>
                    <li class="breadcrumb-item active">Quản lý sản phẩm</li>
                </ol>
            </nav>
            <div class="row align-items-center">
                <div class="col-md-8">
                    <h1 class="page-title">Quản lý sản phẩm</h1>
                    <p>Quản lý danh sách sản phẩm trong cửa hàng</p>
                </div>
                <div class="col-md-4 text-end">
                    <a href="AddProduct.aspx" class="btn-add">
                        <i class="fas fa-plus"></i> Thêm sản phẩm
                    </a>
                </div>
            </div>
        </div>
    </div>

    <div class="container">
        <asp:Panel ID="pnlMessage" runat="server" Visible="false">
            <div id="divAlert" runat="server" class="alert" role="alert">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
        </asp:Panel>

        <div class="products-section">
            <div class="section-header">
                <h2 class="section-title">Danh sách sản phẩm</h2>
                <div class="filter-controls">
                    <div class="filter-group">
                        <label>Danh mục:</label>
                        <asp:DropDownList ID="ddlCategoryFilter" runat="server" CssClass="filter-select">
                            <asp:ListItem Text="Tất cả" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="filter-group">
                        <label>Tìm kiếm:</label>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="filter-select" placeholder="Tên sản phẩm..."></asp:TextBox>
                    </div>
                    <asp:Button ID="btnFilter" runat="server" CssClass="btn-filter" Text="Lọc" OnClick="btnFilter_Click" />
                </div>
            </div>

            <asp:Panel ID="pnlProductsTable" runat="server">
                <table class="products-table">
                    <thead>
                        <tr>
                            <th>Hình ảnh</th>
                            <th>Tên sản phẩm</th>
                            <th>Danh mục</th>
                            <th>Giá</th>
                            <th>Mô tả</th>
                            <th>Thao tác</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Image ID="imgProduct" runat="server" 
                                            CssClass="product-image"
                                            ImageUrl='<%# (Eval("imageUrl")) %>' 
                                            AlternateText='<%# Eval("productName") %>' />
                                    </td>
                                    <td>
                                        <div class="product-name"><%# Eval("productName") %></div>
                                    </td>
                                    <td>
                                        <span class="product-category"><%# Eval("categoryName") %></span>
                                    </td>
                                    <td>
                                        <div class="product-price"><%# String.Format("{0:N0}", Eval("price")) %> VNĐ</div>
                                    </td>
                                   
                                    <td>
                                        <div style="max-width: 200px; overflow: hidden; text-overflow: ellipsis;">
                                            <%# Eval("description") %>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnEdit" runat="server" 
                                            CssClass="btn-edit" 
                                            Text="Sửa" 
                                            CommandName="Edit" 
                                            CommandArgument='<%# Eval("productId") %>' />
                                        <asp:Button ID="btnDelete" runat="server" 
                                            CssClass="btn-delete" 
                                            Text="Xóa" 
                                            CommandName="Delete" 
                                            CommandArgument='<%# Eval("productId") %>'
                                            />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnlNoProducts" runat="server" Visible="false">
                <div class="no-products">
                    <i class="fas fa-box-open"></i>
                    <h3>Không có sản phẩm nào</h3>
                    <p>Chưa có sản phẩm nào phù hợp với bộ lọc đã chọn.</p>
                    <a href="AddProduct.aspx" class="btn-add">Thêm sản phẩm đầu tiên</a>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
