
using System;
using System.Collections.Generic;
using System.Data;
using LibraryManagement.DAL;
using LibraryManagement.DTO;

namespace LibraryManagement.BUS
{
    public class PhieuMuonBUS
    {
        private readonly PhieuMuonDAL dal = new PhieuMuonDAL();

        // LAY TAT CA PHIEU MUON
        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        // LOC PHIEU MUON
        public DataTable Filter(int maDocGia, string trangThai)
        {
            if (maDocGia < 0)
                throw new Exception("Mã độc giả không hợp lệ!");

            if (string.IsNullOrWhiteSpace(trangThai))
                trangThai = "Tất cả";

            return dal.Filter(maDocGia, trangThai);
        }

        // LAP PHIEU MUON
        public bool Insert(
            PhieuMuonDTO phieu,
            List<ChiTietMuonDTO> chiTiet)
        {
            if (phieu == null)
                throw new Exception(
                    "Thông tin phiếu mượn không hợp lệ!");

            if (phieu.MaDocGia <= 0)
                throw new Exception("Vui lòng chọn độc giả!");

            if (phieu.HanTra.Date <= DateTime.Today)
                throw new Exception(
                    "Hạn trả phải lớn hơn ngày hôm nay!");

            if (chiTiet == null || chiTiet.Count == 0)
                throw new Exception(
                    "Vui lòng chọn sách cần mượn!");

            foreach (ChiTietMuonDTO ct in chiTiet)
            {
                if (ct == null)
                    throw new Exception(
                        "Chi tiết mượn không hợp lệ!");

                if (ct.MaSach <= 0)
                    throw new Exception("Mã sách không hợp lệ!");

                if (ct.SoLuong <= 0)
                    throw new Exception(
                        "Số lượng mượn phải lớn hơn 0!");
            }

            return dal.Insert(phieu, chiTiet);
        }

        // TRA SACH
        public bool ReturnBook(int maPhieu)
        {
            if (maPhieu <= 0)
                throw new Exception("Mã phiếu không hợp lệ!");

            return dal.ReturnBook(maPhieu);
        }

        // LAY DANH SACH PHIEU QUA HAN
        public DataTable GetOverdue()
        {
            return dal.GetOverdue();
        }

        // CAP NHAT TRANG THAI QUA HAN
        public void UpdateOverdueStatus()
        {
            dal.UpdateOverdueStatus();
        }

        // THONG KE THEO THE LOAI
        public DataTable ThongKeTheLoai()
        {
            return dal.ThongKeTheLoai();
        }

        // LAY CHI TIET PHIEU MUON
        public DataTable GetChiTiet(int maPhieu)
        {
            if (maPhieu <= 0)
                throw new Exception("Mã phiếu không hợp lệ!");

            return dal.GetChiTiet(maPhieu);
        }
    }
}
