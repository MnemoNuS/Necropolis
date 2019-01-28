using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Necropolis.Helpers;

namespace Necropolis.Controllers
{
    public class InformationController : Controller
    {
        // GET: Information
        public ActionResult Information()
        {
            var model = InformationManager.GetInformationCatalog().Where(i=>i.Published).ToList();
            return View(model);
        }
    }
}