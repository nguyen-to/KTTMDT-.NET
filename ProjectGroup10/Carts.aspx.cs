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
    public partial class Carts : System.Web.UI.Page
    {
        SqlConnectionServer db = new SqlConnectionServer();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra đăng nhập
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCartItems();
            }
        }

        private void LoadCartItems()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                string query = @"SELECT c.cartId, c.productId, c.quantity, 
                                       p.productName, p.price, p.imageUrl
                               FROM Carts c
                               INNER JOIN Products p ON c.productId = p.productId
                               WHERE c.userId = @UserId
                               ORDER BY c.cartId DESC";

                SqlParameter[] parameters = {
                    new SqlParameter("@UserId", userId)
                };

                DataTable dt = db.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    rptCartItems.DataSource = dt;
                    rptCartItems.DataBind();

                    pnlCartItems.Visible = true;
                    pnlEmptyCart.Visible = false;
                    pnlCartSummary.Visible = true;

                    // Cập nhật thông tin tổng kết
                    UpdateCartSummary(dt);
                }
                else
                {
                    pnlCartItems.Visible = false;
                    pnlEmptyCart.Visible = true;
                    pnlCartSummary.Visible = false;
                }

                // Cập nhật số lượng giỏ hàng
                lblCartCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải giỏ hàng: " + ex.Message, "danger");
            }
        }

        private void UpdateCartSummary(DataTable cartData)
        {
            try
            {
                int totalItems = cartData.Rows.Count;
                int totalQuantity = 0;
                decimal totalAmount = 0;

                foreach (DataRow row in cartData.Rows)
                {
                    int quantity = Convert.ToInt32(row["quantity"]);
                    decimal price = Convert.ToDecimal(row["price"]);

                    totalQuantity += quantity;
                    totalAmount += quantity * price;
                }

                lblTotalItems.Text = totalItems.ToString();
                lblTotalQuantity.Text = totalQuantity.ToString();
                lblTotalAmount.Text = String.Format("{0:N0}", totalAmount);
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tính tổng: " + ex.Message, "danger");
            }
        }

        protected void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BuyNow")
                {
                    // Tách commandArgument: productId,quantity,cartId
                    string[] args = e.CommandArgument.ToString().Split(',');
                    int productId = Convert.ToInt32(args[0]);
                    int quantity = Convert.ToInt32(args[1]);
                    int cartId = Convert.ToInt32(args[2]);

                    // Chuyển đến trang PurchaseConfirm với productId và quantity
                    Response.Redirect($"PurchaseConfirm.aspx?productId={productId}&quantity={quantity}&cartId={cartId}");
                }
                else if (e.CommandName == "Remove")
                {
                    int cartId = Convert.ToInt32(e.CommandArgument);
                    RemoveFromCart(cartId);
                }
                else if (e.CommandName == "UpdateQuantity")
                {
                    // Tách commandArgument: cartId,change
                    string[] args = e.CommandArgument.ToString().Split(',');
                    int cartId = Convert.ToInt32(args[0]);
                    int change = Convert.ToInt32(args[1]);

                    UpdateQuantity(cartId, change);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi xử lý: " + ex.Message, "danger");
            }
        }

        private void RemoveFromCart(int cartId)
        {
            try
            {
                string query = "DELETE FROM Carts WHERE cartId = @CartId";
                SqlParameter[] parameters = {
                    new SqlParameter("@CartId", cartId)
                };

                int result = db.ExecuteNonQuery(query, parameters);

                if (result > 0)
                {
                    ShowMessage("Đã xóa sản phẩm khỏi giỏ hàng!", "success");
                    LoadCartItems(); // Refresh lại danh sách

                    // Cập nhật session cart count
                    UpdateSessionCartCount();
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

        private void UpdateQuantity(int cartId, int change)
        {
            try
            {
                // Lấy quantity hiện tại
                string getQuery = "SELECT quantity, productId FROM Carts WHERE cartId = @CartId";
                SqlParameter[] getParams = {
                    new SqlParameter("@CartId", cartId)
                };

                DataTable dt = db.ExecuteQuery(getQuery, getParams);
                if (dt.Rows.Count == 0)
                {
                    ShowMessage("Không tìm thấy sản phẩm trong giỏ hàng!", "danger");
                    return;
                }

                int currentQuantity = Convert.ToInt32(dt.Rows[0]["quantity"]);
                int productId = Convert.ToInt32(dt.Rows[0]["productId"]);
                int newQuantity = currentQuantity + change;

                if (newQuantity <= 0)
                {
                    // Xóa sản phẩm nếu quantity <= 0
                    RemoveFromCart(cartId);
                    return;
                }

                
                // Cập nhật quantity
                string updateQuery = "UPDATE Carts SET quantity = @Quantity WHERE cartId = @CartId";
                SqlParameter[] updateParams = {
                    new SqlParameter("@Quantity", newQuantity.ToString()),
                    new SqlParameter("@CartId", cartId)
                };

                int result = db.ExecuteNonQuery(updateQuery, updateParams);

                if (result > 0)
                {
                    LoadCartItems(); // Refresh lại danh sách
                }
                else
                {
                    ShowMessage("Không thể cập nhật số lượng!", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi cập nhật số lượng: " + ex.Message, "danger");
            }
        }

        private void UpdateSessionCartCount()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                string query = "SELECT COUNT(*) FROM Carts WHERE userId = @UserId";
                SqlParameter[] parameters = {
                    new SqlParameter("@UserId", userId)
                };

                int cartCount = Convert.ToInt32(db.ExecuteScalar(query, parameters));
                Session["CartCount"] = cartCount;
            }
            catch
            {
                Session["CartCount"] = 0;
            }
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