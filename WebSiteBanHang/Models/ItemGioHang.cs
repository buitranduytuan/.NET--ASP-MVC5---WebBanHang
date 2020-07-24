using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSiteBanHang.Models
{
    public class ItemGioHang
    {
        

        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }

        public ItemGioHang(int maSP)
        {
            using (QuanLyBanHangDBEntities db = new QuanLyBanHangDBEntities())
            {
                SanPham sp = db.SanPhams.Where(n=>n.MaSP==maSP).SingleOrDefault();
                MaSP = sp.MaSP;
                TenSP = sp.TenSP;
                SoLuong = 1;
                DonGia = sp.DonGia.Value;
                ThanhTien = DonGia * SoLuong;
                HinhAnh = sp.HinhAnh;
            }
               
        }
        public ItemGioHang(int maSP, int soLuong)
        {
            using (QuanLyBanHangDBEntities db = new QuanLyBanHangDBEntities())
            {
                SanPham sp = db.SanPhams.Where(n => n.MaSP == maSP).SingleOrDefault();
                MaSP = sp.MaSP;
                TenSP = sp.TenSP;
                SoLuong = soLuong;
                DonGia = sp.DonGia.Value;
                ThanhTien = DonGia * soLuong;
                HinhAnh = sp.HinhAnh;
            }

        }
        public ItemGioHang()
        {
            SoLuong = 1;
        }
    }
}