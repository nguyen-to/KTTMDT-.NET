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
    public partial class Default : System.Web.UI.Page
    {
        SqlConnectionServer db = new SqlConnectionServer();
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra đăng nhập
            if (Session["UserID"] == null)
            {
                Response.Redirect("RunningWebform.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProducts();
            }
            UpdateCartCount();
        }

        private void LoadProducts()
        {
            try
            {
                string query = @"SELECT productId, productName, description, price, imageUrl 
                               FROM Products 
                               ORDER BY productName";

                DataTable dt = db.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    rptProducts.DataSource = dt;
                    rptProducts.DataBind();
                    pnlNoProducts.Visible = false;
                }
                else
                {
                    pnlNoProducts.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải danh sách sản phẩm: " + ex.Message, "danger");
            }
        }

        protected void btnViewOrders_Click(object sender, EventArgs e)
        {
            // Chuyển đến trang danh sách sản phẩm đã mua
            Response.Redirect("MyOrders.aspx");
        }
        protected void btnViewCarts_Click(object sender, EventArgs e)
        {
            // Chuyển đến trang danh sách sản phẩm đã mua
            Response.Redirect("Carts.aspx");
        }
        
        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "BuyProduct")
            {
                int productId = Convert.ToInt32(e.CommandArgument);

                // Chuyển đến trang xác nhận mua hàng
                Response.Redirect($"PurchaseConfirm.aspx?productId={productId}");
            }
            if(e.CommandName == "CartProduct")
            {
                try
                {
                    int productId = Convert.ToInt32(e.CommandArgument);
                    int userId = Convert.ToInt32(Session["UserID"]);

                    // Thêm sản phẩm vào giỏ hàng
                    if (AddToCart(userId, productId))
                    {
                        ShowMessage("Đã thêm sản phẩm vào giỏ hàng!", "success");
                        UpdateCartCount(); // Cập nhật số lượng giỏ hàng
                    }
                    else
                    {
                        ShowMessage("Có lỗi xảy ra khi thêm vào giỏ hàng!", "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Lỗi: " + ex.Message, "danger");
                }
            }
        }
        private bool AddToCart(int userId, int productId)
        {
            try
            {
                // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
                string checkCartQuery = "SELECT cartId, quantity FROM Carts WHERE userId = @UserId AND productId = @ProductId";
                SqlParameter[] checkCartParams = {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@ProductId", productId)
                };

                DataTable existingCart = db.ExecuteQuery(checkCartQuery, checkCartParams);

                if (existingCart.Rows.Count > 0)
                {
                    // Sản phẩm đã có trong giỏ hàng - tăng số lượng
                    int currentQuantity = Convert.ToInt32(existingCart.Rows[0]["quantity"]);
                    int newQuantity = currentQuantity + 1;

                    string updateQuery = "UPDATE Carts SET quantity = @Quantity WHERE userId = @UserId AND productId = @ProductId";
                    SqlParameter[] updateParams = {
                        new SqlParameter("@Quantity", newQuantity),
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@ProductId", productId)
                    };

                    return db.ExecuteNonQuery(updateQuery, updateParams) > 0;
                }
                else
                {
                    // Thêm sản phẩm mới vào giỏ hàng
                    string insertQuery = "INSERT INTO Carts (userId, productId, quantity) VALUES (@UserId, @ProductId, @Quantity)";
                    SqlParameter[] insertParams = {
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@ProductId", productId),
                        new SqlParameter("@Quantity", 1)
                    };

                    return db.ExecuteNonQuery(insertQuery, insertParams) > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error adding to cart: " + ex.Message);
                return false;
            }
        }
        private void UpdateCartCount()
        {
            try
            {
                if (Session["UserID"] != null)
                {
                    int userId = Convert.ToInt32(Session["UserID"]);

                    // Đếm số lượng sản phẩm khác nhau trong giỏ hàng
                    string query = "SELECT COUNT(*) FROM Carts WHERE userId = @UserId";
                    SqlParameter[] parameters = {
                        new SqlParameter("@UserId", userId)
                    };

                    int cartCount = Convert.ToInt32(db.ExecuteScalar(query, parameters));

                    // Cập nhật session
                    Session["CartCount"] = cartCount;

                    // Cập nhật hiển thị
                    if (cartCount > 0)
                    {

                        // Cập nhật text của button để hiển thị số lượng
                        btnViewCarts.Text = $"Giỏ Hàng ({cartCount})";
                    }
                    else
                    {
                        btnViewCarts.Text = "Giỏ Hàng";
                    }
                }
                else
                {
                    Session["CartCount"] = 0;
                    btnViewCarts.Text = "Giỏ Hàng";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error updating cart count: " + ex.Message);
                Session["CartCount"] = 0;
                btnViewCarts.Text = "Giỏ Hàng";
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                // Xóa tất cả session
                Session.Clear();
                Session.Abandon();

                // Xóa cookies nếu có
                if (Request.Cookies["UserInfo"] != null)
                {
                    HttpCookie cookie = new HttpCookie("UserInfo");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                // Chuyển hướng đến trang login
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ShowMessage("Có lỗi xảy ra khi đăng xuất: " + ex.Message, "alert-danger");
            }
        }
        protected string GetProductImageUrl(object imageUrl)
        {
            string imagePath = imageUrl.ToString();

            // Nếu là URL đầy đủ (http/https)
            if (imagePath.StartsWith("http://") || imagePath.StartsWith("https://"))
            {
                return imagePath;
            }

            // Nếu là đường dẫn local
            return ResolveUrl("~/Image/" + imagePath);
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