using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Collection
{
	public class BggCollectionItemStatus
	{
		[XmlAttribute("own")]
		public string Own { get; set; }

		[XmlAttribute("prevowned")]
		public string PreviouslyOwned { get; set; }

		[XmlAttribute("fortrade")]
		public string ForTrade { get; set; }

		[XmlAttribute("want")]
		public string Want { get; set; }

		[XmlAttribute("wanttoplay")]
		public string WantToPlay { get; set; }

		[XmlAttribute("wanttobuy")]
		public string WantToBuy { get; set; }

		[XmlAttribute("wishlist")]
		public string Wishlist { get; set; }

		[XmlAttribute("wishlistpriority")]
		public int WishlistPriority { get; set; } = 0;

		[XmlAttribute("preordered")]
		public string Preordered { get; set; }

		[XmlAttribute("lastmodified")]
		public string LastModified { get; set; }
	}
}
