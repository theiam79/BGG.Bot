using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BGG.Bot.Core.Models.Thing
{
	public class Thing
	{
		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("thumbnail")]
		public string Thumbnail { get; set; }

		[XmlElement("image")]
		public string Image { get; set; }

		[XmlElement("name")]
		public Name Name { get; set; }

		[XmlElement("description")]
		public string Description { get; set; }

		[XmlElement("yearpublished")]
		public ValueElement YearPublished { get; set; }

		[XmlElement("minplayers")]
		public ValueElement MinPlayers { get; set; }

		[XmlElement("maxplayers")]
		public ValueElement MaxPlayers { get; set; }

		[XmlElement("poll")]
		public Poll[] Polls { get; set; }

		[XmlElement("playingtime")]
		public ValueElement PlayingTime { get; set; }

		[XmlElement("minplaytime")]
		public ValueElement MinPlaytime { get; set; }

		[XmlElement("maxplaytime")]
		public ValueElement MaxPlaytime { get; set; }

		[XmlElement("minage")]
		public ValueElement MinAge { get; set; }

		[XmlElement("link")]
		public Link[] Links { get; set; }
	}
}
