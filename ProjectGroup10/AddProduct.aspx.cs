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
    public partial class AddProduct : System.Web.UI.Page
    {
        SqlConnectionServer db = new SqlConnectionServer();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra đăng nhập và quyền admin
            if (Session["UserID"] == null || Session["Role"]?.ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                LoadCategories();
            }

        }

        private void LoadCategories()
        {
            try
            {
                string query = "SELECT categoryId, categoryName FROM Categories ORDER BY categoryName";
                DataTable dt = db.ExecuteQuery(query);

                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "categoryName";
                ddlCategory.DataValueField = "categoryId";
                ddlCategory.DataBind();

                // Thêm item mặc định
                ddlCategory.Items.Insert(0, new ListItem("-- Chọn danh mục --", ""));
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải danh mục: " + ex.Message, "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Lấy dữ liệu từ form
                    string productName = txtProductName.Text.Trim();
                    int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                    int price = Convert.ToInt32(txtPrice.Text.Trim());
                    string description = txtDescription.Text.Trim();
                    string imageUrl = txtImageUrl.Text.Trim();

                    // Kiểm tra tên sản phẩm đã tồn tại
                    if (IsProductNameExists(productName))
                    {
                        ShowMessage("Tên sản phẩm đã tồn tại. Vui lòng chọn tên khác.", "warning");
                        return;
                    }
                    // Thêm sản phẩm vào database
                    if (AddProducts(productName, categoryId, price, description, imageUrl))
                    {
                        ShowMessage("Thêm sản phẩm thành công!", "success");
                        ClearForm();

                        // Chuyển hướng về dashboard sau 2 giây
                        Response.AddHeader("REFRESH", "2;URL=Dashboard.aspx");
                    }
                    else
                    {
                        ShowMessage($"product name {productName}, price {price}  ,category {categoryId} , description {description}, imageUrl {imageUrl}", "danger");

                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Lỗi: " + ex.Message, "danger");
                }
            }
        }

        private bool IsProductNameExists(string productName)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Products WHERE productName = @ProductName";
                SqlParameter[] parameters = {
                    new SqlParameter("@ProductName", productName)
                };

                int count = Convert.ToInt32(db.ExecuteScalar(query, parameters));
                return count > 0;
            }
            catch
            {
                return false;
            }
        }

        private bool AddProducts(string productName, int categoryId, int price, string description, string imageUrl)
        {
            try
            {
                string query = @"INSERT INTO Products (productName, categoryId, price, description, imageUrl) 
                                VALUES (@ProductName, @CategoryId, @Price, @Description, @ImageUrl)";

                SqlParameter[] parameters = {
                    new SqlParameter("@ProductName", productName),
                    new SqlParameter("@CategoryId", categoryId),
                    new SqlParameter("@Price", price),
                    new SqlParameter("@Description", description),
                    new SqlParameter("@ImageUrl", imageUrl),
                };

                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error adding product: " + ex.Message);
                return false;
            }
        }

        private void ClearForm()
        {
            txtProductName.Text = "";
            ddlCategory.SelectedIndex = 0;
            txtPrice.Text = "";
            txtDescription.Text = "";
            txtImageUrl.Text = "";
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            divAlert.Attributes["class"] = $"alert alert-{type}";

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
