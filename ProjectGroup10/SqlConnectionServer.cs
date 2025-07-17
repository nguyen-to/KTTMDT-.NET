using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectGroup10
{
    public class SqlConnectionServer
    {
        string path = HttpContext.Current.Server.MapPath("~/App_Data/DBNT.mdf");
        SqlConnection SqlConnection;
        public SqlConnectionServer()
        {
            SqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True");
        }

        // Phương thức ExecuteNonQuery - dùng cho INSERT, UPDATE, DELETE
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            int result = 0;
            try
            {
                if (SqlConnection.State == ConnectionState.Closed)
                    SqlConnection.Open();

                using (SqlCommand command = new SqlCommand(query, SqlConnection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    result = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thực thi câu lệnh: " + ex.Message);
            }
            finally
            {
                if (SqlConnection.State == ConnectionState.Open)
                    SqlConnection.Close();
            }
            return result;
        }

        // Phương thức ExecuteQuery - dùng cho SELECT, trả về DataTable
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                if (SqlConnection.State == ConnectionState.Closed)
                    SqlConnection.Open();

                using (SqlCommand command = new SqlCommand(query, SqlConnection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thực thi truy vấn: " + ex.Message);
            }
            finally
            {
                if (SqlConnection.State == ConnectionState.Open)
                    SqlConnection.Close();
            }
            return dataTable;
        }

        // Phương thức ExecuteScalar - dùng cho truy vấn trả về một giá trị duy nhất
        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            object result = null;
            try
            {
                if (SqlConnection.State == ConnectionState.Closed)
                    SqlConnection.Open();

                using (SqlCommand command = new SqlCommand(query, SqlConnection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    result = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thực thi truy vấn scalar: " + ex.Message);
            }
            finally
            {
                if (SqlConnection.State == ConnectionState.Open)
                    SqlConnection.Close();
            }
            return result;
        }
    }
}
