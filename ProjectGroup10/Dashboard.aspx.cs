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
    public partial class Dashboard : System.Web.UI.Page
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
                LoadStatistics();
                LoadOrders();
            }
        }

        private void LoadStatistics()
        {
            try
            {
                // Tổng số đơn hàng
                string totalOrdersQuery = "SELECT COUNT(*) FROM Orders";
                int totalOrders = Convert.ToInt32(db.ExecuteScalar(totalOrdersQuery));
                lblTotalOrders.Text = totalOrders.ToString();

                // Đơn hàng đang chờ xử lý
                string pendingOrdersQuery = "SELECT COUNT(*) FROM Orders WHERE status = 'pending'";
                int pendingOrders = Convert.ToInt32(db.ExecuteScalar(pendingOrdersQuery));
                lblPendingOrders.Text = pendingOrders.ToString();

                // Đơn hàng đã hoàn thành
                string completedOrdersQuery = "SELECT COUNT(*) FROM Orders WHERE status = 'completed'";
                int completedOrders = Convert.ToInt32(db.ExecuteScalar(completedOrdersQuery));
                lblCompletedOrders.Text = completedOrders.ToString();

                // Tổng doanh thu
                string revenueQuery = "SELECT ISNULL(SUM(totalAmount), 0) FROM Orders WHERE status = 'completed'";
                decimal totalRevenue = Convert.ToDecimal(db.ExecuteScalar(revenueQuery));
                lblTotalRevenue.Text = string.Format("{0:N0}", totalRevenue);
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải thống kê: " + ex.Message, "danger");
            }
        }

        private void LoadOrders()
        {
            try
            {
                string query = @"SELECT o.orderId, o.orderDate, o.totalAmount, o.quantity, o.status,
                                       p.productName, u.fullName as customerName, u.email as customerEmail
                               FROM Orders o
                               INNER JOIN Products p ON o.productId = p.productId
                               INNER JOIN Users u ON o.userId = u.userId";

                // Thêm điều kiện lọc
                string whereClause = BuildWhereClause();
                if (!string.IsNullOrEmpty(whereClause))
                {
                    query += " WHERE " + whereClause;
                }

                query += " ORDER BY o.orderDate DESC";

                DataTable dt = db.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    rptOrders.DataSource = dt;
                    rptOrders.DataBind();
                    pnlOrdersTable.Visible = true;
                    pnlNoOrders.Visible = false;
                }
                else
                {
                    pnlOrdersTable.Visible = false;
                    pnlNoOrders.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải danh sách đơn hàng: " + ex.Message, "danger");
            }
        }

        private string BuildWhereClause()
        {
            string whereClause = "";

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(ddlStatusFilter.SelectedValue))
            {
                whereClause += $"o.status = '{ddlStatusFilter.SelectedValue}'";
            }

            // Lọc theo ngày
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                if (!string.IsNullOrEmpty(whereClause)) whereClause += " AND ";
                whereClause += $"o.orderDate >= '{txtFromDate.Text}'";
            }

            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                if (!string.IsNullOrEmpty(whereClause)) whereClause += " AND ";
                whereClause += $"o.orderDate <= '{txtToDate.Text} 23:59:59'";
            }

            return whereClause;
        }

        
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }

        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "UpdateStatus")
            {
                try
                {
                    int orderId = Convert.ToInt32(e.CommandArgument);

                    // Tìm dropdown trong item hiện tại
                    DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
                    string newStatus = ddlStatus.SelectedValue;

                    // Cập nhật trạng thái
                    string updateQuery = "UPDATE Orders SET status = @Status WHERE orderId = @OrderId";
                    SqlParameter[] parameters = {
                        new SqlParameter("@Status", newStatus),
                        new SqlParameter("@OrderId", orderId)
                    };

                    int result = db.ExecuteNonQuery(updateQuery, parameters);

                    if (result > 0)
                    {
                        ShowMessage($"Đã cập nhật trạng thái đơn hàng #{orderId} thành công!", "success");
                        LoadStatistics(); // Refresh statistics
                        LoadOrders(); // Refresh orders list
                    }
                    else
                    {
                        ShowMessage("Không thể cập nhật trạng thái đơn hàng!", "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Lỗi khi cập nhật trạng thái: " + ex.Message, "danger");
                }
            }
        }

        protected string GetStatusText(string status)
        {
            switch (status?.ToLower())
            {
                case "pending":
                    return "Đang chờ";
                case "processing":
                    return "Đang xử lý";
                case "completed":
                    return "Hoàn thành";
                case "cancelled":
                    return "Đã hủy";
                default:
                    return "Không xác định";
            }
        }
        protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
                if (ddlStatus != null)
                {
                    try
                    {
                        System.Data.DataRowView rowView = (System.Data.DataRowView)e.Item.DataItem;
                        if (rowView != null && rowView["status"] != DBNull.Value)
                        {
                            string currentStatus = rowView["status"].ToString();
                            if (!string.IsNullOrEmpty(currentStatus))
                            {
                                ListItem item = ddlStatus.Items.FindByValue(currentStatus);
                                if (item != null)
                                {
                                    ddlStatus.SelectedValue = currentStatus;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Set default value nếu có lỗi
                        ddlStatus.SelectedValue = "pending";
                        System.Diagnostics.Debug.WriteLine($"Error in ItemDataBound: {ex.Message}");
                    }
                }
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