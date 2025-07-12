<%@ Page Title="" Language="C#" MasterPageFile="~/ProjectMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProjectGroup10.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Css/login.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="login-container">
            <div class="login-header">
                <i class="fas fa-sign-in-alt"></i>
                <h2>Đăng nhập</h2>
                <p class="text-muted">Đăng nhập để tiếp tục mua sắm</p>
            </div>
            
            <!-- Alert Messages -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <div id="divAlert" runat="server" class="alert" role="alert">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
            </asp:Panel>
            
            <div class="login-form">
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
                    <div class="forgot-password">
                        <a href="ForgotPassword.aspx">Quên mật khẩu?</a>
                    </div>
                </div>
                
                <div class="form-group">
                    <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-login" Text="Đăng nhập" OnClick="btnLogin_Click" />
                </div>
                
                <div class="register-link">
                    <p>Chưa có tài khoản? <a href="Register.aspx">Đăng ký ngay</a></p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
