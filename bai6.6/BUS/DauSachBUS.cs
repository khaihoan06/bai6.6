
using System;
using System.Data;
using LibraryManagement.DAL;
using LibraryManagement.DTO;

namespace LibraryManagement.BUS
{
    public class DauSachBUS
    {
        private readonly DauSachDAL dal = new DauSachDAL();

        // LAY TAT CA DAU SACH
        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        // THEM DAU SACH
        public bool Insert(DauSachDTO sach)
        {
            if (sach == null)
                throw new Exception("Thông tin sách không hợp lệ!");

            if (string.IsNullOrWhiteSpace(sach.TenSach))
                throw new Exception("Tên sách không được để trống!");

            if (string.IsNullOrWhiteSpace(sach.TheLoai))
                throw new Exception("Thể loại không được để trống!");

            if (sach.SoLuongCon < 0)
                throw new Exception("Số lượng sách không được âm!");

            return dal.Insert(sach);
        }

        // CAP NHAT DAU SACH
        public bool Update(DauSachDTO sach)
        {
            if (sach == null)
                throw new Exception("Thông tin sách không hợp lệ!");

            if (sach.MaSach <= 0)
                throw new Exception("Mã sách không hợp lệ!");

            if (string.IsNullOrWhiteSpace(sach.TenSach))
                throw new Exception("Tên sách không được để trống!");

            if (string.IsNullOrWhiteSpace(sach.TheLoai))
                throw new Exception("Thể loại không được để trống!");

            if (sach.SoLuongCon < 0)
                throw new Exception("Số lượng sách không được âm!");

            return dal.Update(sach);
        }

        // XOA DAU SACH
        public bool Delete(int maSach)
        {
            if (maSach <= 0)
                throw new Exception("Mã sách không hợp lệ!");

            return dal.Delete(maSach);
        }

        // TIM KIEM SACH
        public DataTable Search(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return dal.GetAll();

            return dal.Search(tuKhoa.Trim());
        }

        // LAY SACH THEO MA
        public DataTable GetById(int maSach)
        {
            if (maSach <= 0)
                throw new Exception("Mã sách không hợp lệ!");

            return dal.GetById(maSach);
        }

        // KIEM TRA SO LUONG SACH
        public bool CheckQuantity(int maSach, int soLuong)
        {
            if (maSach <= 0 || soLuong <= 0)
                return false;

            return dal.CheckQuantity(maSach, soLuong);
        }
    }
}
