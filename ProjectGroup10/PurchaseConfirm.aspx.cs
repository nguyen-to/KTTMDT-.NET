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
        private SqlConnectionServer db = new SqlConnectionServer();
        private int productId;
        private int productPrice;

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
            LoadProductInfo();
            if (!IsPostBack)
            {
                txtQuantity.Text = "1";
                UpdateTotalAmount();
            }
        }

        private void LoadProductInfo()
        {
            try
            {
                string query = "SELECT productName, price FROM Products WHERE productId = @ProductId";
                SqlParameter[] parameters = { new SqlParameter("@ProductId", productId) };
                DataTable dt = db.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblProductName.Text = row["productName"].ToString();

                    if (row["price"] != DBNull.Value)
                    {
                        productPrice = (int)row["price"];
                        lblPrice.Text = string.Format("{0:N0} VNĐ", productPrice);
                        ShowMessage($"Price {productPrice}", "danger");
                    }
                    else
                    {
                        ShowMessage("Giá sản phẩm bị thiếu!", "danger");
                        btnConfirmPurchase.Enabled = false;
                    }
                }

                else
                {
                    ShowMessage("Không tìm thấy sản phẩm!", "danger");
                    btnConfirmPurchase.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi: " + ex.Message, "danger");
            }
        }

        private void UpdateTotalAmount()
        {
            int quantity = GetQuantity();
            int totalAmount = productPrice * quantity;
            lblTotalAmount.Text = string.Format("{0:N0} VNĐ", totalAmount);
        }

        private int GetQuantity()
        {
            if (int.TryParse(txtQuantity.Text, out int quantity) && quantity >= 1 && quantity <= 999)
            {
                return quantity;
            }
            txtQuantity.Text = "1";
            return 1;
        }

        protected void btnIncrease_Click(object sender, EventArgs e)
        {
            int quantity = GetQuantity();
            if (quantity < 999)
            {
                txtQuantity.Text = (quantity + 1).ToString();
                UpdateTotalAmount();
            }
        }

        protected void btnDecrease_Click(object sender, EventArgs e)
        {
            int quantity = GetQuantity();
            if (quantity > 1)
            {
                txtQuantity.Text = (quantity - 1).ToString();
                UpdateTotalAmount();
            }
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            UpdateTotalAmount();
        }

        protected void btnConfirmPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                int quantity = GetQuantity();
                int totalAmount = productPrice * quantity;
                Response.Write("Giá sản phẩm là: " + productPrice);
                Response.Write("Tỏng số là: " + totalAmount);
                string query = @"INSERT INTO Orders (userId, productId, quantity, price, totalAmount, orderDate, status) 
                                VALUES (@UserId, @ProductId, @Quantity, @Price, @TotalAmount, @OrderDate, @Status)";

                if(productPrice == 0 || totalAmount == 0)
                {
                    ShowMessage($"Đặt hàng thất bại! {productPrice } và {totalAmount}", "danger");
                    return;
                }    
                SqlParameter[] parameters = {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@Quantity", quantity),
                    new SqlParameter("@Price", productPrice),
                    new SqlParameter("@TotalAmount", totalAmount),
                    new SqlParameter("@OrderDate", DateTime.Now),
                    new SqlParameter("@Status", "Pending")
                };

                int result = db.ExecuteNonQuery(query, parameters);

                if (result > 0)
                {
                    ShowMessage("Đặt hàng thành công!", "success");
                    btnConfirmPurchase.Enabled = false;
                }
                else
                {
                    ShowMessage("Đặt hàng thất bại!", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi: " + ex.Message, "danger");
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
        }
    }
}