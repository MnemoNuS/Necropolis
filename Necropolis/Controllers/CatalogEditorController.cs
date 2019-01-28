using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Necropolis.Helpers;
using Necropolis.Models;
using Newtonsoft.Json;
using static System.IO.File;

namespace Necropolis.Controllers
{
    public class CatalogEditorController : Controller
    {
        // GET: Catalog
        public ActionResult Index()
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");

            var model = CatalogManager.GetCatalog();
            return View(model);
        }

        // GET: Catalog/Details/5
        public ActionResult Details(string id)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth", Request.RequestContext.RouteData.Values);
            try
            {
                CatalogManager.DeleteItem(id);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Catalog/Create
        public ActionResult Create()
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth", Request.RequestContext.RouteData.Values);
            var model = new CatalogItemModel();
            model.Id = CatalogManager.generateID();
            model.Image = "/Content/images/catalog/icons/no-image.png";
            model.ImagePreview = "/Content/images/catalog/icons/no-image.png";
            return View(model);
        }

        // POST: Catalog/Create
        [HttpPost]
        public ActionResult Create(CatalogItemModel model)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");
            try
            {
                CatalogManager.SaveItem(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Catalog/Edit/5
        public ActionResult Edit(string id)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");
            var item = CatalogManager.GetItem(id);
            if (item != null)
                return View(item);
            return RedirectToAction("Index");
        }

        // POST: Catalog/Edit/5
        [HttpPost]
        public ActionResult Edit(CatalogItemModel model)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth");
            try
            {
                CatalogManager.SaveItem(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Catalog/Delete/5
        public ActionResult Delete(string id)
        {
            if (!AuthController.CheckAuth(Request.Cookies))
                return RedirectToAction("Auth", "Auth" );
            try
            {
                CatalogManager.DeleteItem(id);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<JsonResult> UploadFile(string id)
        {

            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        var fileName = "IMG" + id + Path.GetExtension(fileContent.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/images/catalog"), fileName);

                        using (var image = System.Drawing.Image.FromStream(stream))
                        {
                            using (var newImage = CatalogManager.ScaleImageTo(image, 400, 300))
                            {
                                CatalogManager.AddWatermark(image);
                                image.Save(path);

                                CatalogManager.AddWatermark(newImage);
                                newImage.Save(path + ".[small]" + Path.GetExtension(fileContent.FileName));
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("File uploaded successfully");
        }

        //public bool CheckAuth()
        //{
        //    return Request.Cookies.Get("Necropolis") != null;
        //}
        //[HttpGet]
        //public ActionResult Auth()
        //{
        //    var model = new AuthModel();
        //    return View(model);
        //}
        //[HttpPost]
        //public ActionResult Auth(AuthModel model)
        //{
        //    if (CheckAuth())
        //        return RedirectToAction("Index");
        //    if (model.Login == "necropol" && model.Password == DateTime.Now.Day.ToString() + "pas")
        //    {
        //        HttpCookie myCookie = new HttpCookie("Necropolis");
        //        myCookie.Expires = DateTime.Now.AddDays(1d);
        //        Response.Cookies.Add(myCookie);
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Wrong = true;
        //    return View();
        //}
    }
}
