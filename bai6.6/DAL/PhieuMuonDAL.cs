
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using LibraryManagement.DTO;

namespace LibraryManagement.DAL
{
    public class PhieuMuonDAL
    {
        private readonly DBConnection db = new DBConnection();

        // =========================================
        // HAM LAY DU LIEU
        // =========================================
        private DataTable ExecuteQuery(
            string sql, SqlParameter[]? parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt;
        }

        // =========================================
        // LAY TAT CA PHIEU MUON
        // =========================================
        public DataTable GetAll()
        {
            string sql = @"
                SELECT
                    p.MaPhieu,
                    p.MaDocGia,
                    d.HoTen,
                    p.NgayMuon,
                    p.HanTra,
                    p.TrangThai
                FROM PhieuMuon p
                INNER JOIN DocGia d
                    ON p.MaDocGia = d.MaDocGia
                ORDER BY p.MaPhieu DESC";

            return ExecuteQuery(sql);
        }

        // =========================================
        // LOC PHIEU THEO DOC GIA VA TRANG THAI
        // =========================================
        public DataTable Filter(int maDocGia, string trangThai)
        {
            string sql = @"
                SELECT
                    p.MaPhieu,
                    p.MaDocGia,
                    d.HoTen,
                    p.NgayMuon,
                    p.HanTra,
                    p.TrangThai
                FROM PhieuMuon p
                INNER JOIN DocGia d
                    ON p.MaDocGia = d.MaDocGia
                WHERE
                    (@MaDocGia = 0 OR p.MaDocGia = @MaDocGia)
                    AND
                    (@TrangThai = N'Tất cả'
                    OR p.TrangThai = @TrangThai)
                ORDER BY p.MaPhieu DESC";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaDocGia", maDocGia),
                new SqlParameter("@TrangThai",
                    string.IsNullOrWhiteSpace(trangThai)
                    ? "Tất cả" : trangThai)
            };

