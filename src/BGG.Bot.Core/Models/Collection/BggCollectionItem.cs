using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Collection
{
	public class BggCollectionItem
	{
		[XmlAttribute("objectid")]
		public int BggId { get; set; }
		[XmlElement("name")]
		public string Name { get; set; }
		[XmlElement("yearpublished")]
		public int YearPublished { get; set; }
		[XmlElement("image")]
		public string Image { get; set; }
		[XmlElement("thumbnail")]
		public string Thumbnail { get; set; }
		[XmlElement("status")]
		public CollectionItemStatus CollectionItemStatus { get; set; }
		[XmlElement("numplays")]
		public int NumPlays { get; set; }
	}
}
