using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleMvcSitemap;

namespace Necropolis.Controllers
{
	public class SitemapController : Controller
	{
		// GET: Sitemap
		public ActionResult Index()
		{
			List<SitemapNode> nodes = new List<SitemapNode>
			{
				new SitemapNode( Url.Action( "Index", "Home" ) ),
				new SitemapNode( Url.Action( "Contact", "Home" ) ),
				new SitemapNode( Url.Action( "RitualHalls", "Services" ) ),
				new SitemapNode( Url.Action( "RitualServices", "Services" ) ),
				new SitemapNode( Url.Action( "RitualServices", "Services", new { set = "muslim" } ) ),
				new SitemapNode( Url.Action( "RitualServices", "Services", new { set = "embalming" } ) ),
				new SitemapNode( Url.Action( "RitualServices", "Services", new { set = "cremation" } ) ),
				new SitemapNode( Url.Action( "RitualServices", "Services", new { set = "transport" } ) ),
				new SitemapNode( Url.Action( "RitualServices", "Services", new { set = "care" } ) ),
				new SitemapNode( Url.Action( "RitualServices", "Services", new { set = "contract" } ) ),
				new SitemapNode( Url.Action( "FuneralSets", "Services" ) ),
				new SitemapNode( Url.Action( "FuneralSets", "Services", new { set = "basic" } ) ),
				new SitemapNode( Url.Action( "FuneralSets", "Services", new { set = "extended" } ) ),
				new SitemapNode( Url.Action( "FuneralSets", "Services", new { set = "premium" } ) ),
				new SitemapNode( Url.Action( "Index", "Catalog" ) ),
				new SitemapNode( Url.Action( "Index", "Catalog", new { type = "кресты" } ) ),
				new SitemapNode( Url.Action( "Index", "Catalog", new { type = "памятники" } ) ),
				new SitemapNode( Url.Action( "Index", "Catalog", new { type = "траурные_ленты" } ) ),
				new SitemapNode( Url.Action( "Index", "Catalog", new { type = "венки" } ) ),
				new SitemapNode( Url.Action( "Index", "Catalog", new { type = "гробы" } ) ),
				new SitemapNode( Url.Action( "Index", "Catalog", new { type = "другие_товары" } ) ),
				new SitemapNode( Url.Action( "Information", "Information" ) ),
				new SitemapNode( Url.Action( "Information", "Information", new { set = "сertificate_of_death" } ) ),
				new SitemapNode( Url.Action( "Information", "Information", new { set = "master_of_ceremonies" } ) ),
				new SitemapNode( Url.Action( "Information", "Information", new { set = "orthodox_funeral" } ) ),
				new SitemapNode( Url.Action( "Information", "Information", new { set = "muslim_funeral" } ) )

			};
			return new SitemapProvider().CreateSitemap( new SitemapModel( nodes ) );
		}
	}
}