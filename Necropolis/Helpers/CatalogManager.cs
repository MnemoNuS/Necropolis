using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using Necropolis.Models;
using Newtonsoft.Json;
using static System.Web.HttpContext;
using Image = System.Drawing.Image;

namespace Necropolis.Helpers
{
    public class CatalogManager
    {
        public const string CatalogPath = "~/JsonData/catalog.txt";
        public const string LogoPath = "~/Content/images/logo.png";

        private static long lastTimeStamp = DateTime.UtcNow.Ticks;

        public static long UtcNowTicks
        {
            get
            {
                long original, newValue;
                do
                {
                    original = lastTimeStamp;
                    long now = DateTime.UtcNow.Ticks;
                    newValue = Math.Max(now, original + 1);
                } while (Interlocked.CompareExchange
                             (ref lastTimeStamp, newValue, original) != original);

                return newValue;
            }
        }

        public static string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static void SaveItem(CatalogItemModel model)
        {

            var catalog = GetCatalog();
            var index = -1;
            var item = model;
            if (model.Id == null)
                item.Id = generateID();

            if (catalog.Count > 0)
            {
                foreach (var itemModel in catalog)
                {
                    if (itemModel.Id.Trim() == item.Id.Trim())
                    {
                        index = catalog.IndexOf(itemModel);
                    }
                }
            }
            if (index >= 0)
            {
                catalog[index] = item;
            }
            else
            {
                catalog.Add(item);
            }
            string jsonData = JsonConvert.SerializeObject(catalog, Formatting.Indented);
            File.WriteAllText(Current.Server.MapPath(CatalogPath), jsonData);
        }

        public static void DeleteItem(string itemId)
        {
            var catalog = GetCatalog();
            var index = -1;
            if (catalog.Count > 0)
            {
                foreach (var itemModel in catalog)
                {
                    if (itemModel.Id.Trim() == itemId.Trim())
                    {
                        index = catalog.IndexOf(itemModel);
                    }
                }
            }
            if (index >= 0)
            {
                var deletedItem = catalog.ElementAt(index);
                DeleteImg(deletedItem.Image);
                DeleteImg(deletedItem.ImagePreview);
                catalog.RemoveAt(index);
            }
            string jsonData = JsonConvert.SerializeObject(catalog, Formatting.Indented);
            File.WriteAllText(Current.Server.MapPath(CatalogPath), jsonData);
        }

        public static void DeleteImg(string path)
        {
            var serverPath = Current.Server.MapPath(path);
            if (serverPath != null && File.Exists(serverPath))
                File.Delete(serverPath);
        }
        public static CatalogItemModel GetItem(string itemId)
        {
            var catalog = GetCatalog();
            var index = -1;
            if (catalog.Count > 0)
            {
                foreach (var itemModel in catalog)
                {
                    if (itemModel.Id.Trim() == itemId.Trim())
                    {
                        index = catalog.IndexOf(itemModel);
                    }
                }
            }
            if (index >= 0)
            {
                return catalog[index];
            }
            return catalog[0];
        }

        public static List<CatalogItemModel> GetCatalog()
        {
            if (!File.Exists(Current.Server.MapPath(CatalogPath)))
            {
                File.Create(Current.Server.MapPath(CatalogPath));
            }
            var result = new List<CatalogItemModel>();
            var data = File.ReadAllText(Current.Server.MapPath(CatalogPath));
            var jsonData = JsonConvert.DeserializeObject<List<CatalogItemModel>>(data);
            if (jsonData != null)
                result.AddRange(jsonData);
            return result;
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

        public static Image ScaleImageTo(Image source, int width, int height)
        {
            Image dest = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(dest))
            {
                gr.FillRectangle(Brushes.Black, 0, 0, width, height);  // Очищаем экран
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

        public static void AddWatermark(Image image)
        {
            var logo = Image.FromFile(Current.Server.MapPath(LogoPath));
            logo = ScaleImage(logo, image.Width/3, image.Height/3);
            var watermark = new Bitmap(logo);
            DrawWatermark(image, watermark, WatermarkPosition.BottomRight, Color.Transparent, 1);
        }

        public static void DrawWatermark(Image original, Bitmap watermark, WatermarkPosition position, Color transparentColor, float opacity)
        {
            if (original == null)
                throw new ArgumentNullException("original");
            if (watermark == null)
                throw new ArgumentNullException("watermark");
            if (opacity < 0 || opacity > 1)
                throw new ArgumentOutOfRangeException("Watermark opacity value is out of range");

            Rectangle dest = new Rectangle(
                GetDestination(original.Size, watermark.Size, position), watermark.Size);

            using (Graphics g = Graphics.FromImage(original))
            {
                ImageAttributes attr = new ImageAttributes();
                ColorMatrix matrix = new ColorMatrix(new float[][] {
            new float[] { opacity, 0f, 0f, 0f, 0f },
            new float[] { 0f, opacity, 0f, 0f, 0f },
            new float[] { 0f, 0f, opacity, 0f, 0f },
            new float[] { 0f, 0f, 0f, opacity, 0f },
            new float[] { 0f, 0f, 0f, 0f, opacity } });
                attr.SetColorMatrix(matrix);
                //watermark.MakeTransparent(transparentColor);

                g.DrawImage(watermark, dest, 0, 0, watermark.Width, watermark.Height,
                    GraphicsUnit.Pixel, attr, null, IntPtr.Zero);
                g.Save();
            }
        }

        public enum WatermarkPosition
        {
            TopLeft = 0,
            TopRight,
            BottomLeft,
            BottomRight,
            Middle
        }

        private static Point GetDestination(Size originalSize, Size watermarkSize, WatermarkPosition position)
        {
            Point destination = new Point(0, 0);
            switch (position)
            {
                case WatermarkPosition.TopRight:
                    destination.X = originalSize.Width - watermarkSize.Width;
                    break;
                case WatermarkPosition.BottomLeft:
                    destination.Y = originalSize.Height - watermarkSize.Height;
                    break;
                case WatermarkPosition.BottomRight:
                    destination.X = originalSize.Width - watermarkSize.Width;
                    destination.Y = originalSize.Height - watermarkSize.Height;
                    break;
                case WatermarkPosition.Middle:
                    destination.X = (originalSize.Width - watermarkSize.Width) / 2;
                    destination.Y = (originalSize.Height - watermarkSize.Height) / 2;
                    break;
            }
            return destination;
        }

    }
}
