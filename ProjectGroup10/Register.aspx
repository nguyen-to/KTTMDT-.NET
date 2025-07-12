<%@ Page Title="" Language="C#" MasterPageFile="~/ProjectMaster.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ProjectGroup10.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Css/register.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div class="container">
        <div class="register-container">
            <div class="register-header">
                <i class="fas fa-user-plus"></i>
                <h2>Đăng ký tài khoản</h2>
            </div>
            <!-- Alert Messages -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <div id="divAlert" runat="server" class="alert" role="alert">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
            </asp:Panel>
            
            <div class="register-form">
                <div class="form-group">
                    <label class="form-label">Email <span class="required">*</span></label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Nhập địa chỉ email" TextMode="Email"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                        ControlToValidate="txtEmail" 
                        ErrorMessage="Vui lòng nhập email" 
                        CssClass="text-danger" 
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                        ControlToValidate="txtEmail" 
                        ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" 
                        ErrorMessage="Email không hợp lệ" 
                        CssClass="text-danger" 
                        Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Mật khẩu <span class="required">*</span></label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Nhập mật khẩu" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                        ControlToValidate="txtPassword" 
                        ErrorMessage="Vui lòng nhập mật khẩu" 
                        CssClass="text-danger" 
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revPassword" runat="server" 
                        ControlToValidate="txtPassword" 
                        ValidationExpression="^.{6,}$" 
                        ErrorMessage="Mật khẩu phải có ít nhất 6 ký tự" 
                        CssClass="text-danger" 
                        Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Xác nhận mật khẩu <span class="required">*</span></label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" placeholder="Nhập lại mật khẩu" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                        ControlToValidate="txtConfirmPassword" 
                        ErrorMessage="Vui lòng xác nhận mật khẩu" 
                        CssClass="text-danger" 
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvPassword" runat="server" 
                        ControlToValidate="txtConfirmPassword" 
                        ControlToCompare="txtPassword" 
                        ErrorMessage="Mật khẩu xác nhận không khớp" 
                        CssClass="text-danger" 
                        Display="Dynamic">
                    </asp:CompareValidator>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Họ và tên <span class="required">*</span></label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Nhập họ và tên đầy đủ"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" 
                        ControlToValidate="txtFullName" 
                        ErrorMessage="Vui lòng nhập họ và tên" 
                        CssClass="text-danger" 
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Số điện thoại</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Nhập số điện thoại"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revPhone" runat="server" 
                        ControlToValidate="txtPhone" 
                        ValidationExpression="^[0-9]{10,11}$" 
                        ErrorMessage="Số điện thoại không hợp lệ (10-11 chữ số)" 
                        CssClass="text-danger" 
                        Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </div>
                
                <div class="form-group">
                    <asp:Button ID="btnRegister" runat="server" CssClass="btn btn-register" Text="Đăng ký" OnClick="btnRegister_Click" />
                </div>
                
                <div class="login-link">
                    <p>Đã có tài khoản? <a href="Login.aspx">Đăng nhập ngay</a></p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
