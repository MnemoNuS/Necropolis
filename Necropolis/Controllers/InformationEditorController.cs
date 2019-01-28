using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Necropolis.Helpers;
using Necropolis.Models;

namespace Necropolis.Controllers
{
    public class InformationEditorController : Controller
    {
        // GET: InformationEditor
        public ActionResult Index()
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");
            var model = InformationManager.GetInformationCatalog();
            return View(model);
        }

        // GET: InformationEditor/Details/5
        public ActionResult Details(string id)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth", Request.RequestContext.RouteData.Values);
            try
            {
                InformationManager.DeleteItem(id);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: InformationEditor/Create
        public ActionResult Create()
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");

            var model = new InformationItemModel();
            model.Id = InformationManager.GenerateId();
            return View(model);
        }

        // POST: InformationEditor/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(InformationItemModel model)
        {
            try
            {
                if (!AuthController.CheckAuth(Request.Cookies))
                    return RedirectToAction("Auth", "Auth");
                model.Date = DateTime.Now;
                InformationManager.SaveItem(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: InformationEditor/Edit/5
        public ActionResult PreView(string id)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");
            var item = InformationManager.GetItem(id);
            if (item != null)
                return View(item);
            return RedirectToAction("Index");
        }

        public ActionResult ChangeMenuStatus(string id)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");
            InformationManager.ChangeMenuStatus(id);
            return RedirectToAction("Index");
        }
        
                public ActionResult ChangePublishStatus(string id)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");
            InformationManager.ChangePublishStatus(id);
            return RedirectToAction("Index");
        }

        // GET: InformationEditor/Edit/5
        public ActionResult Edit(string id)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");
            var item = InformationManager.GetItem(id);
            if (item != null)
                return View(item);
            return RedirectToAction("Index");
        }

        // POST: InformationEditor/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(InformationItemModel model)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");
            try
            {
                model.Date = DateTime.Now;
                InformationManager.SaveItem(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: InformationEditor/Delete/5
        public ActionResult Delete(string id)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");
            try
            {
               InformationManager.DeleteItem(id);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}
