using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangDBEntities db = new QuanLyBanHangDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult HeadPartialView()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult HeaderPartialView()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult MenuPartialView()
        {
            var listSP = db.SanPhams;
            return PartialView(listSP);
        }

        [HttpPost]
        public ActionResult Login(FormCollection frm)
        {
            string tenTK = frm.Get("TenTK");
            string mk = frm.Get("MK");
            ThanhVien thanhVien = db.ThanhViens.Where(n => n.TaiKhoan.Equals(tenTK)).Where(n => n.MatKhau.Equals(mk)).SingleOrDefault();
            if (thanhVien != null)
            {
                Session.Add("TaiKhoan", thanhVien);
                return Content("<script>window.location.reload();</script>");
            }
            return Content("Tên đăng nhập hoặc mật khẩu không đúng");
        }

        public ActionResult Register(ThanhVien tv)
        {
            if (ModelState.IsValid)
            {
                db.ThanhViens.Add(tv);
                db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }


        public ActionResult LogOut(string urlCurrent)
        {
            Session.Remove("TaiKhoan");
            return Redirect(urlCurrent);
        }

        public ActionResult RegisterTest()
        {
            return View();
        }

    }
}