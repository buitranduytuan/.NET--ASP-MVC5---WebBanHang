using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSiteBanHang.Controllers
{
    public class DemoAjaxController : Controller
    {
        // GET: DemoAjax
        public ActionResult DemoAjax()
        {
            return View();
        }

        public ActionResult LoadAjaxActionLink()
        {
            System.Threading.Thread.Sleep(500);
            return Content("Hello DemoAjax");
        }

        [HttpPost]
        public ActionResult LoadAjaxBeginFrom(FormCollection frm)
        {
            System.Threading.Thread.Sleep(500);
            string name = frm.Get("txt1");
            return Content("Xin Chao " + name);
        }

        public ActionResult LoadAjaxCach3 (int a, int b)
        {
            return Content((a+b).ToString());
        }
    }
}