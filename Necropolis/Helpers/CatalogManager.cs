using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Necropolis.Models;
using Newtonsoft.Json;
using static System.Web.HttpContext;

namespace Necropolis.Helpers
{
	public class CatalogManager
	{
		public const string CatalogPath = "~/JsonData/catalog.txt";

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
				catalog.RemoveAt(index);
			}
			string jsonData = JsonConvert.SerializeObject(catalog, Formatting.Indented);
			File.WriteAllText(Current.Server.MapPath(CatalogPath), jsonData);
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

	}
}