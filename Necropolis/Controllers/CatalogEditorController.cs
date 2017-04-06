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
			var model = CatalogManager.GetCatalog();
			return View(model);
		}

		// GET: Catalog/Details/5
		public ActionResult Details(string id)
		{
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
			var item = CatalogManager.GetItem(id);
			if (item != null)
				return View(item);
			return RedirectToAction("Index");
		}

		// POST: Catalog/Edit/5
		[HttpPost]
		public ActionResult Edit(CatalogItemModel model)
		{
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

		public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
		{
			var ratioX = (double)maxWidth / image.Width;
			var ratioY = (double)maxHeight / image.Height;
			var ratio = Math.Min(ratioX, ratioY);

			var newWidth = (int)(image.Width * ratio);
			var newHeight = (int)(image.Height * ratio);

			var newImage = new Bitmap(newWidth, newHeight);

			using (var graphics = Graphics.FromImage(newImage))
				graphics.DrawImage(image, 0, 0, newWidth, newHeight);

			return newImage;
		}


		static Image ScaleImageTo(Image source, int width, int height)
		{
			Image dest = new Bitmap(width, height);
			using (Graphics gr = Graphics.FromImage(dest))
			{
				gr.FillRectangle(Brushes.Transparent, 0, 0, width, height);  // Очищаем экран
				gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

				float srcwidth = source.Width;
				float srcheight = source.Height;
				float dstwidth = width;
				float dstheight = height;

				if (srcwidth <= dstwidth && srcheight <= dstheight)  // Исходное изображение меньше целевого
				{
					int left = (width - source.Width) / 2;
					int top = (height - source.Height) / 2;
					gr.DrawImage(source, left, top, source.Width, source.Height);
				}
				else if (srcwidth / srcheight > dstwidth / dstheight)  // Пропорции исходного изображения более широкие
				{
					float cy = srcheight / srcwidth * dstwidth;
					float top = ((float)dstheight - cy) / 2.0f;
					if (top < 1.0f) top = 0;
					gr.DrawImage(source, 0, top, dstwidth, cy);
				}
				else  // Пропорции исходного изображения более узкие
				{
					float cx = srcwidth / srcheight * dstheight;
					float left = ((float)dstwidth - cx) / 2.0f;
					if (left < 1.0f) left = 0;
					gr.DrawImage(source, left, 0, cx, dstheight);
				}

				return dest;
			}
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
						var fileName = "IMG" + id +Path.GetExtension(fileContent.FileName);
						var path = Path.Combine(Server.MapPath("~/Content/images/catalog"), fileName);

							using( var image = System.Drawing.Image.FromStream(stream) )
							{
								using( var newImage = ScaleImage(image, 400, 300) )
								{
								image.Save( path );
								newImage.Save( path + ".[small]" + Path.GetExtension(fileContent.FileName));
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
	}
}
