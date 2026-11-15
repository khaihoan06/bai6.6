
using System;

namespace LibraryManagement.DTO
{
    public class DocGiaDTO
    {
        public int MaDocGia { get; set; }

        public string HoTen { get; set; }

        public string Email { get; set; }

        public DateTime NgayHetHanThe { get; set; }

        public DocGiaDTO()
        {
            HoTen = "";
            Email = "";
            NgayHetHanThe = DateTime.Now;
        }

        public DocGiaDTO(int maDocGia, string hoTen,
                         string email, DateTime ngayHetHanThe)
        {
            MaDocGia = maDocGia;
            HoTen = hoTen;
            Email = email;
            NgayHetHanThe = ngayHetHanThe;
        }
    }
}
