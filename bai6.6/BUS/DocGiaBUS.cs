
using System;
using System.Data;
using LibraryManagement.DAL;

namespace LibraryManagement.BUS
{
    public class DocGiaBUS
    {
        private readonly DocGiaDAL dal = new DocGiaDAL();

        // LAY DANH SACH
        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        // THEM DOC GIA
        public int Insert(string hoTen, string email,
                          DateTime ngayHetHanThe)
        {
            if (string.IsNullOrWhiteSpace(hoTen))
                throw new Exception("Họ tên độc giả không được để trống!");

            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email không được để trống!");

            if (ngayHetHanThe.Date < DateTime.Now.Date)
                throw new Exception("Ngày hết hạn thẻ không hợp lệ!");

            if (dal.Exists(email))
                throw new Exception("Email này đã tồn tại!");

            return dal.Insert(hoTen.Trim(), email.Trim(),
                              ngayHetHanThe);
        }

        // CAP NHAT DOC GIA
        public int Update(int maDocGia, string hoTen,
                          string email, DateTime ngayHetHanThe)
        {
            if (maDocGia <= 0)
                throw new Exception("Mã độc giả không hợp lệ!");

            if (string.IsNullOrWhiteSpace(hoTen))
                throw new Exception("Họ tên độc giả không được để trống!");

            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email không được để trống!");

            if (ngayHetHanThe.Date < DateTime.Now.Date)
                throw new Exception("Ngày hết hạn thẻ không hợp lệ!");

            if (dal.Exists(email.Trim(), maDocGia))
                throw new Exception("Email này đã được sử dụng!");

            return dal.Update(maDocGia, hoTen.Trim(),
                              email.Trim(), ngayHetHanThe);
        }

        // XOA DOC GIA
        public int Delete(int maDocGia)
        {
            if (maDocGia <= 0)
                throw new Exception("Mã độc giả không hợp lệ!");

            return dal.Delete(maDocGia);
        }

        // TIM KIEM
        public DataTable Search(string keyword)
        {
            return dal.Search(keyword ?? "");
        }

        // KIEM TRA EMAIL
        public bool Exists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return dal.Exists(email.Trim());
        }

        // KIEM TRA HAN THE
        public bool IsCardValid(int maDocGia)
        {
            if (maDocGia <= 0)
                return false;

            return dal.IsCardValid(maDocGia);
        }
    }
}
