
using System;

namespace LibraryManagement.DTO
{
    public class PhieuMuonDTO
    {
        public int MaPhieu { get; set; }

        public int MaDocGia { get; set; }

        public DateTime NgayMuon { get; set; }

        public DateTime HanTra { get; set; }

        public string TrangThai { get; set; }

        public PhieuMuonDTO()
        {
            NgayMuon = DateTime.Now;
            HanTra = DateTime.Now.AddDays(7);
            TrangThai = "Đang mượn";
        }

        public PhieuMuonDTO(int maPhieu, int maDocGia,
                            DateTime ngayMuon, DateTime hanTra,
                            string trangThai)
        {
            MaPhieu = maPhieu;
            MaDocGia = maDocGia;
            NgayMuon = ngayMuon;
            HanTra = hanTra;
            TrangThai = trangThai;
        }
    }
}
