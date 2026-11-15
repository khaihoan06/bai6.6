
namespace LibraryManagement.DTO
{
    public class ChiTietMuonDTO
    {
        public int MaPhieu { get; set; }

        public int MaSach { get; set; }

        public int SoLuong { get; set; }

        public ChiTietMuonDTO()
        {
            SoLuong = 1;
        }

        public ChiTietMuonDTO(int maPhieu, int maSach, int soLuong)
        {
            MaPhieu = maPhieu;
            MaSach = maSach;
            SoLuong = soLuong;
        }
    }
}
