
using System.Data;
using Microsoft.Data.SqlClient;
using LibraryManagement.DTO;

namespace LibraryManagement.DAL
{
    public class DauSachDAL
    {
        private readonly DBConnection db = new DBConnection();

        // LAY TAT CA DAU SACH
        public DataTable GetAll()
        {
            string sql = "SELECT * FROM DauSach ORDER BY MaSach DESC";
            return db.ExecuteQuery(sql);
        }

        // TIM SACH THEO MA
        public DataTable GetById(int maSach)
        {
            string sql = "SELECT * FROM DauSach WHERE MaSach = @MaSach";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaSach", maSach)
            };

            return db.ExecuteQuery(sql, parameters);
        }

        // THEM DAU SACH
        public bool Insert(DauSachDTO sach)
        {
            string sql = @"INSERT INTO DauSach
                           (TenSach, TheLoai, SoLuongCon)
                           VALUES
                           (@TenSach, @TheLoai, @SoLuongCon)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@TenSach", sach.TenSach),
                new SqlParameter("@TheLoai", sach.TheLoai),
                new SqlParameter("@SoLuongCon", sach.SoLuongCon)
            };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        // SUA DAU SACH
        public bool Update(DauSachDTO sach)
        {
            string sql = @"UPDATE DauSach
                           SET TenSach = @TenSach,
                               TheLoai = @TheLoai,
                               SoLuongCon = @SoLuongCon
                           WHERE MaSach = @MaSach";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaSach", sach.MaSach),
                new SqlParameter("@TenSach", sach.TenSach),
                new SqlParameter("@TheLoai", sach.TheLoai),
                new SqlParameter("@SoLuongCon", sach.SoLuongCon)
            };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        // XOA DAU SACH
        public bool Delete(int maSach)
        {
            string sql = "DELETE FROM DauSach WHERE MaSach = @MaSach";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaSach", maSach)
            };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        // TIM KIEM SACH THEO TEN
        public DataTable Search(string tenSach)
        {
            string sql = @"SELECT * FROM DauSach
                           WHERE TenSach LIKE @TenSach";

            SqlParameter[] parameters =
            {
                new SqlParameter("@TenSach", "%" + tenSach + "%")
            };

            return db.ExecuteQuery(sql, parameters);

        }


        public bool CheckQuantity(int maSach, int soLuong)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM DauSach
        WHERE MaSach = @MaSach
          AND SoLuongCon >= @SoLuong";

            Microsoft.Data.SqlClient.SqlParameter[] parameters =
            {
        new Microsoft.Data.SqlClient.SqlParameter("@MaSach", maSach),
        new Microsoft.Data.SqlClient.SqlParameter("@SoLuong", soLuong)
    };

            return Convert.ToInt32(
                db.ExecuteScalar(sql, parameters)) > 0;

        }
    }
}
