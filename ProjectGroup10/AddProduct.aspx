<%@ Page Title="" Language="C#" MasterPageFile="~/ProjectMaster.Master" AutoEventWireup="true" CodeBehind="AddProduct.aspx.cs" Inherits="ProjectGroup10.AddProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Css/addproduct.css") %>" rel="stylesheet" type="text/css" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div class="page-header">
        <div class="container">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="Dashboard.aspx">Dashboard</a></li>
                    <li class="breadcrumb-item active">Thêm sản phẩm</li>
                </ol>
            </nav>
            <h1 class="page-title">Thêm sản phẩm mới</h1>
            <p>Thêm sản phẩm nội thất vào cửa hàng</p>
        </div>
    </div>

    <div class="container">
        <div class="form-container">
            <!-- Alert Messages -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <div id="divAlert" runat="server" class="alert" role="alert">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
            </asp:Panel>

            <!-- Product Form -->
            <div class="form-section">
                <h3 class="section-title">Thông tin sản phẩm</h3>
                
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label">Tên sản phẩm <span class="required">*</span></label>
                            <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" placeholder="Nhập tên sản phẩm" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvProductName" runat="server" 
                                ControlToValidate="txtProductName" 
                                ErrorMessage="Vui lòng nhập tên sản phẩm" 
                                CssClass="text-danger" 
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label">Danh mục <span class="required">*</span></label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select">
                                <asp:ListItem Text="-- Chọn danh mục --" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            
                            <asp:RequiredFieldValidator 
                                ID="rfvCategory" runat="server" 
                                ControlToValidate="ddlCategory" 
                                InitialValue=""
                                ErrorMessage="Vui lòng chọn danh mục" 
                                CssClass="text-danger" 
                                Display="Dynamic" />
                         </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label">Giá (VNĐ) <span class="required">*</span></label>
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="Nhập giá sản phẩm" TextMode="Number"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPrice" runat="server" 
                                ControlToValidate="txtPrice" 
                                ErrorMessage="Vui lòng nhập giá sản phẩm" 
                                CssClass="text-danger" 
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvPrice" runat="server" 
                                ControlToValidate="txtPrice" 
                                MinimumValue="1" 
                                MaximumValue="999999999" 
                                Type="Integer" 
                                ErrorMessage="Giá phải lớn hơn 0" 
                                CssClass="text-danger" 
                                Display="Dynamic">
                            </asp:RangeValidator>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="form-label">Mô tả</label>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="Nhập mô tả sản phẩm" TextMode="MultiLine" Rows="4" MaxLength="200"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label class="form-label">URL hình ảnh</label>
                    <asp:TextBox ID="txtImageUrl" runat="server" CssClass="form-control" placeholder="Nhập URL hình ảnh sản phẩm" MaxLength="200"></asp:TextBox>
                    
                    <button type="button" class="btn-preview" onclick="previewImage()">Xem trước</button>
                    
                    <div id="placeholder" class="image-preview-container" >
                    </div>
                        <img id="previewImg" class="preview-image" />
                </div>
            </div>

            <!-- Form Actions -->
            <div class="form-actions">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Thêm sản phẩm" OnClick="btnSave_Click" />
                <a href="Dashboard.aspx" class="btn btn-secondary">Hủy</a>
            </div>
        </div>
    </div>

    <script>
        function previewImage() {
            var url = document.getElementById('<%= txtImageUrl.ClientID %>').value;
            var img = document.getElementById('previewImg');
            var placeholder = document.getElementById('placeholder');

            if (url.trim() === '') {
                img.style.display = 'none';
                placeholder.style.display = 'block';
                return;
            }

            img.onload = function () {
                placeholder.style.display = 'none';
                img.style.display = 'block';
            };

            img.onerror = function () {
                placeholder.style.display = 'block';
                img.style.display = 'none';
                alert('Không thể tải hình ảnh. Vui lòng kiểm tra lại URL.');
            };

            img.src = url;
        }
    </script>
</asp:Content>
