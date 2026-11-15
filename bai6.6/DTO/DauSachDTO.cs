
namespace LibraryManagement.DTO
{
    public class DauSachDTO
    {
        public int MaSach { get; set; }

        public string TenSach { get; set; }

        public string TheLoai { get; set; }

        public int SoLuongCon { get; set; }

        public DauSachDTO()
        {
            TenSach = "";
            TheLoai = "";
            SoLuongCon = 0;
        }

        public DauSachDTO(int maSach, string tenSach,
                          string theLoai, int soLuongCon)
        {
            MaSach = maSach;
            TenSach = tenSach;
            TheLoai = theLoai;
            SoLuongCon = soLuongCon;
        }
    }
}
