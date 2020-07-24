using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using PagedList;

namespace WebSiteBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        QuanLyBanHangDBEntities db = new QuanLyBanHangDBEntities();
        // GET: SanPham

        [ActionName("FindByLoaiSP-NSX")]
        public ActionResult FindByLoaiSPAndMaNSX(int maLoaiSP, int maNSX, int? page)
        {
            List<SanPham> listSP = db.SanPhams.Where(n => n.MaLoaiSP == maLoaiSP).Where(m => m.MaNSX == maNSX).ToList();

            // Thực hiện chức năng phân trang
            // Số sản phẩm trên một trang
            int pageSize = 9;
            int pageNumner = (page ?? 1);
            ViewBag.MaLoaiSP = maLoaiSP;
            ViewBag.MaNSX = maNSX;
            ViewBag.CheckActionName = "FindByLoaiSP - NSX";

            return View("Product", listSP.OrderBy(n => n.MaSP).ToPagedList(pageNumner, pageSize));
        }

        public ActionResult FindProductByLoaiSP(int maLoaiSP, int? page)
        {
            List<SanPham> listSP = db.SanPhams.Where(n => n.MaLoaiSP == maLoaiSP).ToList();

            // Thực hiện chức năng phân trang
            // Số sản phẩm trên một trang
            int pageSize = 9;
            int pageNumner = (page ?? 1);
            ViewBag.MaLoaiSP = maLoaiSP;
            ViewBag.CheckActionName = "FindProductByLoaiSP";

            return View("Product", listSP.OrderBy(n => n.MaSP).ToPagedList(pageNumner, pageSize));
        }


        public ActionResult FindProductByName(int? page, string key)
        {
            string keyName;
            if (string.IsNullOrEmpty(key))
            {
                keyName = Request.Form.Get("productName");
            }
            else
            {
                keyName = key;
            }

            List<SanPham> listSP = db.SanPhams.Where(n => n.TenSP.Contains(keyName)).ToList();
            ViewBag.KeyName = keyName;

            // Thực hiện chức năng phân trang
            // Số sản phẩm trên một trang
            int pageSize = 9;
            int pageNumner = (page ?? 1);
            ViewBag.CheckActionName = "FindProductByName";

            return View("Product", listSP.OrderBy(n => n.MaSP).ToPagedList(pageNumner, pageSize));
        }

        [ChildActionOnly]
        public ActionResult ProductPartialView()
        {
            return PartialView();
        }

        public ActionResult ProductDetail(int id)
        {
            var product = db.SanPhams.Where(n => n.MaSP == id).SingleOrDefault();
            return View("ProductDetail", product);
        }
    }
}