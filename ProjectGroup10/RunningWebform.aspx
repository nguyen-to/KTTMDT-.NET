<%@ Page Title="" Language="C#" MasterPageFile="~/ProjectMaster.Master" AutoEventWireup="true" CodeBehind="RunningWebform.aspx.cs" Inherits="ProjectGroup10.RunningWebform" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="<%= ResolveUrl("~/Css/style.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="welcome-container">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-12">
                    <div class="welcome-card">
                        <div class="welcome-icon">
                            <i class="fas fa-couch"></i>
                        </div>
                        
                        <h1 class="welcome-title">Chào mừng đến với</h1>
                        <div class="divider"></div>
                        <h2 class="welcome-subtitle"><span class="brand-highlight">FurnitureStore</span></h2>
                        
                        <p class="welcome-message">
                            Bạn vui lòng <strong>đăng nhập</strong> để truy cập và khám phá bộ sưu tập nội thất cao cấp của chúng tôi.
                        </p>
                        
                        <div class="auth-buttons">
                            <asp:Button ID="btnLogin" runat="server" 
                                CssClass="btn btn-login btn-auth" 
                                Text="Đăng nhập" 
                                OnClick="btnLogin_Click" />
                            
                            <asp:Button ID="btnRegister" runat="server" 
                                CssClass="btn btn-register btn-auth" 
                                Text="Đăng ký" 
                                OnClick="btnRegister_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
