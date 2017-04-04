using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Necropolis.Controllers
{
    public class ServicesController : Controller
    {
        // GET: RitualServices

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FuneralSets()
        {
            return View();
        }

        public ActionResult RitualHalls()
        {
            return View();
        }
        public ActionResult RitualServices()
        {
            return View();
        }

    }
}