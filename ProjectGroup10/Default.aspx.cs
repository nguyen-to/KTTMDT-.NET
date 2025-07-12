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

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "BuyProduct")
            {
                int productId = Convert.ToInt32(e.CommandArgument);

                // Chuyển đến trang xác nhận mua hàng
                Response.Redirect($"PurchaseConfirm.aspx?productId={productId}");
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