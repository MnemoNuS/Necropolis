using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Necropolis.Models;

namespace Necropolis.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public static bool CheckAuth(HttpCookieCollection cookies)
        {
            return cookies.Get("Necropolis") != null;
        }

        [HttpGet]
        public ActionResult Auth()
        {
            if (CheckAuth(Request.Cookies))
                return RedirectToRoute("Control");
            var model = new AuthModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            Response.Cookies.Remove("Necropolis");

            if (Request.Cookies["Necropolis"] != null)
            {
                var c = new HttpCookie("Necropolis");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return RedirectToRoute("Home");
        }

        [HttpPost]
        public ActionResult Auth(AuthModel model)
        {
            if (model.Login == "necropol" && model.Password == DateTime.Now.Day.ToString() + "pas")
            {
                HttpCookie myCookie = new HttpCookie("Necropolis");
                myCookie.Expires = DateTime.Now.AddDays(1d);
                Response.Cookies.Add(myCookie);
                return RedirectToRoute("Control");
            }
            ViewBag.Wrong = true;
            return View();
        }
    }
}