using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectGroup10
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnectionServer db = new SqlConnectionServer();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string email = txtEmail.Text.Trim();
                    string password = txtPassword.Text.Trim();

                    // Kiểm tra thông tin đăng nhập
                    DataTable user = AuthenticateUser(email, password);

                    if (user != null && user.Rows.Count > 0)
                    {
                        // Đăng nhập thành công
                        DataRow userRow = user.Rows[0];

                        // Lưu thông tin vào Session
                        Session["UserID"] = userRow["UserID"];
                        Session["Email"] = userRow["Email"];
                        Session["FullName"] = userRow["FullName"];
                        Session["Role"] = userRow["Role"];

                        // Hiển thị thông báo thành công
                        ShowMessage("Đăng nhập thành công! Đang chuyển hướng...", "success");

                        // Chuyển hướng dựa trên role
                        if (userRow["Role"].ToString() == "Admin")
                        {
                            Response.AddHeader("REFRESH", "1;URL=Dashboard.aspx");
                        }
                        else
                        {
                            // Kiểm tra có URL return không
                            string returnUrl = Request.QueryString["ReturnUrl"];
                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                Response.AddHeader("REFRESH", "1;URL=" + returnUrl);
                            }
                            else
                            {
                                Response.AddHeader("REFRESH", "1;URL=Default.aspx");
                            }
                        }
                    }
                    else
                    {
                        ShowMessage("Email hoặc mật khẩu không chính xác!", "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Có lỗi xảy ra: " + ex.Message, "danger");
                }
            }
        }

        private DataTable AuthenticateUser(string email, string password)
        {
            try
            {
                string query = @"SELECT userId, email, fullName, role 
                                FROM Users 
                                WHERE email = @email AND password = @Password";

                SqlParameter[] parameters = {
                    new SqlParameter("@email", email),
                    new SqlParameter("@password", password)
                };

                DataTable result = db.ExecuteQuery(query, parameters);
                return result;
            }
            catch
            {
                return null;
            }
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;

            // Xóa các class cũ và thêm class mới
            divAlert.Attributes["class"] = $"alert alert-{type}";

            // Thêm icon tương ứng
            string icon = "";
            switch (type)
            {
                case "success":
                    icon = "<i class='fas fa-check-circle'></i> ";
                    break;
                case "danger":
                    icon = "<i class='fas fa-exclamation-triangle'></i> ";
                    break;
                case "warning":
                    icon = "<i class='fas fa-exclamation-circle'></i> ";
                    break;
                case "info":
                    icon = "<i class='fas fa-info-circle'></i> ";
                    break;
            }

            lblMessage.Text = icon + message;
        }
    }
}