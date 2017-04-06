using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Necropolis.Models
{
	public class CatalogItemModel
	{
		public string Id { get; set; }
		public ItemType Type { get; set; }
		public string Image { get; set; }
		public string ImagePreview { get; set; }
		public int Price { get; set; }
	}

	public enum ItemType
	{
		Кресты,
		Надгробия
	}


}