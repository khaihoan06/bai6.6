
using System.Data;
using Microsoft.Data.SqlClient;

namespace LibraryManagement.DAL
{
    public class DBConnection
    {
        private readonly string connectionString =
            @"Data Source=.;Initial Catalog=LibraryDB;Integrated Security=True;TrustServerCertificate=True";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // LAY DU LIEU
        public DataTable ExecuteQuery(
            string query,
            SqlParameter[]? parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter =
                           new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // THEM, SUA, XOA DU LIEU
        public int ExecuteNonQuery(
            string query,
            SqlParameter[]? parameters = null)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // LAY MOT GIA TRI
        public object? ExecuteScalar(
            string query,
            SqlParameter[]? parameters = null)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
