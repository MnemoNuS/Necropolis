using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Necropolis.Models
{
	public class CatalogItemModel
	{
		public string Id { get; set; }
		public CatalogItemType Type { get; set; }
		public string Image { get; set; }
		public string ImagePreview { get; set; }
		public int Price { get; set; }
	}

	public enum CatalogItemType
	{
		Кресты,
		Памятники,
		Траурные_ленты,
		Венки,
		Гробы,
		Другие_товары
	}

}