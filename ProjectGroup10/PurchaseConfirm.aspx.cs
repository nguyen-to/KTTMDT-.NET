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
    public partial class PurchaseConfirm : System.Web.UI.Page
    {
        SqlConnectionServer db = new SqlConnectionServer();
        private int productId;
        private decimal productPrice;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra đăng nhập
            if (Session["UserID"] == null)
            {
                Response.Redirect("RunningWebform.aspx");
                return;
            }

            // Lấy productId từ query string
            if (!int.TryParse(Request.QueryString["productId"], out productId))
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProductInfo();
                UpdateTotalAmount();
            }
        }

        private void LoadProductInfo()
        {
            try
            {
                string query = "SELECT productName, price FROM Products WHERE productId = @ProductId";
                SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", productId)
                };

                DataTable dt = db.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblProductName.Text = row["productName"].ToString();
                    productPrice = Convert.ToDecimal(row["price"]);

                    lblPrice.Text = string.Format("{0:N0}", productPrice);
                }
                else
                {
                    ShowMessage("Không tìm thấy sản phẩm!", "danger");
                    btnConfirmPurchase.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải thông tin sản phẩm: " + ex.Message, "danger");
            }
        }

        private void UpdateTotalAmount()
        {
            try
            {
                int quantity = Convert.ToInt32(txtQuantity.Text);
                decimal totalAmount = productPrice * quantity;
                lblTotalAmount.Text = string.Format("{0:N0}", totalAmount);
            }
            catch
            {
                lblTotalAmount.Text = "0";
            }
        }

        protected void btnIncrease_Click(object sender, EventArgs e)
        {
            int currentQuantity = Convert.ToInt32(txtQuantity.Text);
        }

        protected void btnDecrease_Click(object sender, EventArgs e)
        {
            int currentQuantity = Convert.ToInt32(txtQuantity.Text);
            if (currentQuantity > 1)
            {
                txtQuantity.Text = (currentQuantity - 1).ToString();
                UpdateTotalAmount();
            }
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            int quantity;
            if (int.TryParse(txtQuantity.Text, out quantity))
            {
                if (quantity < 1)
                {
                    txtQuantity.Text = "1";
                }
                UpdateTotalAmount();
            }
            else
            {
                txtQuantity.Text = "1";
                UpdateTotalAmount();
            }
        }

        protected void btnConfirmPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                int quantity = Convert.ToInt32(txtQuantity.Text);
                decimal totalAmount = productPrice * quantity;

                // Tạo đơn hàng
                string insertQuery = @"INSERT INTO Orders (orderDate, totalAmount, quantity, price, productId, userId, status) 
                                     VALUES (@OrderDate, @TotalAmount, @Quantity, @Price, @ProductId, @UserId, @Status)";

                SqlParameter[] parameters = {
                    new SqlParameter("@OrderDate", DateTime.Now),
                    new SqlParameter("@TotalAmount", totalAmount),
                    new SqlParameter("@Quantity", quantity),
                    new SqlParameter("@Price", productPrice),
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@Status", "pending")
                };

                int result = db.ExecuteNonQuery(insertQuery, parameters);

                if (result > 0)
                {
                    ShowMessage("Mua hàng thành công! Đơn hàng đang được xử lý.", "success");
                    btnConfirmPurchase.Enabled = false;

                    // Chuyển hướng sau 2 giây
                    Response.AddHeader("REFRESH", "2;URL=MyOrders.aspx");
                }
                else
                {
                    ShowMessage("Có lỗi xảy ra khi tạo đơn hàng!", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi xử lý đơn hàng: " + ex.Message, "danger");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
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