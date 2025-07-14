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
    public partial class ManageProduct : System.Web.UI.Page
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
                LoadProducts();
            }
        }

        private void LoadCategories()
        {
            try
            {
                string query = "SELECT categoryId, categoryName FROM Categories ORDER BY categoryName";
                DataTable dt = db.ExecuteQuery(query);

                ddlCategoryFilter.DataSource = dt;
                ddlCategoryFilter.DataTextField = "categoryName";
                ddlCategoryFilter.DataValueField = "categoryId";
                ddlCategoryFilter.DataBind();

                // Thêm item "Tất cả"
                ddlCategoryFilter.Items.Insert(0, new ListItem("Tất cả", ""));
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải danh mục: " + ex.Message, "danger");
            }
        }

        private void LoadProducts()
        {
            try
            {
                string query = @"SELECT p.productId, p.productName, p.price, p.description, p.imageUrl, 
                                       c.categoryName
                               FROM Products p
                               LEFT JOIN Categories c ON p.categoryId = c.categoryId";
                DataTable dt = db.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    rptProducts.DataSource = dt;
                    rptProducts.DataBind();
                    pnlProductsTable.Visible = true;
                    pnlNoProducts.Visible = false;
                }
                else
                {
                    pnlProductsTable.Visible = false;
                    pnlNoProducts.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải danh sách sản phẩm: " + ex.Message, "danger");
            }
        }

        
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Edit")
            {
                ShowMessage("Hiện Tại Chưa Thực Hiện Xong chức năng này", "danger");
            }
            else if (e.CommandName == "Delete")
            {
                DeleteProduct(productId);
            }
        }

        private void DeleteProduct(int productId)
        {
            try
            {
                // Kiểm tra xem sản phẩm có trong đơn hàng nào không
                string checkOrderQuery = "SELECT COUNT(*) FROM Orders WHERE productId = @ProductId";
                SqlParameter[] checkParams = {
                    new SqlParameter("@ProductId", productId)
                };

                int orderCount = Convert.ToInt32(db.ExecuteScalar(checkOrderQuery, checkParams));

                if (orderCount > 0)
                {
                    ShowMessage("Không thể xóa sản phẩm này vì đã có đơn hàng liên quan!", "warning");
                    return;
                }

                // Xóa sản phẩm
                string deleteQuery = "DELETE FROM Products WHERE productId = @ProductId";
                SqlParameter[] deleteParams = {
                    new SqlParameter("@ProductId", productId)
                };

                int result = db.ExecuteNonQuery(deleteQuery, deleteParams);

                if (result > 0)
                {
                    ShowMessage("Xóa sản phẩm thành công!", "success");
                    LoadProducts(); // Refresh danh sách
                }
                else
                {
                    ShowMessage("Không thể xóa sản phẩm!", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi xóa sản phẩm: " + ex.Message, "danger");
            }
        }

        protected string GetProductImageUrl(object imageUrl)
        {
                string imagePath = imageUrl.ToString().Trim();
                if (imagePath.StartsWith("http://") || imagePath.StartsWith("https://"))
                {
                    return imagePath;
                }
                return ResolveUrl("~/Images/products/" + imagePath);
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