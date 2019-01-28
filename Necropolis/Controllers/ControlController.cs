using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Necropolis.Controllers
{
    public class ControlController : Controller
    {
        // GET: CPanel
        public ActionResult Index()
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");
            return View();
        }

    }
}