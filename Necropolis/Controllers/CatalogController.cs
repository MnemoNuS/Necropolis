using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Necropolis.Helpers;
using Necropolis.Models;
using Newtonsoft.Json;
using static System.IO.File;

namespace Necropolis.Controllers
{
	public class CatalogController : Controller
	{
		// GET: Catalog
		public ActionResult Index()
		{
			var model = CatalogManager.GetCatalog();
			return View(model);
		}
	}
}
