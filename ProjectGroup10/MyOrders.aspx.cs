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
    public partial class MyOrders : System.Web.UI.Page
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
                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                string query = @"SELECT o.orderId, o.orderDate, o.totalAmount, o.quantity, 
                                       o.price, o.status, p.productName
                               FROM Orders o
                               INNER JOIN Products p ON o.productId = p.productId
                               WHERE o.userId = @UserId
                               ORDER BY o.orderDate DESC";

                SqlParameter[] parameters = {
                    new SqlParameter("@UserId", userId)
                };

                DataTable dt = db.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    rptOrders.DataSource = dt;
                    rptOrders.DataBind();
                    pnlNoOrders.Visible = false;
                }
                else
                {
                    pnlNoOrders.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // Log error
                pnlNoOrders.Visible = true;
            }
        }

        protected string GetStatusText(string status)
        {
            switch (status?.ToLower())
            {
                case "pending":
                    return "Đang xử lý";
                case "completed":
                    return "Hoàn thành";
                case "cancelled":
                    return "Đã hủy";
                default:
                    return "Không xác định";
            }
        }
    }
}