            return ExecuteQuery(sql, parameters);
        }

        // =========================================
        // LAP PHIEU MUON
        // =========================================
        public bool Insert(
            PhieuMuonDTO phieu,
            List<ChiTietMuonDTO> chiTiet)
        {
            if (phieu == null)
                throw new Exception("Thông tin phiếu mượn không hợp lệ!");

            if (chiTiet == null || chiTiet.Count == 0)
                throw new Exception("Phiếu mượn phải có ít nhất một cuốn sách!");

            foreach (ChiTietMuonDTO ct in chiTiet)
            {
                if (ct.SoLuong <= 0)
                    throw new Exception("Số lượng mượn phải lớn hơn 0!");
            }

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // KIEM TRA THE DOC GIA
                        string checkDocGia = @"
                            SELECT COUNT(*)
                            FROM DocGia
                            WHERE MaDocGia = @MaDocGia
                            AND NgayHetHanThe >= GETDATE()";

                        using (SqlCommand cmd = new SqlCommand(
                            checkDocGia, conn, tran))
                        {
                            cmd.Parameters.Add("@MaDocGia",
                                SqlDbType.Int).Value = phieu.MaDocGia;

                            int result = Convert.ToInt32(
                                cmd.ExecuteScalar());

                            if (result == 0)
                                throw new Exception(
                                    "Độc giả không tồn tại hoặc đã hết hạn thẻ!");
                        }

                        // TAO PHIEU MUON
                        string insertPhieu = @"
                            INSERT INTO PhieuMuon
                                (MaDocGia, NgayMuon, HanTra, TrangThai)
                            VALUES
                                (@MaDocGia, GETDATE(), @HanTra, N'Đang mượn');

                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int maPhieu;

                        using (SqlCommand cmd = new SqlCommand(
                            insertPhieu, conn, tran))
                        {
                            cmd.Parameters.Add("@MaDocGia",
                                SqlDbType.Int).Value = phieu.MaDocGia;

                            cmd.Parameters.Add("@HanTra",
                                SqlDbType.DateTime).Value = phieu.HanTra;

                            maPhieu = Convert.ToInt32(
                                cmd.ExecuteScalar());
                        }

                        // THEM CHI TIET VA TRU KHO
                        foreach (ChiTietMuonDTO ct in chiTiet)
                        {
                            // Tru so luong sach truoc
                            string updateSach = @"
                                UPDATE DauSach
                                SET SoLuongCon = SoLuongCon - @SoLuong
                                WHERE MaSach = @MaSach
                                AND SoLuongCon >= @SoLuong";

                            using (SqlCommand cmd = new SqlCommand(
                                updateSach, conn, tran))
                            {
                                cmd.Parameters.Add("@MaSach",
                                    SqlDbType.Int).Value = ct.MaSach;

                                cmd.Parameters.Add("@SoLuong",
                                    SqlDbType.Int).Value = ct.SoLuong;

                                int rows = cmd.ExecuteNonQuery();

                                if (rows == 0)
                                    throw new Exception(
                                        "Sách không tồn tại hoặc không đủ số lượng!");
                            }

                            // Them chi tiet phieu
                            string insertChiTiet = @"
                                INSERT INTO ChiTietMuon
                                    (MaPhieu, MaSach, SoLuong)
                                VALUES
                                    (@MaPhieu, @MaSach, @SoLuong)";

                            using (SqlCommand cmd = new SqlCommand(
                                insertChiTiet, conn, tran))
                            {
                                cmd.Parameters.Add("@MaPhieu",
                                    SqlDbType.Int).Value = maPhieu;

                                cmd.Parameters.Add("@MaSach",
                                    SqlDbType.Int).Value = ct.MaSach;

                                cmd.Parameters.Add("@SoLuong",
                                    SqlDbType.Int).Value = ct.SoLuong;

                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        // =========================================
        // TRA SACH
        // =========================================
        public bool ReturnBook(int maPhieu)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Kiem tra va cap nhat trang thai phieu
                        string updatePhieu = @"
                            UPDATE PhieuMuon
                            SET TrangThai = N'Đã trả'
                            WHERE MaPhieu = @MaPhieu
                            AND TrangThai IN (N'Đang mượn', N'Quá hạn')";

                        using (SqlCommand cmd = new SqlCommand(
                            updatePhieu, conn, tran))
                        {
                            cmd.Parameters.Add("@MaPhieu",
                                SqlDbType.Int).Value = maPhieu;

                            int rows = cmd.ExecuteNonQuery();

                            if (rows == 0)
                                throw new Exception(
                                    "Phiếu không tồn tại hoặc đã được trả!");
                        }

                        // Cong lai so luong sach
                        string updateSach = @"
                            UPDATE ds
                            SET ds.SoLuongCon =
                                ds.SoLuongCon + ct.SoLuong
                            FROM DauSach ds
                            INNER JOIN ChiTietMuon ct
                                ON ds.MaSach = ct.MaSach
                            WHERE ct.MaPhieu = @MaPhieu";

                        using (SqlCommand cmd = new SqlCommand(
                            updateSach, conn, tran))
                        {
                            cmd.Parameters.Add("@MaPhieu",
                                SqlDbType.Int).Value = maPhieu;

                            int rows = cmd.ExecuteNonQuery();

                            if (rows == 0)
                                throw new Exception(
                                    "Không tìm thấy chi tiết sách của phiếu mượn!");
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        // =========================================
        // LAY DANH SACH PHIEU QUA HAN
        // SU DUNG SQLDATAREADER
        // =========================================
        public DataTable GetOverdue()
        {
            string sql = @"
                SELECT
                    p.MaPhieu,
                    p.MaDocGia,
                    d.HoTen,
                    p.NgayMuon,
                    p.HanTra,
                    p.TrangThai,
                    DATEDIFF(DAY, p.HanTra, GETDATE())
                        AS SoNgayQuaHan
                FROM PhieuMuon p
                INNER JOIN DocGia d
                    ON p.MaDocGia = d.MaDocGia
                WHERE p.HanTra < GETDATE()
                AND p.TrangThai <> N'Đã trả'
                ORDER BY p.HanTra ASC";

            return ExecuteQuery(sql);
        }

        // =========================================
        // CAP NHAT TRANG THAI QUA HAN
        // =========================================
        public void UpdateOverdueStatus()
        {
            string sql = @"
                UPDATE PhieuMuon
                SET TrangThai = N'Quá hạn'
                WHERE HanTra < GETDATE()
                AND TrangThai = N'Đang mượn'";

            using (SqlConnection conn = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // =========================================
        // THONG KE LUOT MUON THEO THE LOAI
        // =========================================
        public DataTable ThongKeTheLoai()
        {
            string sql = @"
                SELECT
                    ds.TheLoai,
                    SUM(ct.SoLuong) AS TongLuotMuon
                FROM ChiTietMuon ct
                INNER JOIN DauSach ds
                    ON ct.MaSach = ds.MaSach
                GROUP BY ds.TheLoai
                ORDER BY TongLuotMuon DESC";

            return ExecuteQuery(sql);
        }

        // =========================================
        // LAY CHI TIET CUA MOT PHIEU
        // =========================================
        public DataTable GetChiTiet(int maPhieu)
        {
            string sql = @"
                SELECT
                    ct.MaPhieu,
                    ct.MaSach,
                    ds.TenSach,
                    ct.SoLuong
                FROM ChiTietMuon ct
                INNER JOIN DauSach ds
                    ON ct.MaSach = ds.MaSach
                WHERE ct.MaPhieu = @MaPhieu";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaPhieu", maPhieu)
            };

            return ExecuteQuery(sql, parameters);
        }
    }
}
