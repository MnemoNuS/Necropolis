using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Necropolis.Models;
using Newtonsoft.Json;
using static System.Web.HttpContext;

namespace Necropolis.Helpers
{
    public class InformationManager
    {
        public const string CatalogPath = "~/JsonData/information-articles.txt";

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

        public static string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static void SaveItem(InformationItemModel model)
        {

            var catalog = GetInformationCatalog();
            var index = -1;
            var item = model;
            if (model.Id == null)
                item.Id = GenerateId();

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

        public static void ChangePublishStatus(string itemId)
        {
            var item = GetItem(itemId);
            item.Published = !item.Published;
            SaveItem(item);

        }
        public static void ChangeMenuStatus(string itemId)
        {
            var item = GetItem(itemId);
            item.AddedToMenu = !item.AddedToMenu;
            SaveItem(item);

        }
        public static List<InformationItemModel> GetMenuMembers()
        {
            var catalog = GetInformationCatalog();
            return catalog.Where(i => i.AddedToMenu).ToList();
        }

        public static void DeleteItem(string itemId)
        {
            var catalog = GetInformationCatalog();
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

        public static InformationItemModel GetItem(string itemId)
        {
            var catalog = GetInformationCatalog();
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

        public static List<InformationItemModel> GetInformationCatalog()
        {
            if (!File.Exists(Current.Server.MapPath(CatalogPath)))
            {
                File.Create(Current.Server.MapPath(CatalogPath));
            }
            var result = new List<InformationItemModel>();
            var data = File.ReadAllText(Current.Server.MapPath(CatalogPath));
            var jsonData = JsonConvert.DeserializeObject<List<InformationItemModel>>(data);
            if (jsonData != null)
                result.AddRange(jsonData);
            return result;
        }

    }
}