using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectGroup10
{
    public partial class Register : System.Web.UI.Page
    {
        SqlConnectionServer db = new SqlConnectionServer();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserID"] != null)
            //{
            //    Response.Redirect("Default.aspx");
            //}
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Kiểm tra email đã tồn tại
                    if (IsEmailExists(txtEmail.Text.Trim()))
                    {
                        ShowMessage("Email đã được sử dụng. Vui lòng sử dụng email khác.", "danger");
                        return;
                    }

                    // Tạo tài khoản mới
                    if (CreateUser())
                    {
                        ShowMessage("Đăng ký thành công! Vui lòng đăng nhập để tiếp tục.", "success");

                        Response.AddHeader("REFRESH", "2;URL=Login.aspx");
                    }
                    else
                    {
                        ShowMessage("Có lỗi xảy ra trong quá trình đăng ký. Vui lòng thử lại.", "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Có lỗi xảy ra: " + ex.Message, "danger");
                }
            }
            
        }
        private bool IsEmailExists(string email)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users WHERE email = @email";
                SqlParameter[] parameters = {
                    new SqlParameter("@email", email)
                };

                int count = Convert.ToInt32(db.ExecuteScalar(query, parameters));
                return count > 0;
            }
            catch
            {
                return false;
            }
        }

        private bool CreateUser()
        {
            try
            {

                string query = @"INSERT INTO Users (email, password,fullName, phone, role) 
                                VALUES (  @email,@password, @fullName, @phone, @role)";

                SqlParameter[] parameters = {
                    new SqlParameter("@email", txtEmail.Text.Trim()),
                    new SqlParameter("@password", txtPassword.Text.Trim()),
                    new SqlParameter("@fullName", txtFullName.Text.Trim()),
                    new SqlParameter("@phone", string.IsNullOrEmpty(txtPhone.Text.Trim()) ? (object)DBNull.Value : txtPhone.Text.Trim()),
                    new SqlParameter("@role", "User")
                };

                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch
            {
                return false;
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