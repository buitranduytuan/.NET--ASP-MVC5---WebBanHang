using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using PagedList;
using System.IO;

namespace WebSiteBanHang.Areas.Admin.Controllers
{
    public class ProductManagementController : Controller
    {

        QuanLyBanHangDBEntities db = new QuanLyBanHangDBEntities();

        public ActionResult ListProduct()
        {
            var listSP = db.SanPhams.Where(n => n.DaXoa == false);
            return View(listSP);
        }

        [ChildActionOnly]
        public ActionResult MenuProductPartialView()
        {
            var listSP = db.SanPhams.Where(n => n.DaXoa == false);
            return PartialView(listSP);
        }

        [ActionName("FindByLoaiSP-NSX")]
        public ActionResult FindByLoaiSPAndMaNSX(int maLoaiSP, int maNSX)
        {
            var listSP = db.SanPhams.Where(n => n.MaLoaiSP == maLoaiSP).Where(m => m.MaNSX == maNSX).Where(n => n.DaXoa == false);
            return View("ListProduct", listSP);
        }

        public ActionResult FindProductByLoaiSP(int maLoaiSP)
        {
            var listSP = db.SanPhams.Where(n => n.MaLoaiSP == maLoaiSP).Where(n => n.DaXoa == false);
            return View("ListProduct", listSP);
        }


        public ActionResult FindProductByName()
        {
            string keyName = Request.Form.Get("productName");
            var listSP = db.SanPhams.Where(n => n.TenSP.Contains(keyName)).Where(n => n.DaXoa == false);
            return View("ListProduct", listSP);
        }


        public ActionResult ProductDetail(int id)
        {
            var product = db.SanPhams.Where(n => n.MaSP == id).SingleOrDefault();
            return View(product);
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            ViewBag.ListMaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.ListMaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.ListMaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            var product = db.SanPhams.Where(n => n.MaSP == id).SingleOrDefault();
            return View(product);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditProductAction(SanPham sp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewBag.ThongBao = "Bạn đã Edit thành công";
                return Redirect("~/Admin/ProductManagement/ProductDetail/"+sp.MaSP);
            }
            return RedirectToAction("ListProduct");
        }

        public ActionResult DeleteProduct(int id)
        {
            var product = db.SanPhams.Where(n => n.MaSP == id).Where(n => n.DaXoa == false).SingleOrDefault();
            if (product != null)
            {
                product.DaXoa = true;
                db.SaveChanges();
            }

            return RedirectToAction("ListProduct");
        }


        [HttpGet]
        public ActionResult CreateNewProduct()
        {
            ViewBag.ListMaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.ListMaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.ListMaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }


        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateProduct(SanPham sp, HttpPostedFileBase HinhAnh)
        {
            ViewBag.ListMaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.ListMaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.ListMaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");

            //Lấy tên hình ảnh
            var fileName = Path.GetFileName(HinhAnh.FileName);
            //Lấy Hình Ảnh chuyển vào thư mục hình ảnh
            var loaiSP = db.LoaiSanPhams.Where(n => n.MaLoaiSP == sp.MaLoaiSP).SingleOrDefault();
            var nhaSX = db.NhaSanXuats.Where(n => n.MaNSX == sp.MaNSX).SingleOrDefault();
            var path = Path.Combine(Server.MapPath("~/Content/Images/" + loaiSP.TenLoai + "/" + nhaSX.TenNSX), fileName);
            // Nếu thư mục đã chứa hình ảnh đó rồi thì xuất thông báo
            if (System.IO.File.Exists(path))
            {
                ViewBag.Upload = "Hình ảnh này đã tồn tại";
                return View("CreateNewProduct", sp);
            }
            else
            {
                // Đưa hình ảnh vào thư mục
                HinhAnh.SaveAs(path);
                sp.HinhAnh = HinhAnh.FileName;
                db.SanPhams.Add(sp);
                db.SaveChanges();
                ViewBag.ThongBao = "Bạn đã tạo mới sản phẩm thành công";
                return View("ProductDetail", sp);
            }

        }

    }
}