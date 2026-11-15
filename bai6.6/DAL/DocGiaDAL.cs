
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace LibraryManagement.DAL
{
    public class DocGiaDAL
    {
        private readonly DBConnection db = new DBConnection();

        // LAY DANH SACH DOC GIA
        public DataTable GetAll()
        {
            string sql = @"
                SELECT MaDocGia, HoTen, Email, NgayHetHanThe
                FROM DocGia
                ORDER BY MaDocGia DESC";

            return db.ExecuteQuery(sql);
        }

        // KIEM TRA EMAIL DA TON TAI
        public bool Exists(string email)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DocGia
                WHERE Email = @Email";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Email", email)
            };

            int count = Convert.ToInt32(
                db.ExecuteScalar(sql, parameters));

            return count > 0;
        }

        // KIEM TRA EMAIL TRUNG KHI CAP NHAT
        public bool Exists(string email, int maDocGia)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DocGia
                WHERE Email = @Email
                AND MaDocGia <> @MaDocGia";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@MaDocGia", maDocGia)
            };

            int count = Convert.ToInt32(
                db.ExecuteScalar(sql, parameters));

            return count > 0;
        }

        // THEM DOC GIA
        public int Insert(string hoTen, string email,
                          DateTime ngayHetHanThe)
        {
            string sql = @"
                INSERT INTO DocGia
                    (HoTen, Email, NgayHetHanThe)
                VALUES
                    (@HoTen, @Email, @NgayHetHanThe)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@HoTen", hoTen),
                new SqlParameter("@Email", email),
                new SqlParameter("@NgayHetHanThe", ngayHetHanThe)
            };

            return db.ExecuteNonQuery(sql, parameters);
        }

        // CAP NHAT DOC GIA
        public int Update(int maDocGia, string hoTen,
                          string email, DateTime ngayHetHanThe)
        {
            string sql = @"
                UPDATE DocGia
                SET HoTen = @HoTen,
                    Email = @Email,
                    NgayHetHanThe = @NgayHetHanThe
                WHERE MaDocGia = @MaDocGia";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaDocGia", maDocGia),
                new SqlParameter("@HoTen", hoTen),
                new SqlParameter("@Email", email),
                new SqlParameter("@NgayHetHanThe", ngayHetHanThe)
            };

            return db.ExecuteNonQuery(sql, parameters);
        }

        // XOA DOC GIA
        public int Delete(int maDocGia)
        {
            string sql = @"
                DELETE FROM DocGia
                WHERE MaDocGia = @MaDocGia";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaDocGia", maDocGia)
            };

            return db.ExecuteNonQuery(sql, parameters);
        }

        // TIM KIEM DOC GIA
        public DataTable Search(string keyword)
        {
            string sql = @"
                SELECT MaDocGia, HoTen, Email, NgayHetHanThe
                FROM DocGia
                WHERE HoTen LIKE @Keyword
                   OR Email LIKE @Keyword
                   OR CAST(MaDocGia AS NVARCHAR(20)) LIKE @Keyword
                ORDER BY MaDocGia DESC";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Keyword", "%" + keyword + "%")
            };

            return db.ExecuteQuery(sql, parameters);
        }

        // KIEM TRA THE DOC GIA CON HAN
        public bool IsCardValid(int maDocGia)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DocGia
                WHERE MaDocGia = @MaDocGia
                  AND NgayHetHanThe >= GETDATE()";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaDocGia", maDocGia)
            };

            int count = Convert.ToInt32(
                db.ExecuteScalar(sql, parameters));

            return count > 0;
        }
    }
}
