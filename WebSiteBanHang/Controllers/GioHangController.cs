using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangDBEntities db = new QuanLyBanHangDBEntities();

        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> listGH = Session["GioHang"] as List<ItemGioHang>;
            if (listGH == null)
            {
                listGH = new List<ItemGioHang>();
                Session["GioHang"] = listGH;
            }

            return listGH;
        }

        public ActionResult ThemGioHang(int maSP, string strURL)
        {
            SanPham sp = db.SanPhams.Where(n => n.MaSP == maSP).SingleOrDefault();
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<ItemGioHang> listGH = LayGioHang();

            // Kiểm tra sp này đã tồn tại trong giỏ hàng chưa
            ItemGioHang spCheck = listGH.SingleOrDefault(n => n.MaSP == maSP);

            if (spCheck != null)
            {
                
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script>alert(\"Sản phẩm đã hết hàng\")</script>");
                }

                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }

            ItemGioHang itemGioHang = new ItemGioHang(maSP);
            if (sp.SoLuongTon < itemGioHang.SoLuong)
            {
                return Content("<script>alert(\"Sản phẩm đã hết hàng\")</script>");
            }

            listGH.Add(itemGioHang);
            return Redirect(strURL);

        }


        public ActionResult ThemGioHangAjax(int maSP)
        {
            SanPham sp = db.SanPhams.Where(n => n.MaSP == maSP).SingleOrDefault();
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<ItemGioHang> listGH = LayGioHang();

            // Kiểm tra sp này đã tồn tại trong giỏ hàng chưa
            ItemGioHang spCheck = listGH.SingleOrDefault(n => n.MaSP == maSP);

            if (spCheck != null)
            {

                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script>alert(\"Sản phẩm đã hết hàng\")</script>");
                }

                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartialView", listGH);
            }

            ItemGioHang itemGioHang = new ItemGioHang(maSP);
            if (sp.SoLuongTon < itemGioHang.SoLuong)
            {
                return Content("<script>alert(\"Sản phẩm đã hết hàng\")</script>");
            }

            listGH.Add(itemGioHang);

            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartialView", listGH);

        }
        public int TinhTongSoLuong()
        {
            List<ItemGioHang> listGH = Session["GioHang"] as List<ItemGioHang>;
            if (listGH == null)
            {
                return 0;
            }
            return listGH.Sum(n=>n.SoLuong);
        }

        public decimal TinhTongTien()
        {
            List<ItemGioHang> listGH = Session["GioHang"] as List<ItemGioHang>;
            if (listGH == null)
            {
                return 0;
            }
            return listGH.Sum(n => n.ThanhTien);
        }


        // GET: GioHang
        public ActionResult XemGioHang()
        {
            var listGH = LayGioHang();
            return View(listGH);
        }


        public ActionResult GioHangPartialView()
        {
            int soLuong = TinhTongSoLuong();
            decimal tongTien = TinhTongTien();
            if (soLuong==0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
            }
            ViewBag.TongSoLuong = soLuong;
            ViewBag.TongTien = tongTien;
            List<ItemGioHang> listGH = LayGioHang();

            return PartialView(listGH);
        }

        public ActionResult SuaGioHang(int maSP, FormCollection frm)
        {

            var soLuongNew = Convert.ToInt32(frm["soLuong"]);
            if (Session["GioHang"]==null)
            {
                return View("Index", "Home");
            }

            var listGH = LayGioHang();
            ItemGioHang itemChinhSua = listGH.SingleOrDefault(n => n.MaSP == maSP);
            if (itemChinhSua == null)
            {
                return View("Index", "Home");
            }

            itemChinhSua.SoLuong = soLuongNew;
            itemChinhSua.ThanhTien = itemChinhSua.DonGia * soLuongNew;

            Session["GioHang"] = listGH;
            return View("XemGioHang", listGH);
        }

        public ActionResult XoaSanPhamTrongGioHang(int maSP)
        {
            if (Session["GioHang"] == null)
            {
                return View("Index", "Home");
            }

            var listGH = LayGioHang();

            ItemGioHang itemCanXoa = listGH.SingleOrDefault(n => n.MaSP == maSP);
            if (itemCanXoa == null)
            {
                return View("Index", "Home");
            }

            listGH.Remove(itemCanXoa);

            Session["GioHang"] = listGH;
            return View("XemGioHang", listGH);
        }


        public ActionResult DatHang(KhachHang kh)
        {
            KhachHang khachHang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                khachHang = kh;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
            }
            else {
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khachHang.TenKH = tv.HoTen;
                khachHang.DiaChi = tv.DiaChi;
                khachHang.Email = tv.Email;
                khachHang.SoDienThoai = tv.SoDienThoai;
                khachHang.MaThanhVien = tv.MaThanhVien;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
            }
            

            // Thêm đơn đặt hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khachHang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();

            // Thêm chi tiết đơn đặt hàng
            List<ItemGioHang> listGH = LayGioHang();
            foreach (var item in listGH)
            {
                ChiTietDonDatHang chiTietDatHang = new ChiTietDonDatHang();
                chiTietDatHang.MaSP = item.MaSP;
                chiTietDatHang.MaDDH = ddh.MaDDH;
                chiTietDatHang.TenSP = item.TenSP;
                chiTietDatHang.SoLuong = item.SoLuong;
                chiTietDatHang.DonGia = item.DonGia;

                db.ChiTietDonDatHangs.Add(chiTietDatHang);
                var sp = db.SanPhams.SingleOrDefault(n=> n.MaSP == item.MaSP);
                sp.SoLanMua += item.SoLuong;
                sp.SoLuongTon -= item.SoLuong;
                db.SaveChanges();

            }

            Session["GioHang"] = null;
            return Redirect("XemGioHang");
        }

    }
}